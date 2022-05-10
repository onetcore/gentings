using System.Collections.Concurrent;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;

namespace Gentings.Sockets
{
    /// <summary>
    /// 套接字处理基类。
    /// </summary>
    public abstract class SocketBase
    {
        /// <summary>
        /// 当前线程包含的任务。
        /// </summary>
        private readonly ConcurrentBag<Task> _tasks = new();

        /// <summary>
        /// 取消标志源。
        /// </summary>
        public readonly CancellationTokenSource TokenSource = new();

        /// <summary>
        /// 添加后台任务。
        /// </summary>
        /// <param name="func">后台服务方法。</param>
        public void AddTask(Func<CancellationToken, Task> func)
        {
            _tasks.Add(Task.Run(async () =>
            {
                while (!TokenSource.IsCancellationRequested && Socket.Connected)
                {
                    try
                    {
                        await func(TokenSource.Token);
                    }
                    finally
                    {
                        await Task.Delay(1, TokenSource.Token);
                    }
                }
            }));
        }

        /// <summary>
        /// 初始化类<see cref="SocketBase"/>
        /// </summary>
        /// <param name="socket">套接字实例。</param>
        protected SocketBase(Socket socket)
        {
            Socket = socket;
            Local = (IPEndPoint)Socket.LocalEndPoint!;
            Remote = (IPEndPoint)Socket.RemoteEndPoint!;
        }

        /// <summary>
        /// 当前提供服务的名称。
        /// </summary>
        public virtual string Name => "client";

        /// <summary>
        /// 当前套接字实例。
        /// </summary>
        protected Socket Socket { get; }

        /// <summary>
        /// 本地IP地址和端口号。
        /// </summary>
        public IPEndPoint Local { get; }

        /// <summary>
        /// 远端IP实例和端口号。
        /// </summary>
        public IPEndPoint Remote { get; }

        /// <summary>
        /// 关闭客户端。
        /// </summary>
        public virtual Task CloseAsync()
        {
            if (Socket.Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }

            TokenSource.CancelAfter(10);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 最后活动时间。
        /// </summary>
        public DateTimeOffset Actived { get; private set; } = DateTimeOffset.Now;

        /// <summary>
        /// 启动线程进行接收数据。
        /// </summary>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回启动任务。</returns>
        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            AddTask(StartPingAsync);
            var pipe = new Pipe();
            var writing = FillPipeAsync(pipe.Writer, cancellationToken);
            var reading = ReadPipeAsync(pipe.Reader, cancellationToken);
            await Task.WhenAll(reading, writing);
        }

        /// <summary>
        /// 发送心跳包。
        /// </summary>
        /// <param name="cancellationToken">取消标识。</param>
        protected abstract Task StartPingAsync(CancellationToken cancellationToken);

        private async Task FillPipeAsync(PipeWriter writer, CancellationToken cancellationToken)
        {
            const int minimumBufferSize = 512;

            while (true)
            {
                // 从PipeWriter至少分配512字节
                var memory = writer.GetMemory(minimumBufferSize);
                try
                {
                    var reads = await Socket.ReceiveAsync(memory, SocketFlags.None, cancellationToken);
                    if (reads == 0)
                        break;

                    Actived = DateTimeOffset.Now;
                    // 告诉PipeWriter从套接字读取了多少
                    writer.Advance(reads);
                }
                catch (SocketException exception)
                {
                    LogError("[{2}] 套接字读取出现错误：{1}({0})", exception.Message, exception.SocketErrorCode, Name);
                    if (!Socket.Connected)
                    {
                        await CloseAsync();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LogError("[{0}] {1}", Name, ex.Message);
                    break;
                }

                // 标记数据可用，让PipeReader读取
                var result = await writer.FlushAsync(cancellationToken);
                if (result.IsCompleted)
                    break;
            }

            // 告诉PipeReader没有更多的数据
            await writer.CompleteAsync();
        }

        private async Task ReadPipeAsync(PipeReader reader, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await reader.ReadAsync(cancellationToken);
                var buffer = new ByteReader(result.Buffer);
                while (!buffer.IsEnd)
                {
                    try
                    {
                        if (!await ProcessAsync(buffer))
                            break;
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                        break;
                    }
                }

                // 告诉PipeReader我们已经处理多少缓冲
                reader.AdvanceTo(buffer.Start, buffer.End);
                // 如果没有更多的数据，停止都去
                if (result.IsCompleted)
                    break;
            }

            // 将PipeReader标记为完成
            await reader.CompleteAsync();
        }

        /// <summary>
        /// 解析错误日志。
        /// </summary>
        /// <param name="exception">错误实例。</param>
        protected virtual void LogError(Exception exception)
        {
        }

        /// <summary>
        /// 解析错误日志。
        /// </summary>
        /// <param name="message">错误消息。</param>
        /// <param name="args">参数。</param>
        protected virtual void LogError(string message, params object[] args)
        {
        }

        /// <summary>
        /// 从字节读取器中读取数据包，并且进行处理。
        /// </summary>
        /// <param name="reader">字节读取实例。</param>
        /// <returns>返回处理结果。</returns>
        protected abstract Task<bool> ProcessAsync(ByteReader reader);

        private readonly SemaphoreSlim _semaphore = new(1);

        /// <summary>
        /// 发送消息包。
        /// </summary>
        /// <param name="buffer">发送的包实例。</param>
        /// <returns>返回当前执行的结果。</returns>
        protected async Task<bool> SendAsync(IByteWriter buffer)
        {
            if (!Socket.Connected)
                return false;
            await using var ms = new MemoryStream();
            await using var bw = new ByteWriter(ms);
            buffer.Write(bw);
            var bytes = ms.ToArray();
            Actived = DateTimeOffset.Now;
            await _semaphore.WaitAsync(TimeSpan.FromMinutes(1));
            var count = await Socket.SendAsync(bytes, SocketFlags.None);
            _semaphore.Release();
            return count > 0;
        }

        private volatile int _seconds;
        private long _lastSecond = Cores.UnixNow;
        private static readonly object _secondsLocker = new();

        /// <summary>
        /// 同一秒钟内的并发数量。
        /// </summary>
        public int Seconds
        {
            get
            {
                lock (_secondsLocker)
                {
                    return _seconds;
                }
            }
            set
            {
                lock (_secondsLocker)
                {
                    var now = Cores.UnixNow;
                    if (_lastSecond == now)
                        _seconds += value;
                    else
                    {
                        _lastSecond = now;
                        _seconds = value;
                    }
                }
            }
        }
    }
}
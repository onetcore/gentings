using System;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Gentings.ConsoleApp
{
    /// <summary>
    /// 套接字处理基类。
    /// </summary>
    public abstract class SocketBase
    {
        /// <summary>
        /// 初始化类<see cref="SocketBase"/>
        /// </summary>
        /// <param name="socket">套接字实例。</param>
        protected SocketBase(Socket socket)
        {
            Socket = socket;
            Local = (IPEndPoint)Socket.LocalEndPoint;
            Remote = (IPEndPoint)Socket.RemoteEndPoint;
        }

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
        public virtual void Close()
        {
            if (Socket.Connected)
                Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }

        /// <summary>
        /// 最后活动时间。
        /// </summary>
        public DateTimeOffset Actived { get; private set; } = DateTimeOffset.Now;

        /// <summary>
        /// 启动线程进行接收数据。
        /// </summary>
        /// <param name="cancellation">取消标识。</param>
        /// <returns>返回启动任务。</returns>
        public async Task StartAsync(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                var pipe = new Pipe();
                var writing = FillPipeAsync(pipe.Writer);
                var reading = ReadPipeAsync(pipe.Reader);
                await Task.WhenAll(reading, writing);
            }
        }

        private async Task FillPipeAsync(PipeWriter writer)
        {
            const int minimumBufferSize = 512;

            while (true)
            {
                // 从PipeWriter至少分配512字节
                var memory = writer.GetMemory(minimumBufferSize);
                try
                {
                    var bytesRead = await Socket.ReceiveAsync(memory, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        break;
                    }

                    Actived = DateTimeOffset.Now;
                    // 告诉PipeWriter从套接字读取了多少
                    writer.Advance(bytesRead);
                }
                catch (Exception ex)
                {
                    Consoles.Error(ex.Message);
                    break;
                }

                // 标记数据可用，让PipeReader读取
                var result = await writer.FlushAsync();
                if (result.IsCompleted)
                {
                    break;
                }
            }

            // 告诉PipeReader没有更多的数据
            writer.Complete();
        }

        private async Task ReadPipeAsync(PipeReader reader)
        {
            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = new ByteReader(result.Buffer);
                do
                {
                    Actived = DateTimeOffset.Now;
                    try { await ProcessAsync(buffer); }
                    catch (Exception ex)
                    {
                        LogError(ex);
                    }
                }
                while (!buffer.IsEnd);

                // 告诉PipeReader我们已经处理多少缓冲
                reader.AdvanceTo(buffer.Start, buffer.End);

                // 如果没有更多的数据，停止都去
                if (result.IsCompleted)
                {
                    break;
                }
            }

            // 将PipeReader标记为完成
            reader.Complete();
        }

        /// <summary>
        /// 解析错误日志。
        /// </summary>
        /// <param name="exception">错误实例。</param>
        protected virtual void LogError(Exception exception)
        {
            Consoles.Error(exception.Message);
        }

        /// <summary>
        /// 从字节读取器中读取数据包，并且进行处理。
        /// </summary>
        /// <param name="reader">字节读取实例。</param>
        /// <returns>返回当前执行的任务。</returns>
        protected abstract Task ProcessAsync(ByteReader reader);

        /// <summary>
        /// 发送消息包。
        /// </summary>
        /// <param name="action">写入字节操作。</param>
        /// <returns>返回当前执行的结果。</returns>
        protected async Task<int> SendAsync(Action<ByteWriter> action)
        {
            await using var ms = new MemoryStream();
            await using var bw = new ByteWriter(ms);
            action(bw);
            return await Socket.SendAsync(ms.GetBuffer(), SocketFlags.None);
        }
    }
}

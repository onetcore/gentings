using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Pipelines;
using System.Buffers;
using Gentings.ConsoleApp;

namespace GCApp.Server
{
    /// <summary>
    /// 用户实例。
    /// </summary>
    public class Session
    {
        internal Session(Socket socket, ConcurrentDictionary<CMPPCommand, ICommand> commands, IServiceProvider serviceProvider)
        {
            _socket = socket;
            _commands = commands;
            _serviceProvider = serviceProvider;
            Remote = (IPEndPoint)socket.RemoteEndPoint;
            IPAddress = Remote.Address;
            SessionId = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 每个用户唯一Id。
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// 连接时间。
        /// </summary>
        public DateTimeOffset Connected { get; } = DateTimeOffset.Now;
        private readonly Socket _socket;
        private readonly ConcurrentDictionary<CMPPCommand, ICommand> _commands;
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 获取服务实例。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回服务实例对象。</returns>
        public TService GetService<TService>() => _serviceProvider.GetService<TService>();

        /// <summary>
        /// 获取服务实例。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回服务实例对象。</returns>
        public TService GetRequiredService<TService>() => _serviceProvider.GetRequiredService<TService>();

        /// <summary>
        /// 客户端IP地址。
        /// </summary>
        public IPAddress IPAddress { get; }

        /// <summary>
        /// 客户端IP实例。
        /// </summary>
        public IPEndPoint Remote { get; }

        /// <summary>
        /// 最后活动时间。
        /// </summary>
        public DateTimeOffset Actived { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 关闭客户端。
        /// </summary>
        public void Close()
        {
            if (_socket.Connected)
                _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

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
                Task writing = FillPipeAsync(_socket, pipe.Writer);
                Task reading = ReadPipeAsync(pipe.Reader);
                await Task.WhenAll(reading, writing);
            }
        }

        private async Task FillPipeAsync(Socket socket, PipeWriter writer)
        {
            const int minimumBufferSize = 512;

            while (true)
            {
                // 从PipeWriter至少分配512字节
                Memory<byte> memory = writer.GetMemory(minimumBufferSize);
                try
                {
                    int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
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
                var buffer = result.Buffer;
                do
                {
                    //获取当前包大小
                    var header = new PackageHeader(buffer.Slice(buffer.Start, 12).ToArray());
                    if (buffer.Length >= header.TotalLength)
                    {
                        var position = buffer.GetPosition(header.TotalLength);
                        await ProcessAsync(header, buffer.Slice(12, position).ToArray(), CancellationToken.None);
                        buffer = buffer.Slice(position);
                    }
                    else
                        break;
                }
                while (buffer.Length > 0);

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

        private async Task ProcessAsync(PackageHeader header, byte[] bytes, CancellationToken cancellationToken)
        {
            if (_commands.TryGetValue(header.CommandId, out var command))
            {
                try { await command.ExecuteAsync(this, header, bytes, cancellationToken); }
                catch (Exception ex) { Consoles.Error(ex); }
            }
        }
    }
}

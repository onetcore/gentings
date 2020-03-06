using Gentings.Messages.CMPP.Packaging;
using Gentings.Messages.CMPP.ServiceProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Gentings.Messages.CMPP.Services
{
    /// <summary>
    /// CMPP后台服务。
    /// </summary>
    /// <![CDATA[
    /// 参数C、T、N原则上应可配置，现阶段建议取值为：C=3分钟，T=60秒，N=3。
    /// ]]>
    public abstract class CMPPBackgroundService : BackgroundService
    {
        private readonly IEnumerable<ICMPPServiceProvider> _serviceProviders;
        private readonly ILogger<CMPPBackgroundService> _logger;
        private readonly ConcurrentDictionary<CMPPCommand, IServiceHandler> _serviceHandlers;
        private readonly ConcurrentDictionary<string, TcpClient> _clients = new ConcurrentDictionary<string, TcpClient>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 初始化类<see cref="CMPPBackgroundService"/>。
        /// </summary>
        /// <param name="serviceProviders">服务器提供者列表。</param>
        /// <param name="logger">日志接口。</param>
        /// <param name="serviceHandlers">服务提供者。</param>
        protected CMPPBackgroundService(IEnumerable<ICMPPServiceProvider> serviceProviders, ILogger<CMPPBackgroundService> logger, IEnumerable<IServiceHandler> serviceHandlers)
        {
            _serviceProviders = serviceProviders;
            _logger = logger;
            _serviceHandlers = new ConcurrentDictionary<CMPPCommand, IServiceHandler>(serviceHandlers.ToDictionary(x => x.Command));
        }

        /// <summary>
        /// 执行服务。
        /// </summary>
        /// <param name="stoppingToken">停止标志。</param>
        /// <returns>返回当前执行的任务。</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var serviceProvider in _serviceProviders)
                {
                    // 禁用或者已经启用略过
                    if (serviceProvider.Disabled || _clients.ContainsKey(serviceProvider.Name))
                        continue;
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    Task.Run(async () =>
                    {
                        try
                        {
                            var client = new TcpClient();
                            await client.ConnectAsync(serviceProvider.Host, serviceProvider.Port);
                            if (_clients.TryAdd(serviceProvider.Name, client))
                            {
                                var stream = client.GetStream();
                                var status = await ConnectAsync(stream, serviceProvider, stoppingToken);
                                if (status == ConnectStatus.Ok)
                                {
                                    var service = new ServiceContext(stream, serviceProvider);
                                    try { await ExecuteAsync(service, stoppingToken); }
                                    catch { await service.SendTerminalAsync(); }
                                }
                                else
                                    _logger.LogError($"连接到服务器提供商出现错误：{status}");
                            }
                        }
                        catch (SocketException exception)
                        {
                            _logger.LogError(1, exception, $"连接{serviceProvider.Name}出现错误！");
                        }
                        finally
                        {
                            if (_clients.TryRemove(serviceProvider.Name, out var client) && client.Connected)
                                client.Close();
                        }
                    }, stoppingToken);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                }
                await Task.Delay(100);
            }
        }

        private async Task ExecuteAsync(ServiceContext service, CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                await service.SendPingAsync();
                var bytes = await service.ReadBytesAsync();
                if (bytes == null)
                {
                    await Task.Delay(100);
                    continue;
                }
                var header = new PackageHeader(bytes);
                if (_serviceHandlers.TryGetValue(header.CommandId, out var serviceHandler))
                {
                    var buffer = new byte[bytes.Length - PackageHeader.Size];
                    if (buffer.Length > 0)
                        bytes.CopyTo(buffer, PackageHeader.Size);
                    try { await serviceHandler.ExecuteAsync(header, buffer); }
                    catch (Exception exception)
                    {
                        _logger.LogError(1, exception, $"处理{header.CommandId}(sid:{header.SequenceId}, length:{header.TotalLength})出错：{buffer.ToHexString()}");
                    }
                }
            }
        }

        /// <summary>
        /// 发送连接请求。
        /// </summary>
        /// <param name="stream">当前文件流。</param>
        /// <param name="serviceProvider">服务提供者实例。</param>
        /// <param name="stoppingToken">停止标志。</param>
        /// <returns>返回连接状态。</returns>
        protected async Task<ConnectStatus> ConnectAsync(NetworkStream stream, ICMPPServiceProvider serviceProvider, CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var package = new ConnectPackage(serviceProvider.Spid, serviceProvider.Password, 1);
            await stream.WritePackageAsync(package);
            var message = await stream.ReadMessageAsync<ConnectMessage>();
            if (message?.IsHeader(package.Header) != true)
                return ConnectStatus.Others;
            if (message.IsValid(serviceProvider.Password, package.AuthenticatorSource))
                return message.Status;
            return ConnectStatus.InvlaidAuthenticator;
        }
    }
}

using Gentings.Extensions.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// CMPP后台服务。
    /// </summary>
    public abstract class CMPPBackgroundService : BackgroundService
    {
        private readonly IEnumerable<ICMPPServiceProvider> _serviceProviders;
        private readonly ILogger<CMPPBackgroundService> _logger;
        private readonly ConcurrentDictionary<string, TcpClient> _clients = new ConcurrentDictionary<string, TcpClient>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 初始化类<see cref="CMPPBackgroundService"/>。
        /// </summary>
        /// <param name="serviceProviders">服务器提供者列表。</param>
        /// <param name="logger">日志接口。</param>
        protected CMPPBackgroundService(IEnumerable<ICMPPServiceProvider> serviceProviders, ILogger<CMPPBackgroundService> logger)
        {
            _serviceProviders = serviceProviders;
            _logger = logger;
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
                                await ExecuteAsync(client.GetStream(), serviceProvider, stoppingToken);
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

        /// <summary>
        /// 执行方法。
        /// </summry>
        /// <param name="stream">网络流实例。</param>
        /// <param name="serviceProvider">服务提供者实例。</param>
        /// <param name="stoppingToken">停止标志。</param>
        protected virtual async Task ExecuteAsync(NetworkStream stream, ICMPPServiceProvider serviceProvider, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //发送和接收数据包，并处理
                await Task.Delay(100);
            }
        }
    }
}

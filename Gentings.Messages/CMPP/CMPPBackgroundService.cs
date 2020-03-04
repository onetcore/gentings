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
                    Task.Run(async () =>
                    {
                        var client = new TcpClient();
                        try
                        {
                            await client.ConnectAsync(serviceProvider.Host, serviceProvider.Port);
                            if (_clients.TryAdd(serviceProvider.Name, client))
                                await ExecuteAsync(client.)
                        }
                        catch(SocketException exception)
                        {
                            client
                        }
                    }, stoppingToken);
                }
                await Task.Delay(100);
            }
        }

        protected virtual Task ExecuteAsync(TcpClient client, ICMPPServiceProvider serviceProvider)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var settings = await _settingsManager.GetSettingsAsync<CMPPSettings>();
                if (!settings.Enabled)
                {
                    await Task.Delay(100);
                    continue;
                }
                Client = new TcpClient(settings.Host, settings.Port);
                Client.Close();
            }
        }
    }
}

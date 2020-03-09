using Gentings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data.Migrations;
using System.Net.Sockets;
using System.Net;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using Gentings.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;

namespace GCApp.Server
{
    /// <summary>
    /// 主程序后台线程。
    /// </summary>
    public class ServerService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<CMPPCommand, ICommand> _commands;
        private readonly int _port;
        private readonly int _backlog;
        private readonly int _keepLiveMinutes;
        private readonly Socket _listener;
        /// <summary>
        /// 初始化类<see cref="ServerService"/>。
        /// </summary>
        /// <param name="configuration">配置实例。</param>
        /// <param name="logger">日志接口。</param>
        /// <param name="commands">命令列表。</param>
        /// <param name="serviceProvider">服务提供者接口。</param>
        public ServerService(IConfiguration configuration, ILogger<ServerService> logger, IEnumerable<ICommand> commands, IServiceProvider serviceProvider)
        {
            _port = configuration.GetValue("Server::Port", 9527);
            _backlog = configuration.GetValue("Server::Backlog", 100);
            _keepLiveMinutes = configuration.GetValue("Server::KeepLive", 3);//分钟
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _logger = logger;
            _serviceProvider = serviceProvider;
            _commands = new ConcurrentDictionary<CMPPCommand, ICommand>(commands.ToDictionary(x => x.CMPPCommand));
        }

        /// <summary>
        /// 连接的客户端。
        /// </summary>
        protected ConcurrentDictionary<string, Session> Sessions { get; } = new ConcurrentDictionary<string, Session>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 运行任务。
        /// </summary>
        /// <param name="stoppingToken">停止标识。</param>
        /// <returns>返回执行任务实例。</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await stoppingToken.WaitDataMigrationCompletedAsync();
            Consoles.Info("正在开启网络监听，监听端口：{0}", _port);
            SetSocketOptions(_listener);
            _listener.Bind(new IPEndPoint(IPAddress.Any, _port));
            _listener.Listen(_backlog);
            _ = Task.Run(() => KeepLiveAsync(stoppingToken));
            Consoles.Info("正在等待网络连接...");
            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptAsync();
                Consoles.Warning("{0}加入网络连接，开始接收数据...", client.RemoteEndPoint);
                var session = new Session(client, _commands, _serviceProvider);
                Sessions.TryAdd(session.SessionId, session);
                _ = Task.Run(async () => await session.StartAsync(stoppingToken));
            }
        }

        private Task KeepLiveAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var expired = DateTimeOffset.Now.AddMinutes(-_keepLiveMinutes);
                var expireds = Sessions.Values
                    .Where(x => x.Actived < expired)
                    .Select(x => x.SessionId)
                    .ToList();
                foreach (var sessionId in expireds)
                {
                    if (Sessions.TryRemove(sessionId, out var session))
                        session.Close();
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 设置选项。
        /// </summary>
        /// <param name="socket">服务端Socket。</param>
        protected virtual void SetSocketOptions(Socket socket)
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        /// <summary>
        /// 停止后执行的方法。
        /// </summary>
        /// <param name="cancellationToken">停止标识。</param>
        /// <returns>返回执行的任务。</returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var session in Sessions.Values)
            { session.Close(); }
            _listener.Close();
            return base.StopAsync(cancellationToken);
        }
    }
}

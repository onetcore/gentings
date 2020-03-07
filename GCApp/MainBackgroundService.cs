using Gentings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data.Migrations;

namespace GCApp
{
    /// <summary>
    /// 主程序后台线程。
    /// </summary>
    public class MainBackgroundService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public MainBackgroundService(IConfiguration configuration, ILogger<MainBackgroundService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await stoppingToken.WaitDataMigrationCompletedAsync();
            stoppingToken.ThrowIfCancellationRequested();
            _logger.LogError("显示错误日志信息！");
        }
    }
}

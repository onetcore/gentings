using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Gentings.Data.Migrations
{
    /// <summary>
    /// 数据库迁移服务。
    /// </summary>
    internal class MigrationBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IDataMigrator _migrator;
        private readonly ILogger<MigrationBackgroundService> _logger;

        /// <summary>
        /// 初始化类<see cref="MigrationBackgroundService"/>。
        /// </summary>
        /// <param name="migrator">数据库迁移接口。</param>
        /// <param name="logger">日志接口。</param>
        public MigrationBackgroundService(IDataMigrator migrator, ILogger<MigrationBackgroundService> logger)
        {
            _migrator = migrator;
            _logger = logger;
        }

        /// <summary>
        /// 执行数据库迁移服务。
        /// </summary>
        /// <param name="stoppingToken">停止标记。</param>
        /// <returns>返回当前任务。</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("开始数据库迁移。");
            MigrationService.Status = MigrationStatus.Normal;
            try
            {
                if (!stoppingToken.IsCancellationRequested)
                    await _migrator.MigrateAsync();
                MigrationService.Status = MigrationStatus.Completed;
                _logger.LogInformation("数据库迁移完成。");
            }
            catch (Exception e)
            {
                MigrationService.Status = MigrationStatus.Error;
                MigrationService.Message = "数据库迁移错误（请查看日志文件）：" + e.Message;
                _logger.LogError("数据库迁移失败。");
            }
        }
    }
}
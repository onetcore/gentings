using System;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Properties;
using Microsoft.Extensions.Logging;

namespace Gentings.Data.Migrations
{
    /// <summary>
    /// 数据库迁移服务。
    /// </summary>
    public class MigrationBackgroundService : BackgroundService
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
            stoppingToken.ThrowIfCancellationRequested();
            _logger.LogInformation(Resources.DataMigration_Start);
            MigrationService.Status = MigrationStatus.Normal;
            try
            {
                    await _migrator.MigrateAsync();
                MigrationService.Status = MigrationStatus.Completed;
                _logger.LogInformation(Resources.DataMigration_Completed);
            }
            catch (Exception e)
            {
                MigrationService.Status = MigrationStatus.Error;
                MigrationService.Message = Resources.DataMigration_Error + e.Message;
                _logger.LogError(Resources.DataMigration_Failured);
            }
        }
    }
}
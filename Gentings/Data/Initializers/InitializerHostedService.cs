using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data.Migrations;
using Gentings.Properties;
using Gentings.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gentings.Data.Initializers
{
    /// <summary>
    /// 数据库迁移后台执行实现类。
    /// </summary>
    public class InitializerHostedService : BackgroundService
    {
        private static InitializerStatus _current;
        private static readonly object _locker = new object();
        private readonly IServiceProvider _serviceProvider;
        private readonly IInitializerManager _installerManager;
        private readonly ILogger _logger;

        /// <summary>
        /// 名称。
        /// </summary>
        public override string Name => Resources.InitializerHostedService;

        /// <summary>
        /// 描述。
        /// </summary>
        public override string Description => Resources.InitializerHostedService_Description;

        /// <summary>
        /// 当前安装状态。
        /// </summary>
        public static InitializerStatus Current
        {
            get
            {
                lock (_locker)
                {
                    return _current;
                }
            }
            set
            {
                lock (_locker)
                {
                    _current = value;
                }
            }
        }

        /// <summary>
        /// 初始化类<see cref="InitializerHostedService"/>。
        /// </summary>
        /// <param name="serviceProvider">服务提供者。</param>
        /// <param name="logger">日志接口。</param>
        public InitializerHostedService(IServiceProvider serviceProvider, ILogger<InitializerHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _installerManager = serviceProvider.GetService<IInitializerManager>();
            _logger = logger;
        }

        /// <summary>
        /// 执行的后台任务方法。
        /// </summary>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回任务实例。</returns>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            //不使用数据库模块时候用。
            if (_installerManager == null)
                return;

            //数据库迁移
            await ExecuteDataMigrationAsync(cancellationToken);

            //启动网站
            _logger.LogInformation(Resources.InitializerHostedService_Starting);
            var registration = await _installerManager.GetRegistrationAsync();
            if (registration.Expired < DateTimeOffset.Now)
            {
                //todo:远程连接获取验证信息
                registration.Status = InitializerStatus.Expired;
            }
            else
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var initializers = scope.ServiceProvider.GetService<IEnumerable<IInitializer>>();
                        if (initializers != null)
                        {
                            initializers = initializers.OrderByDescending(x => x.Priority);
                            foreach (var initializer in initializers)
                            {
                                if (!await initializer.IsDisabledAsync())
                                {
                                    await initializer.ExecuteAsync();
                                }
                            }
                        }
                    }

                    registration.Status = InitializerStatus.Success;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, Resources.InitializerHostedService_InitializedFailured);
                }
            }

            await _installerManager.SaveRegistrationAsync(registration);
            Current = registration.Status;
            _logger.LogInformation(Current == InitializerStatus.Failured ? Resources.InitializerHostedService_StartFailured : Resources.InitializerHostedService_Started);
            IsRunning = false;
        }

        /// <summary>
        /// 执行数据库迁移服务。
        /// </summary>
        /// <param name="cancellationToken">停止标记。</param>
        /// <returns>返回当前任务。</returns>
        protected async Task ExecuteDataMigrationAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation(Resources.DataMigration_Start);
            MigrationService.Status = MigrationStatus.Normal;
            try
            {
                await _serviceProvider.GetRequiredService<IDataMigrator>().MigrateAsync();
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
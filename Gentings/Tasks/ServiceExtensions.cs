using System.Collections.Generic;
using Gentings.Data.Migrations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gentings.Tasks
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        private class DefaultTaskDataMigration : TaskDataMigration
        {

        }

        private class DefaultTaskHostedService : TaskHostedService
        {
            /// <summary>
            /// 初始化<see cref="DefaultTaskHostedService"/>。
            /// </summary>
            /// <param name="services">后台服务列表。</param>
            /// <param name="taskManager">后台服务管理。</param>
            /// <param name="logger">日志接口。</param>
            public DefaultTaskHostedService(IEnumerable<ITaskService> services, ITaskManager taskManager, ILogger<TaskHostedService> logger)
                : base(services, taskManager, logger)
            {
            }
        }

        /// <summary>
        /// 添加后台任务组件。
        /// </summary>
        /// <param name="builder">服务构建实例对象。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddTaskServices(this IServiceBuilder builder)
        {
            return builder.AddTransients<IDataMigration, DefaultTaskDataMigration>()
                .AddSingleton<ITaskManager, TaskManager>()
                .AddSingletons<IHostedService, DefaultTaskHostedService>();
        }
    }
}
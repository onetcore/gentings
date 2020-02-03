using Gentings.Data.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Gentings.Tasks
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加后台服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddTaskServices(this IServiceBuilder builder) =>
            builder.AddServices(services =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Transient<IDataMigration, TaskDataMigration>());
                services.TryAddEnumerable(ServiceDescriptor.Transient<IHostedService, TaskHostedService>());
            });
    }
}
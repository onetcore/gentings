using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Gentings.Data.Migrations
{
    /// <summary>
    /// 数据库扩展。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加数据库迁移服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddDataMigration(this IServiceBuilder builder)
        {
            return builder.AddServices(services =>
                services.TryAddEnumerable(ServiceDescriptor.Transient<IHostedService, MigrationBackgroundService>()));
        }

        /// <summary>
        /// 等待数据库迁移正确完成，如果不正确将一直等待下去。
        /// </summary>
        /// <param name="cancellationToken">取消标志。</param>
        public static async Task WaitDataMigrationCompletedAsync(this CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (MigrationService.Status != MigrationStatus.Completed)
                    break;
                await Task.Delay(100, cancellationToken);
            }
        }
    }
}
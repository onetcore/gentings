using Gentings.Data.Migrations;
using Gentings.Storages.Media;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.Storages
{
    /// <summary>
    /// 服务注册扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加文件存储服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddMediaStorages(this IServiceBuilder builder)
        {
            return builder.AddServices(services =>
            {
                services.AddSingleton<IMediaDirectory, DefaultMediaDirectory>();
                services.AddTransient<IDataMigration, DefaultMediaDataMigration>();
            });
        }
    }
}
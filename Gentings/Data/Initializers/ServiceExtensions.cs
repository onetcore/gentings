using Gentings.Data.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Gentings.Data.Initializers
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加初始化服务。
        /// </summary>
        /// <param name="services">服务集合实例。</param>
        /// <returns>返回服务集合实例。</returns>
        public static IServiceCollection AddInitializerService(this IServiceCollection services)
        {
            services.AddSingleton<IInitializerManager, DefaultInitializerManager>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IDataMigration, DefaultInitializerDataMigration>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), typeof(DefaultInitializerHostedService)));
            return services;
        }
    }
}
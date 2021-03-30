using Gentings.Data.Migrations;
using Gentings.Extensions;

namespace Gentings.OpenServices
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加开放平台组件。
        /// </summary>
        /// <typeparam name="TUser">用户类型。</typeparam>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddOpenServices<TUser>(this IServiceBuilder builder)
            where TUser : IUser
        {
            return builder.AddTransients<IDataMigration, ApplicationDataMigration>()
                .AddSingleton<IApplicationManager, ApplicationManager<TUser>>()
                .AddSingleton<IOpenServiceManager, OpenServiceManager>();
        }

    }
}
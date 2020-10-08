using Gentings.Data.Migrations;
using Gentings.Identity.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gentings.Identity.Permissions
{
    /// <summary>
    /// 扩展服务类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加权限模块。
        /// </summary>
        /// <typeparam name="TRole">角色类型。</typeparam>
        /// <typeparam name="TUserRole">用户角色类型。</typeparam>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddPermissions<TRole, TUserRole>(this IServiceBuilder builder)
            where TUserRole : IUserRole
            where TRole : RoleBase
        {
            return builder.AddServices(services =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Transient<IDataMigration, DefaultPermissionDataMigration<TRole>>());
                services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
                services.AddScoped<IPermissionManager, DefaultPermissionManager<TRole, TUserRole>>();
            });
        }
    }
}
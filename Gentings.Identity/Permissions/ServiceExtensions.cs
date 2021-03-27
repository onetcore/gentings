using System;
using Gentings.Data;
using Gentings.Data.Initializers;
using Gentings.Data.Migrations;
using Gentings.Identity.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Identity.Permissions
{
    /// <summary>
    /// 扩展服务类。
    /// </summary>
    public static class ServiceExtensions
    {
        private class DefaultPermissionManager<TRole, TUserRole> : PermissionManager<TRole, TUserRole>
            where TRole : RoleBase
            where TUserRole : IUserRole
        {
            /// <summary>
            /// 初始化类<see cref="DefaultPermissionManager{TUserRole, TRole}"/>。
            /// </summary>
            /// <param name="db">数据库操作接口实例。</param>
            /// <param name="prdb">数据库操作接口。</param>
            /// <param name="serviceProvider">服务提供者接口。</param>
            /// <param name="cache">缓存接口。</param>
            /// <param name="rdb">角色数据库操作接口。</param>
            /// <param name="urdb">用户角色数据库操作接口。</param>
            public DefaultPermissionManager(IDbContext<Permission> db, IDbContext<PermissionInRole> prdb, IServiceProvider serviceProvider, IMemoryCache cache, IDbContext<TRole> rdb, IDbContext<TUserRole> urdb)
                : base(db, prdb, serviceProvider, cache, rdb, urdb)
            {
            }
        }

        private class DefaultPermissionInitializer : PermissionInitializer
        {
            /// <summary>
            /// 初始化类<see cref="DefaultPermissionInitializer"/>。
            /// </summary>
            /// <param name="permissionManager">权限管理类。</param>
            public DefaultPermissionInitializer(IPermissionManager permissionManager) : base(permissionManager)
            {
            }
        }

        private class DefaultPermissionDataMigration<TRole> : PermissionDataMigration<TRole> where TRole : RoleBase
        {
        }

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
            return builder.AddTransients<IDataMigration, DefaultPermissionDataMigration<TRole>>()
                 .AddTransients<IInitializer, DefaultPermissionInitializer>()
                 .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                 .AddScoped<IPermissionManager, DefaultPermissionManager<TRole, TUserRole>>();
        }
    }
}
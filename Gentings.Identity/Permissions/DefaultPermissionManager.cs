using System;
using Gentings.Data;
using Gentings.Identity.Roles;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Identity.Permissions
{
    internal class DefaultPermissionManager<TRole, TUserRole> : PermissionManager<TRole, TUserRole> 
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
}
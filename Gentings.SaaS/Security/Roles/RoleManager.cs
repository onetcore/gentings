using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gentings.Extensions;
using Gentings.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Gentings.SaaS.Security.Roles
{
    /// <summary>
    /// 角色管理实现类。
    /// </summary>
    /// <typeparam name="TRole">角色类型。</typeparam>
    /// <typeparam name="TUserRole">用户角色类型。</typeparam>
    /// <typeparam name="TRoleClaim">角色声明类型。</typeparam>
    public abstract class RoleManager<TRole, TUserRole, TRoleClaim> : Identity.Roles.RoleManager<TRole, TUserRole, TRoleClaim>
        where TRole : RoleBase
        where TUserRole : IUserRole
        where TRoleClaim : RoleClaimBase, new()
    {
        private readonly SiteDomain _domain;

        /// <summary>
        /// 初始化类<see cref="RoleManager{TRole,TUserRole,TRoleClaim}"/>
        /// </summary>
        /// <param name="store">存储接口。</param>
        /// <param name="roleValidators">角色验证集合。</param>
        /// <param name="keyNormalizer">角色唯一键格式化接口。</param>
        /// <param name="errors">错误实例。</param>
        /// <param name="logger">日志接口。</param>
        /// <param name="cache">缓存接口。</param>
        /// <param name="domain">当前网站域名实例。</param>
        protected RoleManager(IRoleStore<TRole> store, IEnumerable<IRoleValidator<TRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<TRole>> logger, IMemoryCache cache, SiteDomain domain)
            : base(store, roleValidators, keyNormalizer, errors, logger, cache)
        {
            _domain = domain;
        }

        /// <summary>
        /// 通过ID获取角色实例。
        /// </summary>
        /// <param name="id">角色Id。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public override TRole FindById(int id)
        {
            return DbContext.RoleContext.Find(x => x.Id == id && x.SiteId == _domain.SiteId);
        }

        /// <summary>
        /// 通过ID获取角色实例。
        /// </summary>
        /// <param name="id">角色Id。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public override Task<TRole> FindByIdAsync(int id)
        {
            return DbContext.RoleContext.FindAsync(x => x.Id == id && x.SiteId == _domain.SiteId);
        }

        /// <summary>
        /// 通过ID获取角色实例。
        /// </summary>
        /// <param name="roleId">角色Id。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public override async Task<TRole> FindByIdAsync(string roleId)
        {
            if (int.TryParse(roleId, out var id))
                return await DbContext.RoleContext.FindAsync(x => x.Id == id && x.SiteId == _domain.SiteId);
            return null;
        }

        /// <summary>
        /// 通过角色名称获取角色实例。
        /// </summary>
        /// <param name="roleName">角色名称。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public override TRole FindByName(string roleName)
        {
            roleName = NormalizeKey(roleName);
            return DbContext.RoleContext.Find(x => x.NormalizedName == roleName && x.SiteId == _domain.SiteId);
        }

        /// <summary>
        /// 通过角色名称获取角色实例。
        /// </summary>
        /// <param name="roleName">角色名称。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public override Task<TRole> FindByNameAsync(string roleName)
        {
            roleName = NormalizeKey(roleName);
            return DbContext.RoleContext.FindAsync(x => x.NormalizedName == roleName && x.SiteId == _domain.SiteId);
        }

        /// <summary>
        /// 角色实例列表。
        /// </summary>
        public override IEnumerable<TRole> Load()
        {
            return Cache.GetOrCreate(CacheKey, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var roles = DbContext.RoleContext.Fetch(x => x.SiteId == _domain.SiteId);
                return roles.OrderByDescending(x => x.RoleLevel).ToList();
            });
        }

        /// <summary>
        /// 角色实例列表。
        /// </summary>
        /// <returns>返回角色列表。</returns>
        public override async Task<IEnumerable<TRole>> LoadAsync()
        {
            return await Cache.GetOrCreateAsync(CacheKey, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var roles = await DbContext.RoleContext.FetchAsync(x => x.SiteId == _domain.SiteId);
                return roles.OrderByDescending(x => x.RoleLevel).ToList();
            });
        }

        /// <summary>
        /// 删除角色。
        /// </summary>
        /// <param name="ids">角色Id。</param>
        /// <returns>返回删除结果。</returns>
        public override IdentityResult Delete(int[] ids)
        {
            var result = IdentityResult.Success;
            if (!DbContext.RoleContext.Delete(x => x.SiteId == _domain.SiteId && x.Id.Included(ids)))
            {
                result = IdentityResult.Failed(ErrorDescriber.DefaultError());
            }

            return FromResult(result, null);
        }

        /// <summary>
        /// 删除角色。
        /// </summary>
        /// <param name="ids">角色Id。</param>
        /// <returns>返回删除结果。</returns>
        public override async Task<IdentityResult> DeleteAsync(int[] ids)
        {
            var result = IdentityResult.Success;
            if (!await DbContext.RoleContext.DeleteAsync(x => x.SiteId == _domain.SiteId && x.Id.Included(ids)))
            {
                result = IdentityResult.Failed(ErrorDescriber.DefaultError());
            }

            return FromResult(result, null);
        }

        /// <summary>
        /// 缓存键。
        /// </summary>
        protected string CacheKey => $"sites{_domain.SiteId}:roles";

        /// <summary>
        /// 移动角色分组条件表达式。
        /// </summary>
        /// <param name="role">当前角色。</param>
        /// <returns>返回条件表达式。</returns>
        protected override Expression<Predicate<TRole>> MoveExpression(TRole role)
        {
            return x => x.SiteId == _domain.SiteId && x.RoleLevel > 0 && x.RoleLevel < int.MaxValue;
        }

        /// <summary>
        /// 如果成功移除缓存。
        /// </summary>
        /// <param name="result">返回结果。</param>
        /// <param name="role">当前角色实例。</param>
        /// <returns>返回结果。</returns>
        protected override bool FromResult(bool result, TRole role)
        {
            if (result)
            {
                Cache.Remove(CacheKey);
            }

            return result;
        }
    }
}
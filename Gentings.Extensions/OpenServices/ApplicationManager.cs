using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Security;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 应用管理实现类。
    /// </summary>
    public class ApplicationManager<TUser> : ObjectManager<Application, Guid>, IApplicationManager where TUser : class, IUser
    {
        private readonly IDbContext<ApplicationService> _asdb;
        private readonly IMemoryCache _cache;
        private readonly IDbContext<TUser> _userdb;

        /// <summary>
        /// 初始化类<see cref="ApplicationManager{TUser}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="asdb">应用程序服务数据库接口。</param>
        /// <param name="cache">缓存接口。</param>
        /// <param name="userdb">用户数据库操作接口实例。</param>
        public ApplicationManager(IDbContext<Application> context, IDbContext<ApplicationService> asdb, IMemoryCache cache, IDbContext<TUser> userdb)
            : base(context)
        {
            _asdb = asdb;
            _cache = cache;
            _userdb = userdb;
        }

        /// <summary>
        /// 获取用户应用，包含用户实例。
        /// </summary>
        /// <param name="appId">应用Id。</param>
        /// <returns>返回包含用户实例的应用类型实例。</returns>
        public virtual Task<Application> FindUserApplicationAsync(Guid appId)
        {
            return Context.JoinSelect<Application, TUser>((a, u) => a.UserId == u.Id, x => x.UserName)
                .Where(x => x.Id == appId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取缓存键。
        /// </summary>
        /// <param name="appid">AppId。</param>
        /// <returns>返回缓存键字符串。</returns>
        protected string GetCacheKey(Guid appid) => $"appservices:{appid.ToString().ToLower()}";

        /// <summary>
        /// 获取应用程序所包含的服务Id列表。
        /// </summary>
        /// <param name="appid">应用程序Id。</param>
        /// <returns>返回服务Id列表。</returns>
        public Task<List<int>> LoadApplicationServicesAsync(Guid appid)
        {
            return _cache.GetOrCreateAsync(GetCacheKey(appid), async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var services = await _asdb.AsQueryable().WithNolock()
                    .Where(x => x.AppId == appid)
                    .Select(x => x.ServiceId)
                    .AsEnumerableAsync(reader => reader.GetInt32(0));
                return services.ToList();
            });
        }

        /// <summary>
        /// 将服务添加到应用程序中。
        /// </summary>
        /// <param name="appid">应用程序Id。</param>
        /// <param name="ids">服务Id列表。</param>
        /// <returns>返回添加结果。</returns>
        public async Task<bool> AddApplicationServicesAsync(Guid appid, int[] ids)
        {
            if (await _asdb.BeginTransactionAsync(async db =>
            {
                await db.DeleteAsync(x => x.AppId == appid);
                foreach (var id in ids)
                {
                    await db.CreateAsync(new ApplicationService { AppId = appid, ServiceId = id });
                }

                return true;
            }))
            {
                _cache.Remove(GetCacheKey(appid));
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取用户列表。
        /// </summary>
        /// <returns>返回用户列表实例。</returns>
        public async Task<IDictionary<int, string>> LoadUsersAsync()
        {
            var users = await _userdb.AsQueryable().WithNolock()
                .Select(x => new { x.Id, x.NickName })
                .OrderBy(x => x.NickName)
                .AsEnumerableAsync(x => KeyValuePair.Create(x.GetInt32(0), x.GetString(1)));
            return users.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
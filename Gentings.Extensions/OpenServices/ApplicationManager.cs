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
    /// <typeparam name="TUser">用户类型。</typeparam>
    public class ApplicationManager<TUser> : IApplicationManager where TUser : IUser
    {
        private readonly IDbContext<Application> _context;
        private readonly IDbContext<ApplicationService> _asdb;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// 初始化类<see cref="ApplicationManager{TUser}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="asdb">应用程序服务数据库接口。</param>
        /// <param name="cache">缓存接口。</param>
        public ApplicationManager(IDbContext<Application> context, IDbContext<ApplicationService> asdb, IMemoryCache cache)
        {
            _context = context;
            _asdb = asdb;
            _cache = cache;
        }

        /// <summary>
        /// 通过应用Id获取应用程序实例。
        /// </summary>
        /// <param name="appId">应用Id。</param>
        /// <returns>返回当前应用实例。</returns>
        public virtual Application Find(Guid appId)
        {
            return _context.Find(appId);
        }

        /// <summary>
        /// 通过应用Id获取应用程序实例。
        /// </summary>
        /// <param name="appId">应用Id。</param>
        /// <returns>返回当前应用实例。</returns>
        public virtual Task<Application> FindAsync(Guid appId)
        {
            return _context.FindAsync(appId);
        }

        /// <summary>
        /// 获取用户应用，包含用户实例。
        /// </summary>
        /// <param name="appId">应用Id。</param>
        /// <returns>返回包含用户实例的应用类型实例。</returns>
        public virtual Task<Application> FindUserApplicationAsync(Guid appId)
        {
            return _context.AsQueryable().WithNolock()
                .InnerJoin<TUser>((a, u) => a.UserId == u.Id)
                .Select()//备注，这个需要放在用户之前，否正可能会被覆盖
                .Select<TUser>(x => x.UserName)
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
        /// 分页获取应用。
        /// </summary>
        /// <param name="query">查询实例。</param>
        /// <returns>应用列表。</returns>
        public virtual IPageEnumerable<Application> Load(QueryBase<Application> query)
        {
            return _context.Load(query);
        }

        /// <summary>
        /// 分页获取应用。
        /// </summary>
        /// <param name="query">查询实例。</param>
        /// <returns>应用列表。</returns>
        public virtual Task<IPageEnumerable<Application>> LoadAsync(QueryBase<Application> query)
        {
            return _context.LoadAsync(query);
        }

        /// <summary>
        /// 添加应用。
        /// </summary>
        /// <param name="application">应用实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual DataResult Save(Application application)
        {
            if (_context.Any(x => x.Id == application.Id))
                return DataResult.FromResult(_context.Update(application), DataAction.Updated);
            return DataResult.FromResult(_context.Create(application), DataAction.Created);
        }

        /// <summary>
        /// 添加应用。
        /// </summary>
        /// <param name="application">应用实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual async Task<DataResult> SaveAsync(Application application)
        {
            if (await _context.AnyAsync(x => x.Id == application.Id))
                return DataResult.FromResult(await _context.UpdateAsync(application), DataAction.Updated);
            return DataResult.FromResult(await _context.CreateAsync(application), DataAction.Created);
        }

        /// <summary>
        /// 删除应用。
        /// </summary>
        /// <param name="ids">应用ID列表。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(Guid[] ids)
        {
            return DataResult.FromResult(_context.Delete(x => x.Id.Included(ids)), DataAction.Deleted);
        }

        /// <summary>
        /// 删除应用。
        /// </summary>
        /// <param name="ids">应用ID列表。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(Guid[] ids)
        {
            return DataResult.FromResult(await _context.DeleteAsync(x => x.Id.Included(ids)), DataAction.Deleted);
        }
    }
}
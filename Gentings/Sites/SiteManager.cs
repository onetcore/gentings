using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Sites
{
    /// <summary>
    /// 网站管理基类。
    /// </summary>
    /// <typeparam name="TSite">网站实例。</typeparam>
    public class SiteManager<TSite> : ISiteManager<TSite> where TSite : Site, new()
    {
        private readonly IEnumerable<ISiteEventHandler<TSite>> _eventHandlers;

        /// <summary>
        /// 数据库操作接口。
        /// </summary>
        protected IDbContext<SiteAdapter> Context { get; }

        /// <summary>
        /// 缓存实例。
        /// </summary>
        protected IMemoryCache Cache { get; }

        /// <summary>
        /// 初始化类<see cref="SiteManager{TSite}"/>。
        /// </summary>
        /// <param name="context">数据库操作接口。</param>
        /// <param name="cache">缓存实例。</param>
        /// <param name="eventHandlers">网站事件处理器。</param>
        public SiteManager(IDbContext<SiteAdapter> context, IMemoryCache cache, IEnumerable<ISiteEventHandler<TSite>> eventHandlers)
        {
            _eventHandlers = eventHandlers.OrderByDescending(x => x.Priority);
            Context = context;
            Cache = cache;
        }

        /// <summary>
        /// 如果结果正确返回<paramref name="succeed"/>，否则返回失败项。
        /// </summary>
        /// <param name="siteId">网站id。</param>
        /// <param name="result">执行结果。</param>
        /// <param name="succeed">执行成功返回的值。</param>
        /// <returns>返回执行结果实例对象。</returns>
        protected DataResult FromResult(int siteId, bool result, DataAction succeed)
        {
            if (result)
            {
                Cache.Remove(GetCacheKey(siteId));
                return succeed;
            }
            return (DataAction)(-(int)succeed);
        }

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="result">执行结果。</param>
        /// <param name="ids">网站ID列表。</param>
        /// <returns>返回执行结果。</returns>
        protected bool Refresh(bool result, int[] ids)
        {
            if (result)
            {
                foreach (var id in ids)
                {
                    Cache.Remove(GetCacheKey(id));
                }
            }

            return result;
        }

        /// <summary>
        /// 获取缓存键。
        /// </summary>
        /// <param name="siteId">网站id。</param>
        /// <returns>返回缓存键实例。</returns>
        protected string GetCacheKey(int siteId) => $"Site{siteId}";

        /// <summary>
        /// 获取网站实例。
        /// </summary>
        /// <param name="id">网站Id。</param>
        /// <returns>返回网站实例。</returns>
        public virtual TSite Find(int id)
        {
            return Cache.GetOrCreate(GetCacheKey(id), ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                return Context.Find(id)?.AsSite<TSite>();
            });
        }

        /// <summary>
        /// 获取网站实例。
        /// </summary>
        /// <param name="id">网站Id。</param>
        /// <returns>返回网站实例。</returns>
        public virtual Task<TSite> FindAsync(int id)
        {
            return Cache.GetOrCreateAsync(GetCacheKey(id), async ctx =>
             {
                 ctx.SetDefaultAbsoluteExpiration();
                 var site = await Context.FindAsync(id);
                 return site?.AsSite<TSite>();
             });
        }

        /// <summary>
        /// 保存当前实例。
        /// </summary>
        /// <param name="site">网站实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public virtual DataResult Save(TSite site)
        {
            var adapter = SiteAdapter.FromSite(site);
            if (Context.Any(x => x.SiteKey == adapter.SiteKey && x.Id != adapter.Id))
                return DataAction.Duplicate;
            if (adapter.Id > 0)
                return FromResult(adapter.Id, Context.Update(adapter), DataAction.Updated);
            var result = Context.BeginTransaction(db =>
            {
                if (!db.Create(adapter))
                    return false;
                site.Id = adapter.Id;
                foreach (var eventHandler in _eventHandlers)
                {
                    if (!eventHandler.OnCreated(db, site))
                    {
                        site.Id = 0;
                        return false;
                    }
                }
                return true;
            }, 600);
            return FromResult(adapter.Id, result, DataAction.Created);
        }

        /// <summary>
        /// 保存当前实例。
        /// </summary>
        /// <param name="site">网站实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public virtual async Task<DataResult> SaveAsync(TSite site)
        {
            var adapter = SiteAdapter.FromSite(site);
            if (Context.Any(x => x.SiteKey == adapter.SiteKey && x.Id != adapter.Id))
                return DataAction.Duplicate;
            if (adapter.Id > 0)
                return FromResult(adapter.Id, await Context.UpdateAsync(adapter), DataAction.Updated);
            var result = await Context.BeginTransactionAsync(async db =>
            {
                if (!await db.CreateAsync(adapter))
                    return false;
                site.Id = adapter.Id;
                foreach (var eventHandler in _eventHandlers)
                {
                    if (!await eventHandler.OnCreatedAsync(db, site))
                    {
                        site.Id = 0;
                        return false;
                    }
                }
                return true;
            }, 600);
            return FromResult(adapter.Id, result, DataAction.Created);
        }

        /// <summary>
        /// 删除当前实例。
        /// </summary>
        /// <param name="ids">网站Id列表。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(int[] ids)
        {
            var result = Refresh(Context.BeginTransaction(db =>
            {
                foreach (var eventHandler in _eventHandlers)
                {
                    if (!eventHandler.OnDelete(db, ids))
                        return false;
                }

                return db.Delete(x => x.Id.Included(ids));
            }, 600), ids);
            return DataResult.FromResult(result, DataAction.Created);
        }

        /// <summary>
        /// 删除当前实例。
        /// </summary>
        /// <param name="ids">网站Id列表。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(int[] ids)
        {
            var result = Refresh(await Context.BeginTransactionAsync(async db =>
            {
                foreach (var eventHandler in _eventHandlers)
                {
                    if (!await eventHandler.OnDeleteAsync(db, ids))
                        return false;
                }

                return await db.DeleteAsync(x => x.Id.Included(ids));
            }, 600), ids);
            return DataResult.FromResult(result, DataAction.Created);
        }

        /// <summary>
        /// 分页查询网站实例。
        /// </summary>
        /// <param name="query">网站查询实例。</param>
        /// <returns>返回当前网站列表。</returns>
        public virtual IPageEnumerable<Site> Load(SiteQuery query)
        {
            return Context.Load<SiteQuery, Site>(query);
        }

        /// <summary>
        /// 分页查询网站实例。
        /// </summary>
        /// <param name="query">网站查询实例。</param>
        /// <returns>返回当前网站列表。</returns>
        public virtual Task<IPageEnumerable<Site>> LoadAsync(SiteQuery query)
        {
            return Context.LoadAsync<SiteQuery, Site>(query);
        }

        /// <summary>
        /// 启用网站。
        /// </summary>
        /// <param name="ids">启用Id列表。</param>
        /// <returns>返回启用结果。</returns>
        public virtual bool Enabled(int[] ids)
        {
            return Refresh(Context.Update(x => x.Id.Included(ids), new { Disabled = false }), ids);
        }

        /// <summary>
        /// 启用网站。
        /// </summary>
        /// <param name="ids">启用Id列表。</param>
        /// <returns>返回启用结果。</returns>
        public virtual async Task<bool> EnabledAsync(int[] ids)
        {
            return Refresh(await Context.UpdateAsync(x => x.Id.Included(ids), new { Disabled = false }), ids);
        }

        /// <summary>
        /// 禁用网站。
        /// </summary>
        /// <param name="ids">禁用Id列表。</param>
        /// <returns>返回禁用结果。</returns>
        public virtual bool Disabled(int[] ids)
        {
            return Refresh(Context.Update(x => x.Id.Included(ids), new { Disabled = true }), ids);
        }

        /// <summary>
        /// 禁用网站。
        /// </summary>
        /// <param name="ids">禁用Id列表。</param>
        /// <returns>返回禁用结果。</returns>
        public virtual async Task<bool> DisabledAsync(int[] ids)
        {
            return Refresh(await Context.UpdateAsync(x => x.Id.Included(ids), new { Disabled = true }), ids);
        }
    }
}
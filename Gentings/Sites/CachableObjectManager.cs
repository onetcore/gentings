using Microsoft.Extensions.Caching.Memory;
using Gentings.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Extensions;
using Gentings.Sites.Internal;

namespace Gentings.Sites
{
    /// <summary>
    /// 缓存对象管理基类。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    /// <typeparam name="TKey">模型主键类型。</typeparam>
    public abstract class CachableObjectManager<TModel, TKey> : ObjectManagerBase<TModel, TKey>,
        ICachableObjectManager<TModel, TKey>
        where TModel : ISiteIdObject<TKey>
    {
        /// <summary>
        /// 缓存实例。
        /// </summary>
        protected IMemoryCache Cache { get; }

        /// <summary>
        /// 初始化类<see cref="CachableObjectManager{TModel,TKey}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        protected CachableObjectManager(IDbContext<TModel> context, IMemoryCache cache) : base(context)
        {
            Cache = cache;
        }

        /// <summary>
        /// 缓存键。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        protected virtual object GetCacheKey(int siteId) => new SiteCacheKey(typeof(TModel), siteId);

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        public virtual void Refresh(int siteId)
        {
            Cache.Remove(GetCacheKey(siteId));
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public override void Clear()
        {
            var siteIds = Context.AsQueryable()
                .WithNolock()
                .Distinct(x => x.SiteId)
                .AsEnumerable(x => x.GetInt32(0));
            if (siteIds.Any())
            {
                Context.Delete();
                foreach (var siteId in siteIds)
                {
                    Refresh(siteId);
                }
            }
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public override async Task ClearAsync(CancellationToken cancellationToken = default)
        {
            var siteIds = await Context.AsQueryable()
                .WithNolock()
                .Distinct(x => x.SiteId)
                .AsEnumerableAsync(x => x.GetInt32(0), cancellationToken);
            if (siteIds.Any())
            {
                await Context.DeleteAsync(cancellationToken: cancellationToken);
                foreach (var siteId in siteIds)
                {
                    Refresh(siteId);
                }
            }
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        public override void Clear(int siteId)
        {
            base.Clear(siteId);
            Refresh(siteId);
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        public override async Task ClearAsync(int siteId, CancellationToken cancellationToken = default)
        {
            await base.ClearAsync(siteId, cancellationToken);
            Refresh(siteId);
        }

        /// <summary>
        /// 如果操作结果成功刷新缓存并返回结果。
        /// </summary>
        /// <param name="result">数据库操作结果。</param>
        /// <param name="siteId">网站Id。</param>
        /// <returns>数据库操作结果。</returns>
        protected DataResult Refresh(DataResult result, int siteId)
        {
            if (result)
            {
                Refresh(siteId);
            }

            return result;
        }

        /// <summary>
        /// 如果操作结果成功刷新缓存并返回结果。
        /// </summary>
        /// <param name="result">数据库操作结果。</param>
        /// <param name="siteId">网站Id。</param>
        /// <returns>数据库操作结果。</returns>
        protected bool Refresh(bool result, int siteId)
        {
            if (result)
            {
                Refresh(siteId);
            }

            return result;
        }

        /// <summary>
        /// 如果结果正确返回<paramref name="succeed"/>，否则返回失败项。
        /// </summary>
        /// <param name="result">执行结果。</param>
        /// <param name="succeed">执行成功返回的值。</param>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回执行结果实例对象。</returns>
        protected DataResult Refresh(bool result, DataAction succeed, int siteId)
        {
            if (result)
            {
                Refresh(siteId);
                return succeed;
            }

            return -(int)succeed;
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="ids">唯一Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public override DataResult Delete(int siteId, IEnumerable<TKey> ids)
        {
            return Refresh(base.Delete(siteId, ids), siteId);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="ids">唯一Id集合。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public override async Task<DataResult> DeleteAsync(int siteId, IEnumerable<TKey> ids,
            CancellationToken cancellationToken = default)
        {
            return Refresh(await base.DeleteAsync(siteId, ids, cancellationToken), siteId);
        }

        /// <summary>
        /// 根据条件删除实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回删除结果。</returns>
        public override DataResult Delete(int siteId, Expression<Predicate<TModel>> expression)
        {
            return Refresh(base.Delete(siteId, expression), siteId);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <returns>返回删除结果。</returns>
        public override DataResult Delete(int siteId, TKey id)
        {
            return Refresh(base.Delete(siteId, id), siteId);
        }

        /// <summary>
        /// 根据条件删除实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public override async Task<DataResult> DeleteAsync(int siteId, Expression<Predicate<TModel>> expression,
            CancellationToken cancellationToken = default)
        {
            return Refresh(await base.DeleteAsync(siteId, expression, cancellationToken), siteId);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public override async Task<DataResult> DeleteAsync(int siteId, TKey id, CancellationToken cancellationToken = default)
        {
            return Refresh(await base.DeleteAsync(siteId, id, cancellationToken), siteId);
        }

        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public override DataResult Save(TModel model)
        {
            return Refresh(base.Save(model), model.SiteId);
        }

        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        public override async Task<DataResult> SaveAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Refresh(await base.SaveAsync(model, cancellationToken), model.SiteId);
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="fields">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public override DataResult Update(int siteId, Expression<Predicate<TModel>> expression, object fields)
        {
            return Refresh(base.Update(siteId, expression, fields), siteId);
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="fields">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public override async Task<DataResult> UpdateAsync(int siteId, Expression<Predicate<TModel>> expression, object fields,
            CancellationToken cancellationToken = default)
        {
            return Refresh(await base.UpdateAsync(siteId, expression, fields, cancellationToken), siteId);
        }

        /// <summary>
        /// 获取分类。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">分类Id。</param>
        /// <returns>返回分类实例。</returns>
        public override TModel Find(int siteId, TKey id)
        {
            var models = LoadCached(siteId);
            models.TryGetValue(id, out var model);
            return model;
        }

        /// <summary>
        /// 获取分类。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">分类Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回分类实例。</returns>
        public override async Task<TModel> FindAsync(int siteId, TKey id, CancellationToken cancellationToken = default)
        {
            var models = await LoadCachedAsync(siteId, cancellationToken);
            models.TryGetValue(id, out var model);
            return model;
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回当前模型实例。</returns>
        public override TModel Find(int siteId, Expression<Predicate<TModel>> expression)
        {
            return Fetch(siteId, expression).SingleOrDefault();
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前模型实例。</returns>
        public override async Task<TModel> FindAsync(int siteId, Expression<Predicate<TModel>> expression,
            CancellationToken cancellationToken = default)
        {
            var models = await FetchAsync(siteId, expression, cancellationToken);
            return models.SingleOrDefault();
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <param name="fields">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public override DataResult Update(int siteId, TKey id, object fields)
        {
            return Refresh(base.Update(siteId, id, fields), siteId);
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <param name="fields">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public override async Task<DataResult> UpdateAsync(int siteId, TKey id, object fields,
            CancellationToken cancellationToken = default)
        {
            return Refresh(await base.UpdateAsync(siteId, id, fields, cancellationToken), siteId);
        }

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例列表。</returns>
        public override IEnumerable<TModel> Fetch(int siteId, Expression<Predicate<TModel>> expression = null)
        {
            var models = LoadCached(siteId);
            return models.Values.Filter(expression);
        }

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回模型实例列表。</returns>
        public override async Task<IEnumerable<TModel>> FetchAsync(int siteId, Expression<Predicate<TModel>> expression = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var models = await LoadCachedAsync(siteId, cancellationToken);
            return models.Values.Filter(expression);
        }

        /// <summary>
        /// 缓存当前网站所有实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回当前网站所有实例。</returns>
        protected virtual ConcurrentDictionary<TKey, TModel> LoadCached(int siteId)
        {
            return Cache.GetOrCreate(GetCacheKey(siteId), ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var models = Context.Fetch(x => x.SiteId == siteId);
                return new ConcurrentDictionary<TKey, TModel>(models.ToDictionary(x => x.Id));
            });
        }

        /// <summary>
        /// 缓存当前网站所有实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前网站所有实例。</returns>
        protected virtual Task<ConcurrentDictionary<TKey, TModel>> LoadCachedAsync(int siteId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Cache.GetOrCreateAsync(GetCacheKey(siteId), async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var models = await Context.FetchAsync(x => x.SiteId == siteId, cancellationToken);
                return new ConcurrentDictionary<TKey, TModel>(models.ToDictionary(x => x.Id));
            });
        }

        /// <summary>
        /// 更新特定的实例。
        /// </summary>
        /// <param name="model">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public override bool Update(TModel model)
        {
            return Refresh(base.Update(model), model.SiteId);
        }

        /// <summary>
        /// 更新特定的实例。
        /// </summary>
        /// <param name="model">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public override async Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Refresh(await Context.UpdateAsync(model, cancellationToken), model.SiteId);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <returns>返回添加结果。</returns>
        public override bool Create(TModel model)
        {
            return Refresh(Context.Create(model), model.SiteId);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回添加结果。</returns>
        public override async Task<bool> CreateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Refresh(await Context.CreateAsync(model, cancellationToken), model.SiteId);
        }
    }

    /// <summary>
    /// 缓存对象管理基类。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public abstract class CachableObjectManager<TModel> : CachableObjectManager<TModel, int>,
        ICachableObjectManager<TModel>
        where TModel : ISiteIdObject
    {
        /// <summary>
        /// 初始化类<see cref="CachableObjectManager{TModel}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        protected CachableObjectManager(IDbContext<TModel> context, IMemoryCache cache) : base(context, cache)
        {
        }
    }
}
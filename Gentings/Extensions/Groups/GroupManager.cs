using Microsoft.Extensions.Caching.Memory;
using Gentings.Data;
using System.Collections.Concurrent;

namespace Gentings.Extensions.Groups
{
    /// <summary>
    /// 初始化类<see cref="GroupManager{TGroup}"/>。
    /// </summary>
    /// <typeparam name="TGroup">分组类型。</typeparam>
    public abstract class GroupManager<TGroup> : CachableObjectManager<TGroup>, IGroupManager<TGroup>
        where TGroup : GroupBase<TGroup>
    {
        /// <summary>
        /// 初始化类<see cref="GroupManager{TCategory}"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="cache">缓存接口。</param>
        protected GroupManager(IDbContext<TGroup> context, IMemoryCache cache)
            : base(context, cache)
        {
        }

        /// <summary>
        /// 判断是否已经存在。
        /// </summary>
        /// <param name="category">分类实例。</param>
        /// <returns>返回判断结果。</returns>
        public override bool IsDuplicated(TGroup category)
        {
            return Context.Any(x => x.ParentId == category.ParentId && x.Id != category.Id && x.Name == category.Name);
        }

        /// <summary>
        /// 判断是否已经存在。
        /// </summary>
        /// <param name="category">分类实例。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回判断结果。</returns>
        public override Task<bool> IsDuplicatedAsync(TGroup category,
            CancellationToken cancellationToken = default)
        {
            return Context.AnyAsync(x => x.ParentId == category.ParentId && x.Id != category.Id && x.Name == category.Name,
                cancellationToken);
        }

        /// <summary>
        /// 缓存当前网站所有实例。
        /// </summary>
        /// <returns>返回当前网站所有实例。</returns>
        protected override ConcurrentDictionary<int, TGroup> LoadCached()
        {
            return Cache.GetOrCreate(CacheKey, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var models = Context.Fetch();
                var groups = models.MakeDictionary();
                return new ConcurrentDictionary<int, TGroup>(groups);
            });
        }

        /// <summary>
        /// 缓存当前网站所有实例。
        /// </summary>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前网站所有实例。</returns>
        protected override Task<ConcurrentDictionary<int, TGroup>> LoadCachedAsync(CancellationToken cancellationToken)
        {
            return Cache.GetOrCreateAsync(CacheKey, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var models = await Context.FetchAsync(cancellationToken: cancellationToken);
                var groups = models.MakeDictionary();
                return new ConcurrentDictionary<int, TGroup>(groups);
            });
        }
    }
}
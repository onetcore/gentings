using Gentings.Data;
using Gentings.Data.Internal;
using Gentings.Data.Migrations;
using Gentings.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace Gentings.Extensions.Tagables
{
    /// <summary>
    /// 包含标签实体。
    /// </summary>
    public interface ITagable : IIdObject
    {
        /// <summary>
        /// 标签列表字段。
        /// </summary>
        string? Tags { get; set; }
    }

    /// <summary>
    /// 标签索引基类。
    /// </summary>
    public abstract class TagBase : IIdObject
    {
        /// <summary>
        /// 唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 标签名称。
        /// </summary>
        [Size(10)]
        public string? Name { get; set; }

        /// <summary>
        /// 标签首字母。
        /// </summary>
        [Size(10)]
        public string? Letters { get; set; }

        /// <summary>
        /// 是否已经人工审核确认过。
        /// </summary>
        public bool IsConfirm { get; set; }

        /// <summary>
        /// 索引大小。
        /// </summary>
        [NotUpdated]
        public int Count { get; set; }
    }

    /// <summary>
    /// 标签索引关联基类。
    /// </summary>
    public abstract class TagIndexBase
    {
        /// <summary>
        /// 标签Id。
        /// </summary>
        [Key]
        public int TagId { get; set; }

        /// <summary>
        /// 关联实体Id。
        /// </summary>
        [Key]
        public int IndexedId { get; set; }
    }

    /// <summary>
    /// 标签管理接口。
    /// </summary>
    /// <typeparam name="TTag">标签类型。</typeparam>
    /// <typeparam name="TTagIndex">标签索引类型。</typeparam>
    public interface ITagManager<TTag, TTagIndex> : ICachableObjectManager<TTag>
        where TTag : TagBase, new()
        where TTagIndex : TagIndexBase, new()
    {
        /// <summary>
        /// 通过名称获取标签Id。
        /// </summary>
        /// <param name="name">标签名称。</param>
        /// <returns>返回标签Id。</returns>
        int Find(string name);

        /// <summary>
        /// 通过名称获取标签Id。
        /// </summary>
        /// <param name="name">标签名称。</param>
        /// <returns>返回标签Id。</returns>
        Task<int> FindAsync(string name);

        /// <summary>
        /// 获取或者创建标签实例，并且返回标签对应的Id列表。
        /// </summary>
        /// <param name="tags">标签名称集合。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回标签对应的Id列表。</returns>
        List<int> GetOrCreate(string tags, char separator = ',');

        /// <summary>
        /// 获取或者创建标签实例，并且返回标签对应的Id列表。
        /// </summary>
        /// <param name="tags">标签名称集合。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回标签对应的Id列表。</returns>
        Task<List<int>> GetOrCreateAsync(string tags, char separator = ',');

        /// <summary>
        /// 执行统计清理服务。
        /// </summary>
        Task ExecuteAsync();
    }

    /// <summary>
    /// 标签管理接口。
    /// </summary>
    /// <typeparam name="TTag">标签类型。</typeparam>
    /// <typeparam name="TTagIndex">标签索引类型。</typeparam>
    public abstract class TagManager<TTag, TTagIndex> : CachableObjectManager<TTag>, ITagManager<TTag, TTagIndex>
        where TTag : TagBase, new()
        where TTagIndex : TagIndexBase, new()
    {
        /// <summary>
        /// 初始化类<see cref="TagManager{TTag, TTagIndex}"/>。
        /// </summary>
        /// <param name="context">标签数据库操作上下文。</param>
        /// <param name="cache">缓存接口。</param>
        protected TagManager(IDbContext<TTag> context, IMemoryCache cache)
            : base(context, cache)
        {
        }

        private static readonly string CacheKeyByName = $"tags[{typeof(TTag)}]";
        /// <summary>
        /// 刷新缓存。
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            Cache.Remove(CacheKeyByName);
        }

        private ConcurrentDictionary<string, int> LoadNamedIds()
        {
            return Cache.GetOrCreate(CacheKeyByName, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var tags = Context.Fetch();
                return new ConcurrentDictionary<string, int>(tags.ToDictionary(x => x.Name!, x => x.Id), StringComparer.OrdinalIgnoreCase);
            });
        }

        private Task<ConcurrentDictionary<string, int>> LoadNamedIdsAsync()
        {
            return Cache.GetOrCreateAsync(CacheKeyByName, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var tags = await Context.FetchAsync();
                return new ConcurrentDictionary<string, int>(tags.ToDictionary(x => x.Name!, x => x.Id), StringComparer.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// 通过名称获取标签Id。
        /// </summary>
        /// <param name="name">标签名称。</param>
        /// <returns>返回标签Id。</returns>
        public virtual int Find(string name)
        {
            var cached = LoadNamedIds();
            cached.TryGetValue(name, out int id);
            return id;
        }

        /// <summary>
        /// 通过名称获取标签Id。
        /// </summary>
        /// <param name="name">标签名称。</param>
        /// <returns>返回标签Id。</returns>
        public virtual async Task<int> FindAsync(string name)
        {
            var cached = await LoadNamedIdsAsync();
            cached.TryGetValue(name, out int id);
            return id;
        }

        /// <summary>
        /// 获取或者创建标签实例，并且返回标签对应的Id列表。
        /// </summary>
        /// <param name="tags">标签名称集合。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回标签对应的Id列表。</returns>
        public virtual List<int> GetOrCreate(string tags, char separator = ',')
        {
            var names = tags.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            var ids = new List<int>();
            var unames = new List<string>();
            var cached = LoadNamedIds();
            foreach (var name in names)
            {
                if (cached.TryGetValue(name, out var id))
                    ids.Add(id);
                else
                    unames.Add(name);
            }
            if (unames.Count > 0)
            {
                Context.BeginTransaction(db =>
                {
                    foreach (var name in unames)
                    {
                        var current = db.Find(x => x.Name == name);
                        if (current == null)
                        {
                            current = new TTag { Name = name };
                            if (!db.Create(current))
                                return false;
                        }
                        ids.Add(current.Id);
                    }
                    return true;
                });
            }
            return ids;
        }

        /// <summary>
        /// 获取或者创建标签实例，并且返回标签对应的Id列表。
        /// </summary>
        /// <param name="tags">标签名称集合。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回标签对应的Id列表。</returns>
        public virtual async Task<List<int>> GetOrCreateAsync(string tags, char separator = ',')
        {
            var names = tags.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            var ids = new List<int>();
            var unames = new List<string>();
            var cached = await LoadNamedIdsAsync();
            foreach (var name in names)
            {
                if (cached.TryGetValue(name, out var id))
                    ids.Add(id);
                else
                    unames.Add(name);
            }
            if (unames.Count > 0)
            {
                await Context.BeginTransactionAsync(async db =>
                {
                    foreach (var name in unames)
                    {
                        var current = await db.FindAsync(x => x.Name == name);
                        if (current == null)
                        {
                            current = new TTag { Name = name };
                            if (!await db.CreateAsync(current))
                                return false;
                        }
                        ids.Add(current.Id);
                    }
                    return true;
                });
            }
            return ids;
        }

        /// <summary>
        /// 执行统计清理服务。
        /// </summary>
        public virtual async Task ExecuteAsync()
        {
            var tagName = Context.EntityType.Table;
            var indexName = Context.As<TTagIndex>().EntityType.Table;
            await Context.ExecuteNonQueryAsync($@"UPDATE {tagName}
SET[Count] = b.[Count]
FROM(SELECT TagId, COUNT(IndexedId) as [Count] FROM {indexName} GROUP BY TagId)  b
WHERE b.TagId = {tagName}.Id; ");
        }
    }

    /// <summary>
    /// 标签扩展类。
    /// </summary>
    public static class TagExtensions
    {
        /// <summary>
        /// 添加<paramref name="target"/>的标签索引。
        /// </summary>
        /// <param name="context">事物上下文实例。</param>
        /// <param name="target">当前实例对象。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回添加结果。</returns>
        public static bool CreateTagIndex<TTarget, TTag, TTagIndex>(this IDbTransactionContext<TTarget> context, TTarget target, char separator = ',')
            where TTarget : ITagable, new()
            where TTag : TagBase, new()
            where TTagIndex : TagIndexBase, new()
        {
            if (string.IsNullOrWhiteSpace(target.Tags))
                return true;

            var indexdb = context.As<TTagIndex>();
            return indexdb.CreateIndex<TTarget, TTag, TTagIndex>(target, separator);
        }

        /// <summary>
        /// 添加<paramref name="target"/>的标签索引。
        /// </summary>
        /// <param name="context">事物上下文实例。</param>
        /// <param name="target">当前实例对象。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回添加结果。</returns>
        public static async Task<bool> CreateTagIndexAsync<TTarget, TTag, TTagIndex>(this IDbTransactionContext<TTarget> context, TTarget target, char separator = ',')
            where TTarget : ITagable, new()
            where TTag : TagBase, new()
            where TTagIndex : TagIndexBase, new()
        {
            if (string.IsNullOrWhiteSpace(target.Tags))
                return true;

            var indexdb = context.As<TTagIndex>();
            return await indexdb.CreateIndexAsync<TTarget, TTag, TTagIndex>(target, separator);
        }

        /// <summary>
        /// 更新<paramref name="target"/>的标签索引。
        /// </summary>
        /// <param name="context">事物上下文实例。</param>
        /// <param name="target">当前实例对象。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回添加结果。</returns>
        public static bool UpdateTagIndex<TTarget, TTag, TTagIndex>(this IDbTransactionContext<TTarget> context, TTarget target, char separator = ',')
            where TTarget : ITagable, new()
            where TTag : TagBase, new()
            where TTagIndex : TagIndexBase, new()
        {
            var indexdb = context.As<TTagIndex>();
            indexdb.Delete(x => x.IndexedId == target.Id);
            if (string.IsNullOrWhiteSpace(target.Tags))
                return true;

            return indexdb.CreateIndex<TTarget, TTag, TTagIndex>(target, separator);
        }

        /// <summary>
        /// 更新<paramref name="target"/>的标签索引。
        /// </summary>
        /// <param name="context">事物上下文实例。</param>
        /// <param name="target">当前实例对象。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回添加结果。</returns>
        public static async Task<bool> UpdateTagIndexAsync<TTarget, TTag, TTagIndex>(this IDbTransactionContext<TTarget> context, TTarget target, char separator = ',')
            where TTarget : ITagable, new()
            where TTag : TagBase, new()
            where TTagIndex : TagIndexBase, new()
        {
            var indexdb = context.As<TTagIndex>();
            await indexdb.DeleteAsync(x => x.IndexedId == target.Id);
            if (string.IsNullOrWhiteSpace(target.Tags))
                return true;

            return await indexdb.CreateIndexAsync<TTarget, TTag, TTagIndex>(target, separator);
        }

        private static bool CreateIndex<TTarget, TTag, TTagIndex>(this IDbTransactionContext<TTagIndex> context, TTarget target, char separator = ',')
            where TTarget : ITagable, new()
            where TTag : TagBase, new()
            where TTagIndex : TagIndexBase, new()
        {
            var tags = target.Tags!.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            var tagdb = context.As<TTag>();
            foreach (var tag in tags)
            {
                var current = tagdb.Find(x => x.Name == tag);
                if (current == null)
                {
                    current = new TTag { Name = tag };
                    if (!tagdb.Create(current))
                        return false;
                }
                var index = new TTagIndex { IndexedId = target.Id, TagId = current.Id };
                if (!context.Create(index))
                    return false;
            }
            return true;
        }

        private static async Task<bool> CreateIndexAsync<TTarget, TTag, TTagIndex>(this IDbTransactionContext<TTagIndex> context, TTarget target, char separator = ',')
            where TTarget : ITagable, new()
            where TTag : TagBase, new()
            where TTagIndex : TagIndexBase, new()
        {
            var tags = target.Tags!.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            var tagdb = context.As<TTag>();
            foreach (var tag in tags)
            {
                var current = await tagdb.FindAsync(x => x.Name == tag);
                if (current == null)
                {
                    current = new TTag { Name = tag };
                    if (!await tagdb.CreateAsync(current))
                        return false;
                }
                var index = new TTagIndex { IndexedId = target.Id, TagId = current.Id };
                if (!await context.CreateAsync(index))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        /// <param name="tags">标签Id列表。</param>
        public static void Where<TTarget, TTag, TTagIndex>(this IQueryContext<TTarget> context, int[] tags)
            where TTarget : ITagable, new()
            where TTag : TagBase, new()
            where TTagIndex : TagIndexBase, new()
        {
            context.InnerJoin<TTagIndex>((t, i) => t.Id == i.IndexedId)
                .InnerJoin<TTagIndex, TTag>((i, t) => i.TagId == t.Id)
                .Where<TTag>(t => t.Id.Included(tags));
        }
    }

    /// <summary>
    /// 标签统计清理后台服务。
    /// </summary>
    /// <typeparam name="TTag">标签类型。</typeparam>
    /// <typeparam name="TTagIndex">标签索引类型。</typeparam>
    public abstract class TagTaskService<TTag, TTagIndex> : TaskService
        where TTag : TagBase, new()
        where TTagIndex : TagIndexBase, new()
    {
        private readonly ITagManager<TTag, TTagIndex> _tagManager;
        /// <summary>
        /// 初始化类<see cref="TagTaskService{TTag, TTagIndex}"/>。
        /// </summary>
        /// <param name="tagManager">标签管理接口。</param>
        protected TagTaskService(ITagManager<TTag, TTagIndex> tagManager)
        {
            _tagManager = tagManager;
        }

        /// <summary>
        /// 名称。
        /// </summary>
        public override string Name => $"{typeof(TTag).Name}标签后台服务";

        /// <summary>
        /// 描述。
        /// </summary>
        public override string Description => $"{typeof(TTag).Name}标签统计清理后台服务";

        /// <summary>
        /// 执行间隔时间。
        /// </summary>
        public override TaskInterval Interval => new TimeSpan(3, 0, 0);

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="argument">参数。</param>
        public override async Task ExecuteAsync(Argument argument)
        {
            await _tagManager.ExecuteAsync();
        }
    }

    /// <summary>
    /// 数据库迁移基类。
    /// </summary>
    /// <typeparam name="TTag">标签类型。</typeparam>
    /// <typeparam name="TTagIndex">标签索引类型。</typeparam>
    public abstract class DataMigration<TTag, TTagIndex> : DataMigration
        where TTag : TagBase, new()
        where TTagIndex : TagIndexBase, new()
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<TTag>(table => table
                .Column(x => x.Id)
                .Column(x => x.Name)
                .Column(x => x.Count)
                .Column(x => x.Letters)
                .Column(x => x.IsConfirm)
                .UniqueConstraint(x => x.Name)
            );

            builder.CreateTable<TTagIndex>(table => table
                .Column(x => x.IndexedId)
                .Column(x => x.TagId)
                .ForeignKey<TTag>(x => x.TagId, x => x.Id, onDelete: ReferentialAction.Cascade)
            );
        }
    }
}

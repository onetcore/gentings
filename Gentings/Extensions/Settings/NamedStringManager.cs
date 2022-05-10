using Microsoft.Extensions.Caching.Memory;
using Gentings.Data;
using Gentings.Extensions.Groups;
using System.Collections.Concurrent;

namespace Gentings.Extensions.Settings
{
    /// <summary>
    /// 字典管理实现类。
    /// </summary>
    public class NamedStringManager : GroupManager<NamedString>, INamedStringManager
    {
        private static readonly object _pathCacheKey = new Tuple<Type, string>(typeof(NamedString), "path");

        /// <summary>
        /// 初始化类<see cref="NamedStringManager"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="cache">缓存接口。</param>
        public NamedStringManager(IDbContext<NamedString> context, IMemoryCache cache)
            : base(context, cache)
        {
        }

        private ConcurrentDictionary<string, NamedString> LoadPathCache()
        {
            return Cache.GetOrCreate(_pathCacheKey, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var strings = Fetch().ToDictionary(x => x.Path);
                return new ConcurrentDictionary<string, NamedString>(strings, StringComparer.OrdinalIgnoreCase);
            });
        }

        private Task<ConcurrentDictionary<string, NamedString>> LoadPathCacheAsync()
        {
            return Cache.GetOrCreateAsync(_pathCacheKey, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var strings = (await FetchAsync()).ToDictionary(x => x.Path);
                return new ConcurrentDictionary<string, NamedString>(strings, StringComparer.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            Cache.Remove(_pathCacheKey);
        }

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        public virtual string? GetString(string path)
        {
            var strings = LoadPathCache();
            if (strings.TryGetValue(path, out var ns))
                return ns.Value;
            return null;
        }

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        public virtual async Task<string?> GetStringAsync(string path)
        {
            var strings = await LoadPathCacheAsync();
            if (strings.TryGetValue(path, out var ns))
                return ns.Value;
            return null;
        }

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        public virtual string? GetOrAddString(string path)
        {
            var strings = LoadPathCache();
            if (strings.TryGetValue(path, out var ns))
                return ns.Value;

            if (Context.BeginTransaction(db =>
            {
                var names = path.Split('.');
                var parentId = 0;
                foreach (var name in names)
                {
                    ns = db.Find(x => x.Name == name && x.ParentId == parentId);
                    if (ns == null)
                    {
                        ns = new NamedString { ParentId = parentId, Name = name, Value = name };
                        if (db.Create(ns))
                            parentId = ns.Id;
                        else
                            return false;
                    }
                    else
                    {
                        parentId = ns.Id;
                    }
                }

                return true;
            }))
            {
                Refresh();
                return ns!.Value;
            }

            return null;
        }

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        public virtual async Task<string?> GetOrAddStringAsync(string path)
        {
            var strings = await LoadPathCacheAsync();
            if (strings.TryGetValue(path, out var ns))
                return ns.Value;

            if (await Context.BeginTransactionAsync(async db =>
            {
                var names = path.Split('.');
                var parentId = 0;
                foreach (var name in names)
                {
                    ns = await db.FindAsync(x => x.Name == name && x.ParentId == parentId);
                    if (ns == null)
                    {
                        ns = new NamedString { ParentId = parentId, Name = name, Value = name };
                        if (await db.CreateAsync(ns))
                            parentId = ns.Id;
                        else
                            return false;
                    }
                    else
                    {
                        parentId = ns.Id;
                    }
                }

                return true;
            }))
            {
                Refresh();
                return ns?.Value;
            }

            return null;
        }
    }
}
using Microsoft.Extensions.Caching.Memory;
using Gentings.Data;
using Gentings.Extensions.Groups;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Gentings.Extensions.Settings
{
    /// <summary>
    /// 字典管理实现类。
    /// </summary>
    public class SettingDictionaryManager : GroupManager<SettingDictionary>, ISettingDictionaryManager
    {
        private static readonly object _pathCacheKey = new Tuple<Type, string>(typeof(SettingDictionary), "path");
        /// <summary>
        /// 初始化类<see cref="SettingDictionaryManager"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="cache">缓存接口。</param>
        public SettingDictionaryManager(IDbContext<SettingDictionary> context, IMemoryCache cache)
            : base(context, cache)
        {
        }

        private ConcurrentDictionary<string, SettingDictionary> LoadPathCache()
        {
            return Cache.GetOrCreate(_pathCacheKey, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                System.Collections.Generic.Dictionary<string, SettingDictionary> settings = Fetch().ToDictionary(x => x.Path);
                return new ConcurrentDictionary<string, SettingDictionary>(settings, StringComparer.OrdinalIgnoreCase);
            });
        }

        private Task<ConcurrentDictionary<string, SettingDictionary>> LoadPathCacheAsync()
        {
            return Cache.GetOrCreateAsync(_pathCacheKey, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                System.Collections.Generic.Dictionary<string, SettingDictionary> settings = (await FetchAsync()).ToDictionary(x => x.Path);
                return new ConcurrentDictionary<string, SettingDictionary>(settings, StringComparer.OrdinalIgnoreCase);
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
        public virtual string GetSettings(string path)
        {
            ConcurrentDictionary<string, SettingDictionary> settings = LoadPathCache();
            settings.TryGetValue(path, out SettingDictionary value);
            return value;
        }

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        public virtual async Task<string> GetSettingsAsync(string path)
        {
            ConcurrentDictionary<string, SettingDictionary> settings = await LoadPathCacheAsync();
            settings.TryGetValue(path, out SettingDictionary value);
            return value;
        }

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        public virtual string GetOrAddSettings(string path)
        {
            ConcurrentDictionary<string, SettingDictionary> settings = LoadPathCache();
            if (settings.TryGetValue(path, out SettingDictionary setting))
                return setting;
            if (Context.BeginTransaction(db =>
            {
                string[] names = path.Split('.');
                int parentId = 0;
                foreach (string name in names)
                {
                    setting = db.Find(x => x.Name == name && x.ParentId == parentId);
                    if (setting == null)
                    {
                        setting = new SettingDictionary { ParentId = parentId, Name = name, Value = name };
                        if (db.Create(setting))
                            parentId = setting.Id;
                        else
                            return false;
                    }
                    else
                    {
                        parentId = setting.Id;
                    }
                }

                return true;
            }))
            {
                Refresh();
                return setting;
            }
            return null;
        }

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        public virtual async Task<string> GetOrAddSettingsAsync(string path)
        {
            ConcurrentDictionary<string, SettingDictionary> settings = await LoadPathCacheAsync();
            if (settings.TryGetValue(path, out SettingDictionary setting))
                return setting;
            if (await Context.BeginTransactionAsync(async db =>
            {
                string[] names = path.Split('.');
                int parentId = 0;
                foreach (string name in names)
                {
                    setting = await db.FindAsync(x => x.Name == name && x.ParentId == parentId);
                    if (setting == null)
                    {
                        setting = new SettingDictionary { ParentId = parentId, Name = name, Value = name };
                        if (await db.CreateAsync(setting))
                            parentId = setting.Id;
                        else
                            return false;
                    }
                    else
                    {
                        parentId = setting.Id;
                    }
                }

                return true;
            }))
            {
                Refresh();
                return setting;
            }
            return null;
        }
    }
}
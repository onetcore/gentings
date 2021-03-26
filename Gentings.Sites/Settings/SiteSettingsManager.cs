using System.Threading.Tasks;
using Gentings.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Sites.Settings
{
    /// <summary>
    /// 网站配置管理类。
    /// </summary>
    public class SiteSettingsManager : ISiteSettingsManager
    {
        /// <summary>
        /// 数据库操作接口。
        /// </summary>
        protected IDbContext<SiteSettingsAdapter> Context { get; }

        /// <summary>
        /// 缓存对象。
        /// </summary>
        protected IMemoryCache Cache { get; }

        /// <summary>
        /// 获取缓存键。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">唯一键。</param>
        /// <returns></returns>
        protected virtual string GetCacheKey(int siteId, string key) => $"Sites[{siteId}][{key}]";

        /// <summary>
        /// 初始化类<see cref="SiteSettingsManager"/>。
        /// </summary>
        /// <param name="context">数据库操作接口。</param>
        /// <param name="cache">缓存接口。</param>
        public SiteSettingsManager(IDbContext<SiteSettingsAdapter> context, IMemoryCache cache)
        {
            Context = context;
            Cache = cache;
        }

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        public virtual string GetSettings(int siteId, string key)
        {
            return Cache.GetOrCreate(GetCacheKey(siteId, key), entry =>
            {
                entry.SetDefaultAbsoluteExpiration();
                return Context.Find(x => x.SiteId == siteId && x.SettingKey == key)?.SettingValue;
            });
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual TSiteSettings GetSettings<TSiteSettings>(int siteId, string key) where TSiteSettings : ISite, new()
        {
            return Cache.GetOrCreate(GetCacheKey(siteId, key), entry =>
            {
                entry.SetDefaultAbsoluteExpiration();
                var settings = Context.Find(x => x.SiteId == siteId && x.SettingKey == key)?.SettingValue;
                if (settings == null)
                    return new TSiteSettings { SiteId = siteId };

                var result = Cores.FromJsonString<TSiteSettings>(settings);
                if (result == null)
                    result = new TSiteSettings();
                result.SiteId = siteId;
                return result;
            });
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual TSiteSettings GetSettings<TSiteSettings>(int siteId) where TSiteSettings : ISite, new()
        {
            return GetSettings<TSiteSettings>(siteId, typeof(TSiteSettings).FullName);
        }

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        public virtual Task<string> GetSettingsAsync(int siteId, string key)
        {
            return Cache.GetOrCreateAsync(GetCacheKey(siteId, key), async entry =>
            {
                entry.SetDefaultAbsoluteExpiration();
                var settings = await Context.FindAsync(x => x.SiteId == siteId && x.SettingKey == key);
                return settings?.SettingValue;
            });
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(int siteId, string key) where TSiteSettings : ISite, new()
        {
            return Cache.GetOrCreateAsync(GetCacheKey(siteId, key), async entry =>
            {
                entry.SetDefaultAbsoluteExpiration();
                var settings = await Context.FindAsync(x => x.SettingKey == key);
                if (settings?.SettingValue == null)
                    return new TSiteSettings { SiteId = siteId };

                var result = Cores.FromJsonString<TSiteSettings>(settings.SettingValue);
                if (result == null)
                    result = new TSiteSettings();
                result.SiteId = siteId;
                return result;
            });
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(int siteId) where TSiteSettings : ISite, new()
        {
            return GetSettingsAsync<TSiteSettings>(siteId, typeof(TSiteSettings).FullName);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="settings">网站配置实例。</param>
        public virtual Task<bool> SaveSettingsAsync<TSiteSettings>(TSiteSettings settings) where TSiteSettings : ISite, new()
        {
            return SaveSettingsAsync(settings.SiteId, typeof(TSiteSettings).FullName, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual Task<bool> SaveSettingsAsync<TSiteSettings>(int siteId, string key, TSiteSettings settings)
        {
            return SaveSettingsAsync(siteId, key, settings.ToJsonString());
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        /// <param name="siteId">网站Id。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual async Task<bool> SaveSettingsAsync(int siteId, string key, string settings)
        {
            var adapter = new SiteSettingsAdapter { SiteId = siteId, SettingKey = key, SettingValue = settings };
            if (await Context.AnyAsync(x => x.SiteId == siteId && x.SettingKey == key))
            {
                if (await Context.UpdateAsync(adapter))
                {
                    Refresh(siteId, key);
                    return true;
                }
            }

            if (await Context.CreateAsync(adapter))
            {
                Refresh(siteId, key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings<TSiteSettings>(TSiteSettings settings) where TSiteSettings : ISite, new()
        {
            return SaveSettings(settings.SiteId, typeof(TSiteSettings).FullName, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings<TSiteSettings>(int siteId, string key, TSiteSettings settings)
        {
            return SaveSettings(siteId, key, settings.ToJsonString());
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings(int siteId, string key, string settings)
        {
            var adapter = new SiteSettingsAdapter { SiteId = siteId, SettingKey = key, SettingValue = settings };
            if (Context.Any(x => x.SiteId == siteId && x.SettingKey == key))
            {
                if (Context.Update(adapter))
                {
                    Refresh(siteId, key);
                    return true;
                }
            }

            if (Context.Create(adapter))
            {
                Refresh(siteId, key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        public virtual void Refresh(int siteId, string key)
        {
            Cache.Remove(GetCacheKey(siteId, key));
        }

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        public virtual bool DeleteSettings<TSiteSettings>(int siteId) =>
            DeleteSettings(siteId, typeof(TSiteSettings).FullName);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        public virtual bool DeleteSettings(int siteId, string key)
        {
            if (Context.Delete(x => x.SiteId == siteId && x.SettingKey == key))
            {
                Refresh(siteId, key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        public virtual Task<bool> DeleteSettingsAsync<TSiteSettings>(int siteId) =>
            DeleteSettingsAsync(siteId, typeof(TSiteSettings).FullName);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        public virtual async Task<bool> DeleteSettingsAsync(int siteId, string key)
        {
            if (await Context.DeleteAsync(x => x.SiteId == siteId && x.SettingKey == key))
            {
                Refresh(siteId, key);
                return true;
            }

            return false;
        }
    }
}
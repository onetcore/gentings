﻿using System.Threading.Tasks;
using Gentings.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Security.Settings
{
    /// <summary>
    /// 网站配置管理类。
    /// </summary>
    public class SettingsManager : ISettingsManager
    {
        /// <summary>
        /// 数据库操作接口。
        /// </summary>
        protected IDbContext<SettingsAdapter> Context { get; }

        /// <summary>
        /// 缓存对象。
        /// </summary>
        protected IMemoryCache Cache { get; }

        /// <summary>
        /// 初始化类<see cref="SettingsManager"/>。
        /// </summary>
        /// <param name="context">数据库操作接口。</param>
        /// <param name="cache">缓存接口。</param>
        public SettingsManager(IDbContext<SettingsAdapter> context, IMemoryCache cache)
        {
            Context = context;
            Cache = cache;
        }

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        public virtual string GetSettings(int userId, string key)
        {
            return Cache.GetOrCreate(GetCacheKey(userId, key), entry =>
            {
                entry.SetDefaultAbsoluteExpiration();
                return Context.Find(x => x.UserId == userId && x.SettingKey == key)?.SettingValue;
            });
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual TSiteSettings GetSettings<TSiteSettings>(int userId, string key) where TSiteSettings : class, new()
        {
            return Cache.GetOrCreate(GetCacheKey(userId, key), entry =>
            {
                entry.SetDefaultAbsoluteExpiration();
                var settings = Context.Find(x => x.UserId == userId && x.SettingKey == key)?.SettingValue;
                if (settings == null)
                {
                    return new TSiteSettings();
                }

                return Cores.FromJsonString<TSiteSettings>(settings);
            });
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual TSiteSettings GetSettings<TSiteSettings>(int userId) where TSiteSettings : class, new()
        {
            return GetSettings<TSiteSettings>(userId, typeof(TSiteSettings).FullName);
        }

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        public virtual Task<string> GetSettingsAsync(int userId, string key)
        {
            return Cache.GetOrCreateAsync(GetCacheKey(userId, key), async entry =>
            {
                entry.SetDefaultAbsoluteExpiration();
                var settings = await Context.FindAsync(x => x.UserId == userId && x.SettingKey == key);
                return settings?.SettingValue;
            });
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(int userId, string key)
            where TSiteSettings : class, new()
        {
            return Cache.GetOrCreateAsync(GetCacheKey(userId, key), async entry =>
            {
                entry.SetDefaultAbsoluteExpiration();
                var settings = await Context.FindAsync(x => x.UserId == userId && x.SettingKey == key);
                if (settings?.SettingValue == null)
                {
                    return new TSiteSettings();
                }

                return Cores.FromJsonString<TSiteSettings>(settings.SettingValue);
            });
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(int userId) where TSiteSettings : class, new()
        {
            return GetSettingsAsync<TSiteSettings>(userId, typeof(TSiteSettings).FullName);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual Task<bool> SaveSettingsAsync<TSiteSettings>(int userId, TSiteSettings settings)
            where TSiteSettings : class, new()
        {
            return SaveSettingsAsync(userId, typeof(TSiteSettings).FullName, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual Task<bool> SaveSettingsAsync<TSiteSettings>(int userId, string key, TSiteSettings settings)
        {
            return SaveSettingsAsync(userId, key, settings.ToJsonString());
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual async Task<bool> SaveSettingsAsync(int userId, string key, string settings)
        {
            var adapter = new SettingsAdapter { UserId = userId, SettingKey = key, SettingValue = settings };
            if (await Context.AnyAsync(x => x.UserId == userId && x.SettingKey == key))
            {
                if (await Context.UpdateAsync(adapter))
                {
                    Refresh(userId, key);
                    return true;
                }
            }

            if (await Context.CreateAsync(adapter))
            {
                Refresh(userId, key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings<TSiteSettings>(int userId, TSiteSettings settings) where TSiteSettings : class, new()
        {
            return SaveSettings(userId, typeof(TSiteSettings).FullName, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings<TSiteSettings>(int userId, string key, TSiteSettings settings)
        {
            return SaveSettings(userId, key, settings.ToJsonString());
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings(int userId, string key, string settings)
        {
            var adapter = new SettingsAdapter { UserId = userId, SettingKey = key, SettingValue = settings };
            if (Context.Any(x => x.UserId == userId && x.SettingKey == key))
            {
                if (Context.Update(adapter))
                {
                    Refresh(userId, key);
                    return true;
                }
            }

            if (Context.Create(adapter))
            {
                Refresh(userId, key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        public virtual void Refresh(int userId, string key)
        {
            Cache.Remove(GetCacheKey(userId, key));
        }

        /// <summary>
        /// 获取缓存键。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回缓存键字符串。</returns>
        protected virtual string GetCacheKey(int userId, string key) => $"settings:{userId}:{key}";

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        public virtual bool DeleteSettings<TSiteSettings>(int userId) => DeleteSettings(userId, typeof(TSiteSettings).FullName);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        public virtual bool DeleteSettings(int userId, string key)
        {
            if (Context.Delete(x=>x.UserId == userId && x.SettingKey == key))
            {
                Refresh(userId, key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        public virtual Task<bool> DeleteSettingsAsync<TSiteSettings>(int userId) => DeleteSettingsAsync(userId, typeof(TSiteSettings).FullName);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        public virtual async Task<bool> DeleteSettingsAsync(int userId, string key)
        {
            if (await Context.DeleteAsync(x => x.UserId == userId && x.SettingKey == key))
            {
                Refresh(userId, key);
                return true;
            }

            return false;
        }
    }
}
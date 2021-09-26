using System.Threading.Tasks;
using Gentings.Extensions;
using Microsoft.AspNetCore.Http;

namespace Gentings.Security.Settings
{
    /// <summary>
    /// 网站配置管理类。
    /// </summary>
    public class UserSettingsManager : IUserSettingsManager
    {
        private readonly Extensions.Settings.ISettingsManager _sysManager;
        private readonly ISettingsManager _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 初始化类<see cref="UserSettingsManager"/>。
        /// </summary>
        /// <param name="context">数据库操作接口。</param>
        /// <param name="cache">缓存接口。</param>
        public UserSettingsManager(Extensions.Settings.ISettingsManager settingsManager, ISettingsManager securitySettingsManager, IHttpContextAccessor contextAccessor)
        {
            _sysManager = settingsManager;
            _userManager = securitySettingsManager;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 当前用户Id。
        /// </summary>
        protected int UserId => _contextAccessor.HttpContext.User.GetUserId();

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        public virtual string GetSettings(string key)
        {
            return UserId == 0 ? _sysManager.GetSettings(key) : _userManager.GetSettings(UserId, key);
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual TSiteSettings GetSettings<TSiteSettings>(string key) where TSiteSettings : class, new()
        {
            return UserId == 0 ? _sysManager.GetSettings<TSiteSettings>(key) : _userManager.GetSettings<TSiteSettings>(UserId, key);
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <returns>返回网站配置实例。</returns>
        public virtual TSiteSettings GetSettings<TSiteSettings>() where TSiteSettings : class, new()
        {
            return UserId == 0 ? _sysManager.GetSettings<TSiteSettings>() : _userManager.GetSettings<TSiteSettings>(UserId);
        }

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        public virtual Task<string> GetSettingsAsync(string key)
        {
            return UserId == 0 ? _sysManager.GetSettingsAsync(key) : _userManager.GetSettingsAsync(UserId, key);
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        public virtual Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(string key)
            where TSiteSettings : class, new()
        {
            return UserId == 0 ? _sysManager.GetSettingsAsync<TSiteSettings>(key) : _userManager.GetSettingsAsync<TSiteSettings>(UserId, key);
        }

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <returns>返回网站配置实例。</returns>
        public virtual Task<TSiteSettings> GetSettingsAsync<TSiteSettings>() where TSiteSettings : class, new()
        {
            return UserId == 0 ? _sysManager.GetSettingsAsync<TSiteSettings>() : _userManager.GetSettingsAsync<TSiteSettings>(UserId);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="settings">网站配置实例。</param>
        public virtual Task<bool> SaveSettingsAsync<TSiteSettings>(TSiteSettings settings)
            where TSiteSettings : class, new()
        {
            return UserId == 0 ? _sysManager.SaveSettingsAsync(settings) : _userManager.SaveSettingsAsync(UserId, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual Task<bool> SaveSettingsAsync<TSiteSettings>(string key, TSiteSettings settings)
        {
            return UserId == 0 ? _sysManager.SaveSettingsAsync(key, settings) : _userManager.SaveSettingsAsync(UserId, key, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual Task<bool> SaveSettingsAsync(string key, string settings)
        {
            return UserId == 0 ? _sysManager.SaveSettingsAsync(key, settings) : _userManager.SaveSettingsAsync(UserId, key, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings<TSiteSettings>(TSiteSettings settings) where TSiteSettings : class, new()
        {
            return UserId == 0 ? _sysManager.SaveSettings(settings) : _userManager.SaveSettings(UserId, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings<TSiteSettings>(string key, TSiteSettings settings)
        {
            return UserId == 0 ? _sysManager.SaveSettings(key, settings) : _userManager.SaveSettings(UserId, key, settings);
        }

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        public virtual bool SaveSettings(string key, string settings)
        {
            return UserId == 0 ? _sysManager.SaveSettings(key, settings) : _userManager.SaveSettings(UserId, key, settings);
        }

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        public virtual void Refresh(string key)
        {
            if (UserId == 0)
                _sysManager.Refresh(key);
            else
                _userManager.Refresh(UserId, key);
        }

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        public virtual bool DeleteSettings<TSiteSettings>()
        {
            return UserId == 0 ? _sysManager.DeleteSettings<TSiteSettings>() : _userManager.DeleteSettings<TSiteSettings>(UserId);
        }

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        public virtual bool DeleteSettings(string key)
        {
            return UserId == 0 ? _sysManager.DeleteSettings(key) : _userManager.DeleteSettings(UserId, key);
        }

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        public virtual Task<bool> DeleteSettingsAsync<TSiteSettings>()
        {
            return UserId == 0 ? _sysManager.DeleteSettingsAsync<TSiteSettings>() : _userManager.DeleteSettingsAsync<TSiteSettings>(UserId);
        }

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        public virtual Task<bool> DeleteSettingsAsync(string key)
        {
            return UserId == 0 ? _sysManager.DeleteSettingsAsync(key) : _userManager.DeleteSettingsAsync(UserId, key);
        }
    }
}
using System.Threading.Tasks;

namespace Gentings.Security.Settings
{
    /// <summary>
    /// 配置管理接口。
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        string GetSettings(string key);

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        TSiteSettings GetSettings<TSiteSettings>(string key)
            where TSiteSettings : class, new();

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <returns>返回网站配置实例。</returns>
        TSiteSettings GetSettings<TSiteSettings>()
            where TSiteSettings : class, new();

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        Task<string> GetSettingsAsync(string key);

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(string key)
            where TSiteSettings : class, new();

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <returns>返回网站配置实例。</returns>
        Task<TSiteSettings> GetSettingsAsync<TSiteSettings>()
            where TSiteSettings : class, new();

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        string GetSettings(int userId, string key);

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        TSiteSettings GetSettings<TSiteSettings>(int userId, string key)
            where TSiteSettings : class, new();

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回网站配置实例。</returns>
        TSiteSettings GetSettings<TSiteSettings>(int userId)
            where TSiteSettings : class, new();

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        Task<string> GetSettingsAsync(int userId, string key);

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(int userId, string key)
            where TSiteSettings : class, new();

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回网站配置实例。</returns>
        Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(int userId)
            where TSiteSettings : class, new();

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="settings">网站配置实例。</param>
        Task<bool> SaveSettingsAsync<TSiteSettings>(int userId, TSiteSettings settings)
            where TSiteSettings : class, new();

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        Task<bool> SaveSettingsAsync<TSiteSettings>(int userId, string key, TSiteSettings settings);

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        Task<bool> SaveSettingsAsync(int userId, string key, string settings);

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="settings">网站配置实例。</param>
        bool SaveSettings<TSiteSettings>(int userId, TSiteSettings settings)
            where TSiteSettings : class, new();

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        bool SaveSettings<TSiteSettings>(int userId, string key, TSiteSettings settings);

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        bool SaveSettings(int userId, string key, string settings);

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        void Refresh(int userId, string key);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        bool DeleteSettings<TSiteSettings>(int userId);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        bool DeleteSettings(int userId, string key);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="userId">用户Id。</param>
        Task<bool> DeleteSettingsAsync<TSiteSettings>(int userId);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="key">配置唯一键。</param>
        Task<bool> DeleteSettingsAsync(int userId, string key);
    }
}
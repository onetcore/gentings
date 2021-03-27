using System.Threading.Tasks;

namespace Gentings.Saas.Settings
{
    /// <summary>
    /// 配置管理接口。
    /// </summary>
    public interface ISiteSettingsManager : ISingletonService
    {
        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        string GetSettings(int siteId, string key);

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        TSiteSettings GetSettings<TSiteSettings>(int siteId, string key)
            where TSiteSettings : ISite, new();

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回网站配置实例。</returns>
        TSiteSettings GetSettings<TSiteSettings>(int siteId)
            where TSiteSettings : ISite, new();

        /// <summary>
        /// 获取配置字符串。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回当前配置字符串实例。</returns>
        Task<string> GetSettingsAsync(int siteId, string key);

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <returns>返回网站配置实例。</returns>
        Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(int siteId, string key)
            where TSiteSettings : ISite, new();

        /// <summary>
        /// 获取网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回网站配置实例。</returns>
        Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(int siteId)
            where TSiteSettings : ISite, new();

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="settings">网站配置实例。</param>
        Task<bool> SaveSettingsAsync<TSiteSettings>(TSiteSettings settings)
            where TSiteSettings : ISite, new();

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        Task<bool> SaveSettingsAsync<TSiteSettings>(int siteId, string key, TSiteSettings settings);

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="key">配置唯一键。</param>
        /// <param name="siteId">网站Id。</param>
        /// <param name="settings">网站配置实例。</param>
        Task<bool> SaveSettingsAsync(int siteId, string key, string settings);

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="settings">网站配置实例。</param>
        bool SaveSettings<TSiteSettings>(TSiteSettings settings)
            where TSiteSettings : ISite, new();

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        bool SaveSettings<TSiteSettings>(int siteId, string key, TSiteSettings settings);

        /// <summary>
        /// 保存网站配置实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        /// <param name="settings">网站配置实例。</param>
        bool SaveSettings(int siteId, string key, string settings);

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        void Refresh(int siteId, string key);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        bool DeleteSettings<TSiteSettings>(int siteId);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        bool DeleteSettings(int siteId, string key);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
        /// <param name="siteId">网站Id。</param>
        Task<bool> DeleteSettingsAsync<TSiteSettings>(int siteId);

        /// <summary>
        /// 删除网站配置实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="key">配置唯一键。</param>
        Task<bool> DeleteSettingsAsync(int siteId, string key);
    }
}
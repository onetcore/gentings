using System.Threading.Tasks;

namespace Gentings.Extensions.Emails
{
    /// <summary>
    /// 电子邮件配置管理接口。
    /// </summary>
    public interface IEmailSettingsManager : ICachableObjectManager<EmailSettings>, ISingletonService
    {
        /// <summary>
        /// 是否开启电子邮件系统，主要检查看是否有激活的配置。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        bool IsEnabled();

        /// <summary>
        /// 是否开启电子邮件系统，主要检查看是否有激活的配置。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsEnabledAsync();

        /// <summary>
        /// 获取当前可用的配置。
        /// </summary>
        /// <returns>返回当前可用的配置。</returns>
        EmailSettings GetSettings();

        /// <summary>
        /// 获取当前可用的配置。
        /// </summary>
        /// <returns>返回当前可用的配置。</returns>
        Task<EmailSettings> GetSettingsAsync();
    }
}
using Gentings.Data;
using Gentings.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;

namespace Gentings.Messages.Emails
{
    /// <summary>
    /// 电子邮件配置管理类。
    /// </summary>
    public class EmailSettingsManager : CachableObjectManager<EmailSettings>, IEmailSettingsManager
    {
        /// <summary>
        /// 初始化类<see cref="EmailSettingsManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        public EmailSettingsManager(IDbContext<EmailSettings> context, IMemoryCache cache)
            : base(context, cache)
        {
        }

        /// <summary>
        /// 获取当前可用的配置。
        /// </summary>
        /// <returns>返回当前可用的配置。</returns>
        public virtual EmailSettings GetSettings()
        {
            return Fetch(x => x.Enabled).OrderByDescending(x => x.Count).FirstOrDefault();
        }

        /// <summary>
        /// 获取当前可用的配置。
        /// </summary>
        /// <returns>返回当前可用的配置。</returns>
        public virtual async Task<EmailSettings> GetSettingsAsync()
        {
            var settings = await FetchAsync(x => x.Enabled);
            return settings.OrderByDescending(x => x.Count).FirstOrDefault();
        }

        /// <summary>
        /// 是否开启电子邮件系统，主要检查看是否有激活的配置。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsEnabled()
        {
            return Fetch().Any(x => x.Enabled);
        }

        /// <summary>
        /// 是否开启电子邮件系统，主要检查看是否有激活的配置。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public virtual async Task<bool> IsEnabledAsync()
        {
            var settings = await FetchAsync();
            return settings.Any(x => x.Enabled);
        }
    }
}
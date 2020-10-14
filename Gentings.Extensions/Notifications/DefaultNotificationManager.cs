using Gentings.Data;
using Gentings.Settings;
using Microsoft.AspNetCore.Http;

namespace Gentings.Extensions.Notifications
{
    internal class DefaultNotificationManager : NotificationManager
    {
        /// <summary>
        /// 初始化类<see cref="DefaultNotificationManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="httpContextAccessor">Http上下文访问接口。</param>
        /// <param name="settingsManager">配置管理接口。</param>
        public DefaultNotificationManager(IDbContext<Notification> context, IHttpContextAccessor httpContextAccessor, ISettingsManager settingsManager) : base(context, httpContextAccessor, settingsManager)
        {
        }
    }
}
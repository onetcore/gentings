using Gentings.Data;
using Gentings.Extensions;

namespace Gentings.Identity.Notifications
{
    /// <summary>
    /// 通知管理接口。
    /// </summary>
    public interface INotificationManager : IObjectManager<Notification>
    {

    }

    /// <summary>
    /// 通知管理实现类。
    /// </summary>
    public class NotificationManager : ObjectManager<Notification>, INotificationManager
    {
        /// <summary>
        /// 初始化类<see cref="NotificationManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        public NotificationManager(IDbContext<Notification> context) : base(context)
        {
        }
    }
}
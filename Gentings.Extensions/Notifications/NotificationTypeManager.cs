using Gentings.Data;
using Gentings.Extensions.Categories;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Extensions.Notifications
{
    /// <summary>
    /// 系统通知类型管理类型。
    /// </summary>
    public class NotificationTypeManager : CachableCategoryManager<NotificationType>, INotificationTypeManager
    {
        /// <summary>
        /// 初始化类<see cref="NotificationTypeManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        public NotificationTypeManager(IDbContext<NotificationType> context, IMemoryCache cache) : base(context, cache)
        {
        }
    }
}
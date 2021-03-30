using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gentings.Security.Notifications
{
    /// <summary>
    /// 通知集合。
    /// </summary>
    public class NotificationCollection : IEnumerable<Notification>
    {
        private readonly IEnumerable<Notification> _notifications;

        internal NotificationCollection(IEnumerable<Notification> notifications)
        {
            _notifications = notifications;
            News = notifications.Count(x => x.Status == NotificationStatus.New);
        }

        /// <summary>
        /// 新通知。
        /// </summary>
        public int News { get; }

        /// <summary>
        /// 获取通知迭代器。
        /// </summary>
        /// <returns>通知迭代器。</returns>
        public IEnumerator<Notification> GetEnumerator() => _notifications.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
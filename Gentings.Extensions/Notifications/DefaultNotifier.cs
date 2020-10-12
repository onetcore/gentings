namespace Gentings.Extensions.Notifications
{
    internal class DefaultNotifier : Notifier
    {
        /// <summary>
        /// 初始化类<see cref="DefaultNotifier"/>。
        /// </summary>
        /// <param name="notificationManager">通知管理接口。</param>
        /// <param name="typeManager">通知类型管理接口。</param>
        public DefaultNotifier(INotificationManager notificationManager, INotificationTypeManager typeManager) : base(notificationManager, typeManager)
        {
        }
    }
}
using Gentings.Data.Migrations;

namespace Gentings.Security.Notifications
{
    /// <summary>
    /// 扩展服务类。
    /// </summary>
    public static class ServiceExtensions
    {
        private class DefaultNotificationDataMigration : NotificationDataMigration
        {
        }

        /// <summary>
        /// 添加用户通知组件。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddNotification(this IServiceBuilder builder)
        {
            return builder.AddTransients<IDataMigration, DefaultNotificationDataMigration>()
                 .AddTransients<INotificationManager, NotificationManager>();
        }
    }
}
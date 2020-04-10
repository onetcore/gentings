using Gentings.Data.Migrations;
using Gentings.Messages.Emails;
using Gentings.Messages.Notifications;
using Gentings.Messages.SMS;
using Gentings.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gentings.Messages
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加短信服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回构建实例。</returns>
        public static IServiceBuilder AddSMS(this IServiceBuilder builder)
        {
            return builder.AddServices(services =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<ITaskService, DefaultSmsTaskService>());
                services.TryAddEnumerable(ServiceDescriptor.Transient<IDataMigration, DefaultSmsDataMigration>());
            });
        }

        /// <summary>
        /// 添加通知服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回构建实例。</returns>
        public static IServiceBuilder AddNotification(this IServiceBuilder builder)
        {
            return builder.AddServices(services =>
            {
                services.AddSingleton<INotificationManager, DefaultNotificationManager>();
                services.AddSingleton<INotifier, DefaultNotifier>();
                services.TryAddEnumerable(ServiceDescriptor.Singleton<ITaskService, DefaultNotificationTaskService>());
                services.TryAddEnumerable(ServiceDescriptor.Transient<IDataMigration, DefaultNotificationDataMigration>());
            });
        }

        /// <summary>
        /// 添加电子邮件服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回构建实例。</returns>
        public static IServiceBuilder AddEmail(this IServiceBuilder builder)
        {
            return builder.AddServices(services =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<ITaskService, DefaultEmailTaskService>());
                services.TryAddEnumerable(ServiceDescriptor.Transient<IDataMigration, DefaultEmailDataMigration>());
            });
        }
    }
}

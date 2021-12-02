using Gentings.Data.Migrations;
using Gentings.Tasks;
using Microsoft.Extensions.Logging;

namespace Gentings.Extensions.Emails
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        internal class DefaultEmailDataMigration : EmailDataMigration { }

        internal class DefaultEmailTaskService : EmailTaskService
        {
            public DefaultEmailTaskService(IEmailSettingsManager settingsManager, IEmailManager emailManager, ILogger<EmailTaskService> logger)
                : base(settingsManager, emailManager, logger)
            {
            }
        }

        /// <summary>
        /// 添加邮件组件。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddEmails(this IServiceBuilder builder)
        {
            return builder.AddTransients<IDataMigration, DefaultEmailDataMigration>()
                .AddSingleton<IEmailManager, EmailManager>()
                .AddSingleton<IEmailSettingsManager, EmailSettingsManager>()
                .AddSingletons<ITaskService, DefaultEmailTaskService>();
        }

    }
}
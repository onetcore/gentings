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
        private class DefaultEmailDataMigration : EmailDataMigration
        {

        }

        private class DefaultEmailTaskService : EmailTaskService
        {
            /// <summary>
            /// 初始化类<see cref="EmailTaskService"/>。
            /// </summary>
            /// <param name="settingsManager">配置管理接口。</param>
            /// <param name="emailManager">电子邮件管理接口。</param>
            /// <param name="logger">日志接口。</param>
            public DefaultEmailTaskService(IEmailSettingsManager settingsManager, IEmailManager emailManager, ILogger<EmailTaskService> logger) : base(settingsManager, emailManager, logger)
            {
            }
        }

        /// <summary>
        /// 添加Email服务。
        /// </summary>
        /// <param name="builder">服务构建接口。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddEmails(this IServiceBuilder builder)
        {
            return builder.AddTransients<IDataMigration, DefaultEmailDataMigration>()
                 .AddSingletons<ITaskService, DefaultEmailTaskService>()
                 .AddSingleton<IEmailManager, EmailManager>()
                 .AddSingleton<IEmailSettingsManager, EmailSettingsManager>();
        }
    }
}
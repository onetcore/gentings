using Gentings.Data.Migrations;
using Gentings.Extensions.Captchas;
using Gentings.Tasks;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加SMS服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddSMS(this IServiceBuilder builder)
        {
            return builder.AddTransients<IDataMigration, DefaultSmsDataMigration>()
                .AddSingletons<ITaskService, DefaultSmsTaskService>()
                .AddSingleton<ISmsManager, SmsManager>()
                .AddSingleton<ISmsSettingManager, SmsSettingManager>();
        }

        private class DefaultCaptchaDataMigration : CaptchaDataMigration
        {

        }

        private class DefaultSmsDataMigration : SmsDataMigration
        {

        }

        private class DefaultSmsTaskService : SmsTaskService
        {
            /// <summary>
            /// 初始化类<see cref="DefaultSmsTaskService"/>。
            /// </summary>
            /// <param name="smsManager">短信管理接口。</param>
            public DefaultSmsTaskService(ISmsManager smsManager) : base(smsManager)
            {
            }
        }
    }
}
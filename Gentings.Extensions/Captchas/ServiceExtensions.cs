using Gentings.Data.Migrations;
using Gentings.Tasks;

namespace Gentings.Extensions.Captchas
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加短信验证服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddCaptchas(this IServiceBuilder builder)
        {
            return builder.AddTransients<IDataMigration, DefaultCaptchaDataMigration>()
                   .AddSingleton<ICaptchaManager, CaptchaManager>()
                   .AddSingletons<ITaskService, DefaultClearTaskService>();
        }

        private class DefaultCaptchaDataMigration : CaptchaDataMigration
        {

        }

        private class DefaultClearTaskService : ClearTaskService
        {
            public DefaultClearTaskService(ICaptchaManager captchaManager) : base(captchaManager)
            {
            }
        }
    }
}
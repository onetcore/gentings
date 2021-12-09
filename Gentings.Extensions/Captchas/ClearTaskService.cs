using System;
using System.Threading.Tasks;
using Gentings.Extensions.Properties;
using Gentings.Tasks;

namespace Gentings.Extensions.Captchas
{
    /// <summary>
    /// 短信发送后台服务。
    /// </summary>
    public abstract class ClearTaskService : TaskService
    {
        private readonly ICaptchaManager _captchaManager;
        /// <summary>
        /// 初始化类<see cref="ClearTaskService"/>。
        /// </summary>
        /// <param name="captchaManager">短信验证管理接口。</param>
        public ClearTaskService(ICaptchaManager captchaManager)
        {
            _captchaManager = captchaManager;
        }

        /// <summary>
        /// 名称。
        /// </summary>
        public override string Name => Resources.CaptchaTaskService;

        /// <summary>
        /// 描述。
        /// </summary>
        public override string Description => Resources.CaptchaTaskService_Description;

        /// <summary>
        /// 执行间隔时间。
        /// </summary>
        public override TaskInterval Interval => new TimeSpan(2, 30, 0);

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="argument">参数。</param>
        public override async Task ExecuteAsync(Argument argument)
        {
            await _captchaManager.ClearAsync();
        }
    }
}
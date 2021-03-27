using System.Threading.Tasks;
using Gentings.Extensions.SMS.Properties;
using Gentings.Tasks;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 短信发送后台服务。
    /// </summary>
    public abstract class SmsTaskService : TaskService
    {
        private readonly ISmsManager _smsManager;
        /// <summary>
        /// 初始化类<see cref="SmsTaskService"/>。
        /// </summary>
        /// <param name="smsManager">短信管理接口。</param>
        public SmsTaskService(ISmsManager smsManager)
        {
            _smsManager = smsManager;
        }

        /// <summary>
        /// 名称。
        /// </summary>
        public override string Name => Resources.SMSTaskService;

        /// <summary>
        /// 描述。
        /// </summary>
        public override string Description => Resources.SMSTaskService_Description;

        /// <summary>
        /// 执行间隔时间。
        /// </summary>
        public override TaskInterval Interval => 30;

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="argument">参数。</param>
        public override async Task ExecuteAsync(Argument argument)
        {
            var messages = await _smsManager.LoadAsync(SmsSettings.BatchSize);
            foreach (var message in messages)
            {
                await _smsManager.SendAsync(message);
            }
        }
    }
}
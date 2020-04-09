﻿using Gentings.Messages.Properties;
using Gentings.Tasks;
using System.Threading.Tasks;

namespace Gentings.Messages.SMS
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
            var notes = await _smsManager.LoadAsync(SmsSettings.BatchSize);
            foreach (var note in notes)
            {
                await _smsManager.SendAsync(note);
            }
        }
    }
}
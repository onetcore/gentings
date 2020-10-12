using System;
using System.Linq;
using System.Threading.Tasks;
using Gentings.Extensions.Properties;
using Gentings.Tasks;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Gentings.Extensions.Emails
{
    /// <summary>
    /// 邮件发送服务。
    /// </summary>
    public abstract class EmailTaskService : TaskService
    {
        private readonly IEmailSettingsManager _settingsManager;
        private readonly IEmailManager _emailManager;
        private readonly ILogger<EmailTaskService> _logger;
        /// <summary>
        /// 初始化类<see cref="EmailTaskService"/>。
        /// </summary>
        /// <param name="settingsManager">配置管理接口。</param>
        /// <param name="emailManager">电子邮件管理接口。</param>
        /// <param name="logger">日志接口。</param>
        public EmailTaskService(IEmailSettingsManager settingsManager, IEmailManager emailManager, ILogger<EmailTaskService> logger)
        {
            _settingsManager = settingsManager;
            _emailManager = emailManager;
            _logger = logger;
        }

        /// <summary>
        /// 名称。
        /// </summary>
        public override string Name => Resources.EmailTaskService;

        /// <summary>
        /// 描述。
        /// </summary>
        public override string Description => Resources.EmailTaskService_Description;

        /// <summary>
        /// 执行间隔时间。
        /// </summary>
        public override TaskInterval Interval => 60;

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="argument">参数。</param>
        public override async Task ExecuteAsync(Argument argument)
        {
            var settings = await _settingsManager.GetSettingsAsync();
            if (settings == null)
            {
                return;
            }

            var messages = await _emailManager.LoadAsync(EmailStatus.Pending);
            if (!messages.Any())
            {
                return;
            }

            foreach (var message in messages)
            {
                try
                {
                    await SendAsync(settings, message);
                    await _emailManager.SetSuccessAsync(message.Id, settings.Id);
                }
                catch (Exception exception)
                {
                    await _emailManager.SetFailuredAsync(message.Id, EmailSettings.MaxTryTimes);
                    _logger.LogError(exception, "发送邮件错误");
                }
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 发送电子邮件。
        /// </summary>
        /// <param name="settings">网站配置。</param>
        /// <param name="message">电子邮件实例。</param>
        /// <returns>返回发送任务。</returns>
        protected virtual async Task SendAsync(EmailSettings settings, Email message)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(settings.SmtpServer, settings.SmtpPort, settings.UseSsl);
            await client.AuthenticateAsync(settings.SmtpUserName, settings.SmtpPassword);

            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(settings.SmtpUserName));
            mail.To.Add(new MailboxAddress(message.To));
            mail.Subject = message.Title;
            var html = new TextPart("Html") { Text = message.Content };

            var multipart = await InitAsync(mail, message);
            if (multipart != null)
            {
                multipart.Insert(0, html);
                mail.Body = multipart;
            }
            else
            {
                mail.Body = html;
            }

            await client.SendAsync(mail);
        }

        /// <summary>
        /// 实例化一个电子邮件。
        /// </summary>
        /// <param name="mail">邮件实例。</param>
        /// <param name="message">电子邮件实例。</param>
        /// <returns>返回邮件实体对象。</returns>
        protected virtual Task<Multipart> InitAsync(MimeMessage mail, Email message) => Task.FromResult(default(Multipart));
    }
}
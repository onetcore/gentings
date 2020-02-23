using Microsoft.Extensions.Logging;

namespace Gentings.Messages.Emails
{
    internal class DefaultEmailTaskService : EmailTaskService
    {
        public DefaultEmailTaskService(IEmailSettingsManager settingsManager, IEmailManager emailManager, ILogger<EmailTaskService> logger) 
            : base(settingsManager, emailManager, logger)
        {
        }
    }
}
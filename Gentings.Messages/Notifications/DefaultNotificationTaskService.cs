using Gentings.Data;
using Gentings.Extensions.Settings;

namespace Gentings.Messages.Notifications
{
    internal class DefaultNotificationTaskService : NotificationTaskService
    {
        public DefaultNotificationTaskService(IDbContext<Notification> context, ISettingsManager settingsManager) 
            : base(context, settingsManager)
        {
        }
    }
}
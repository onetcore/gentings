namespace Gentings.Messages.SMS
{
    internal class DefaultSmsTaskService : SmsTaskService
    {
        public DefaultSmsTaskService(ISmsManager smsManager) : base(smsManager)
        {
        }
    }
}
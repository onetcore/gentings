namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 短信状态。
    /// </summary>
    public enum SmsStatus
    {
        /// <summary>
        /// 等待发送。
        /// </summary>
        Pending,

        /// <summary>
        /// 发送中，等待下发状态。
        /// </summary>
        Sending,

        /// <summary>
        /// 发送成功。
        /// </summary>
        Completed,

        /// <summary>
        /// 发送失败。
        /// </summary>
        Failured,
    }
}
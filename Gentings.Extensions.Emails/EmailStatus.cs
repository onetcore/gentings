namespace Gentings.Extensions.Emails
{
    /// <summary>
    /// 电子邮件状态。
    /// </summary>
    public enum EmailStatus
    {
        /// <summary>
        /// 等待发送。
        /// </summary>
        Pending,

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
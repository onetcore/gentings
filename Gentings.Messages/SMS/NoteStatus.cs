namespace Gentings.Messages.SMS
{
    /// <summary>
    /// 短信状态。
    /// </summary>
    public enum NoteStatus
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
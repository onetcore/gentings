namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// 是否要求返回状态确认报告。
    /// </summary>
    public enum RegisteredDelivery : byte
    {
        /// <summary>
        /// 不需要。
        /// </summary>
        No,
        /// <summary>
        /// 需要。
        /// </summary>
        Yes,
    }
}

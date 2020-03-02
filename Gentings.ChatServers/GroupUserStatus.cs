namespace Gentings.ChatServers
{
    /// <summary>
    /// 群状态。
    /// </summary>
    public enum GroupUserStatus
    {
        /// <summary>
        /// 等待验证。
        /// </summary>
        Pending,
        /// <summary>
        /// 正常。
        /// </summary>
        Normal,
        /// <summary>
        /// 黑名单。
        /// </summary>
        BlackList,
    }
}
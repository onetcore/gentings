namespace Gentings.AspNetCore.OpenServices
{
    /// <summary>
    /// 应用状态。
    /// </summary>
    public enum ApplicationStatus
    {
        /// <summary>
        /// 正常。
        /// </summary>
        Normal,

        /// <summary>
        /// 过期。
        /// </summary>
        Expired,

        /// <summary>
        /// 禁用。
        /// </summary>
        Disabled,

        /// <summary>
        /// 等待验证。
        /// </summary>
        Pending,

        /// <summary>
        /// 验证失败。
        /// </summary>
        Unapproved,
    }
}
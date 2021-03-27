using System;

namespace Gentings.Identity
{
    /// <summary>
    /// 用户模块。
    /// </summary>
    [Flags]
    public enum IdentityMode
    {
        /// <summary>
        /// 无。
        /// </summary>
        None = 0,
        /// <summary>
        /// 权限验证。
        /// </summary>
        PermissionAuthorization = 1,
        /// <summary>
        /// 用户通知。
        /// </summary>
        Notification = 2,
    }
}
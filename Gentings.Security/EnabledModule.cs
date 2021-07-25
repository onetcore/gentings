using System;

namespace Gentings.Security
{
    /// <summary>
    /// 用户模块。
    /// </summary>
    [Flags]
    public enum EnabledModule
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
        /// <summary>
        /// Cookie验证。
        /// </summary>
        Cookies = 4,
    }
}
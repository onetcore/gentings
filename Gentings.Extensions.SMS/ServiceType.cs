using System;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 运营商。
    /// </summary>
    [Flags]
    public enum ServiceType
    {
        /// <summary>
        /// 未知
        /// </summary>
        None = 0,
        /// <summary>
        /// 移动网关
        /// </summary>
        Mobile = 1,
        /// <summary>
        /// 电信网关
        /// </summary>
        Telecom = 2,
        /// <summary>
        /// 联通网关
        /// </summary>
        Unicom = 4,
        /// <summary>
        /// 三网通。
        /// </summary>
        All = Mobile | Telecom | Unicom,
    }
}
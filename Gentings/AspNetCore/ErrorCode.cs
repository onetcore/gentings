﻿namespace Gentings.AspNetCore
{
    /// <summary>
    /// 错误代码。
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 验证错误。
        /// </summary>
        ValidError = -3,
        /// <summary>
        /// 参数错误。
        /// </summary>
        InvalidParameters = -2,
        /// <summary>
        /// 未知错误。
        /// </summary>
        UnknownError = -1,
    }
}
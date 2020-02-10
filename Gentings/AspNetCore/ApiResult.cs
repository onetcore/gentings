﻿using System;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// API结果。
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 成功实例。
        /// </summary>
        public static readonly ApiResult Success = new ApiResult();

        /// <summary>
        /// 状态：成功true/失败false。
        /// </summary>
        public bool Status => Code == null || (int)(object)Code == 0;

        /// <summary>
        /// 设置错误编码。
        /// </summary>
        public Enum Code { private get; set; }

        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; set; }
    }
}
using System;

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
        /// 状态，就ok和error两个字符串。
        /// </summary>
        public string Status => Code == null || (int)(object)Code == 0 ? "ok" : "error";

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
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
        /// 状态：成功true/失败false。
        /// </summary>
        public bool Status => Code == 0;

        /// <summary>
        /// 设置错误编码。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 隐式转换为返回结果。
        /// </summary>
        /// <param name="code">当前错误代码。</param>
        public static implicit operator ApiResult(Enum code)
        {
            return new ApiResult { Code = (int)(object)code };
        }

        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; set; }
    }
}

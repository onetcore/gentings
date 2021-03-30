using Gentings.Properties;

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
        public static readonly ApiResult Success = new ApiResult { Message = Resources.ErrorCode_Success };

        /// <summary>
        /// 状态：成功true/失败false。
        /// </summary>
        public bool Status => Code == 0;

        /// <summary>
        /// 设置错误编码。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; set; }
    }
}

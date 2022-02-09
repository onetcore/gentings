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
        public static readonly ApiResult Success = new() { Message = Resources.ErrorCode_Success };

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

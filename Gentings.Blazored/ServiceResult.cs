namespace Gentings.Blazored
{
    /// <summary>
    /// API结果。
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// 成功实例。
        /// </summary>
        public static readonly ServiceResult Success = new ServiceResult();

        /// <summary>
        /// 状态：成功true/失败false。
        /// </summary>
        public bool Status { get; set; }

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
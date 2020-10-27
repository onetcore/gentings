namespace Gentings.RabbitMQ
{
    /// <summary>
    /// 错误码。
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 成功。
        /// </summary>
        Success,
        /// <summary>
        /// 发送实体不能为空。
        /// </summary>
        NullBody,
        /// <summary>
        /// 失败。
        /// </summary>
        Failure,
    }
}
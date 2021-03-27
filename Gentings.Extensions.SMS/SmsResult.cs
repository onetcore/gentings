namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// SMS发送返回的结果。
    /// </summary>
    public class SmsResult
    {
        /// <summary>
        /// 失败。
        /// </summary>
        public static readonly SmsResult Failured = new SmsResult { Status = SmsStatus.Failured };

        /// <summary>
        /// 成功。
        /// </summary>
        public static readonly SmsResult Succeed = new SmsResult { Status = SmsStatus.Completed };

        /// <summary>
        /// 编码。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回短信Id。
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        public SmsStatus Status { get; set; }

        /// <summary>
        /// 隐士转换为布尔类型。
        /// </summary>
        /// <param name="result">当前值。</param>
        public static implicit operator bool(SmsResult result) => result.Status == SmsStatus.Completed;

        /// <summary>
        /// 隐士转换为布尔类型。
        /// </summary>
        /// <param name="result">当前值。</param>
        public static implicit operator SmsResult(bool result) => result ? Succeed : Failured;
    }
}
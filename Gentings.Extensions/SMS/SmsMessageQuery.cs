namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 短信查询。
    /// </summary>
    public class SmsMessageQuery : QueryBase<SmsMessage>
    {
        /// <summary>
        /// 状态。
        /// </summary>
        public SmsStatus? Status { get; set; }

        /// <summary>
        /// 电话号码。
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<SmsMessage> context)
        {
            context.WithNolock();
            if (Status != null)
            {
                context.Where(x => x.Status == Status);
            }

            if (!string.IsNullOrEmpty(No))
            {
                context.Where(x => x.PhoneNumber == No);
            }
        }
    }
}
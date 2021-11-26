namespace Gentings.Security.Notifications
{
    /// <summary>
    /// 通知查询实例。
    /// </summary>
    public class NotificationQuery : QueryBase<Notification>
    {
        /// <summary>
        /// 接收通知用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 发送通知用户。
        /// </summary>
        public int SendId { get; set; }

        /// <summary>
        /// 通知状态。
        /// </summary>
        public NotificationStatus? Status { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<Notification> context)
        {
            context.WithNolock()
                .Exclude(x => x.ExtendProperties)
                .OrderByDescending(x => x.Id);
            if (UserId > 0)
                context.Where(x => x.UserId == UserId);
            if (SendId > 0)
                context.Where(x => x.SendId == SendId);
            if (Status != null)
                context.Where(x => x.Status == Status);
            if (!string.IsNullOrEmpty(Title))
                context.Where(x => x.Title.Contains(Title));
        }
    }
}
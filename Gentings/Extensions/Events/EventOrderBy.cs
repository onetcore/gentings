namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件查询排序列表。
    /// </summary>
    public enum EventOrderBy
    {
        /// <summary>
        /// 唯一Id。
        /// </summary>
        Id,
        /// <summary>
        /// 事件类型Id。
        /// </summary>
        EventId,
        /// <summary>
        /// 用户Id。
        /// </summary>
        UserId,
        /// <summary>
        /// 记录时间。
        /// </summary>
        CreatedDate,
        /// <summary>
        /// 来源。
        /// </summary>
        Source,
        /// <summary>
        /// 消息等级。
        /// </summary>
        Level,
    }
}
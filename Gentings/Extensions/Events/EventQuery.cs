using System;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件查询实例。
    /// </summary>
    public class EventQuery : QueryBase<Event>, IOrderBy
    {
        /// <summary>
        /// 事件类型Id。
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 当前用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// IP地址。
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 来源。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 是否降序。
        /// </summary>
        public bool Desc { get; set; } = true;

        /// <summary>
        /// 排序列枚举。
        /// </summary>
        Enum IOrderBy.Order => Order;

        /// <summary>
        /// 排序。
        /// </summary>
        public EventOrderBy Order { get; set; }

        /// <summary>
        /// 起始时间。
        /// </summary>
        public DateTimeOffset? Start { get; set; }

        /// <summary>
        /// 结束时间。
        /// </summary>
        public DateTimeOffset? End { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected internal override void Init(IQueryContext<Event> context)
        {
            base.Init(context);
            if (EventId > 0)
                context.Where(x => x.EventId == EventId);
            if (!string.IsNullOrEmpty(IP))
                context.Where(x => x.IPAdress == IP);
            if (!string.IsNullOrEmpty(Source))
                context.Where(x => x.Source.Contains(Source));
            if (Start != null)
                context.Where(x => x.CreatedDate >= Start);
            if (End != null)
                context.Where(x => x.CreatedDate <= End);
            InitUsers(context);
        }

        /// <summary>
        /// 初始化用户条件。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected virtual void InitUsers(IQueryContext<Event> context)
        {
            if (UserId > 0)
                context.Where(x => x.UserId == UserId);
        }
    }
}
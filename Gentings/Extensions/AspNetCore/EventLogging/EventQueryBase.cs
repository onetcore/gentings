using System;
using Gentings.Data;

namespace Gentings.Extensions.AspNetCore.EventLogging
{
    /// <summary>
    /// 事件查询实例。
    /// </summary>
    public abstract class EventQueryBase<TUser> : QueryBase<EventMessage>
        where TUser : IUser
    {
        /// <summary>
        /// 事件类型Id。
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IP地址。
        /// </summary>
        public string IP { get; set; }

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
        protected internal override void Init(IQueryContext<EventMessage> context)
        {
            context.WithNolock().Select();
            context.InnerJoin<TUser>((a, u) => a.UserId == u.Id)
                .Select<TUser>(x => new { x.UserName, x.NickName, x.Avatar });

            if (EventId > 0)
            {
                context.Where(x => x.EventId == EventId);
            }

            if (UserId > 0)
            {
                context.Where(x => x.UserId == UserId);
            }

            if (Start != null)
            {
                context.Where(x => x.CreatedDate >= Start);
            }

            if (End != null)
            {
                context.Where(x => x.CreatedDate <= End);
            }

            if (!string.IsNullOrEmpty(IP))
            {
                context.Where(x => x.IPAdress == IP);
            }

            if (!string.IsNullOrEmpty(Name))
            {
                context.Where<TUser>(x => x.UserName.Contains(Name) || x.NickName.Contains(Name));
            }

            context.OrderByDescending(x => x.Id);
        }
    }
}
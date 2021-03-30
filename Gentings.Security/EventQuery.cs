using Gentings.Data;
using Gentings.Extensions.Events;

namespace Gentings.Security
{
    /// <summary>
    /// 事件查询实例。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    public class EventQuery<TUser> : EventQuery
        where TUser : UserBase
    {
        /// <summary>
        /// 是否获取子用户的。
        /// </summary>
        public bool IsChildren { get; set; }

        /// <summary>
        /// 初始化用户条件。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void InitUsers(IQueryContext<Event> context)
        {
            if (UserId > 0)
            {
                if (IsChildren)
                    context.InnerJoin<IndexedUser>((e, u) => e.UserId == u.Id)
                        .Where<IndexedUser>(x => x.ParentId == UserId);
                else
                    context.Where(x => x.UserId == UserId);
            }
        }
    }
}
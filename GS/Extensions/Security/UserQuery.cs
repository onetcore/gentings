using Gentings.Data;
using System;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 用户查询实例。
    /// </summary>
    public class UserQuery : QueryBase<User>, IOrderBy
    {
        /// <summary>
        /// 用户名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否降序。
        /// </summary>
        public bool Desc { get; set; }

        /// <summary>
        /// 排序列枚举。
        /// </summary>
        public UserOrderBy Order { get; set; }
        Enum IOrderBy.Order => Order;

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<User> context)
        {
            base.Init(context);
            if (!string.IsNullOrEmpty(Name))
                context.Where(x => x.UserName.Contains(Name) || x.NickName.Contains(Name));
        }
    }
}

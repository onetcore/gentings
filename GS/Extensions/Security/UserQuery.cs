using Gentings.Extensions;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 用户查询实例。
    /// </summary>
    public class UserQuery : OrderableQueryBase<User, UserOrderBy>
    {
        /// <summary>
        /// 用户名称。
        /// </summary>
        public string Name { get; set; }

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

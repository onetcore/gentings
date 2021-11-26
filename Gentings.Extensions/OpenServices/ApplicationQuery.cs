using System;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 应用查询实例。
    /// </summary>
    public abstract class ApplicationQuery<TUser> : QueryBase<Application>
        where TUser : IUser
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用Id。
        /// </summary>
        public Guid? AppId { get; set; }

        /// <summary>
        /// 用户名称。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        public ApplicationStatus? Status { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<Application> context)
        {
            context.WithNolock()
                .InnerJoin<TUser>((a, u) => a.UserId == u.Id)
                .Select()//备注，这个需要放在用户之前，否正可能会被覆盖
                .Select<TUser>(x => new { x.UserName, x.NickName });
            if (!string.IsNullOrWhiteSpace(Name))
                context.Where(x => x.Name.Contains(Name));
            if (!string.IsNullOrWhiteSpace(UserName))
                context.Where<TUser>(x => x.UserName.Contains(UserName) || x.NickName.Contains(UserName));
            if (UserId > 0)
                context.Where(x => x.UserId == UserId);
            if (AppId != null && AppId != Guid.Empty)
                context.Where(x => x.Id == AppId);
            if (Status != null)
                context.Where(x => x.Status == Status);
            context.OrderByDescending(x => x.CreatedDate);
        }
    }
}
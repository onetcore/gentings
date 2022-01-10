using Gentings.Data;
using Gentings.Security;
using System;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 应用查询实例。
    /// </summary>
    public class ApplicationQuery : QueryBase<Application>
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
            base.Init(context);
            if (!string.IsNullOrWhiteSpace(Name))
                context.Where(x => x.Name.Contains(Name));
            if (UserId > 0)
                context.Where(x => x.UserId == UserId);
            if (AppId != null && AppId != Guid.Empty)
                context.Where(x => x.Id == AppId);
            if (Status != null)
                context.Where(x => x.Status == Status);
            context.OrderByDescending(x => x.CreatedDate);
        }

        /// <summary>
        /// 用户名称。
        /// </summary>
        public string UserName { get; set; }
    }

    /// <summary>
    /// 应用查询实例。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    public class ApplicationQuery<TUser> : ApplicationQuery where TUser : class, IUser
    {
        /// <summary>
        /// 初始化类<see cref="ApplicationQuery{TUser}"/>。
        /// </summary>
        public ApplicationQuery() { }

        /// <summary>
        /// 初始化类<see cref="ApplicationQuery{TUser}"/>。
        /// </summary>
        /// <param name="query">应用程序查询实例对象。</param>
        public ApplicationQuery(ApplicationQuery query)
        {
            PageIndex = query.PageIndex;
            PageSize = query.PageSize;
            Name = query.Name;
            AppId = query.AppId;
            UserId = query.UserId;
            Status = query.Status;
            UserName = query.UserName;
        }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<Application> context)
        {
            base.Init(context);
            context.JoinSelect<Application, TUser>((a, u) => a.UserId == u.Id, x => new { x.UserName, x.NickName });
            if (!string.IsNullOrWhiteSpace(UserName))
                context.Where<TUser>(x => x.UserName.Contains(UserName) || x.NickName.Contains(UserName));
        }
    }
}
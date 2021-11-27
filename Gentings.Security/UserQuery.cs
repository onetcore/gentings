using Gentings.Extensions;
using System;

namespace Gentings.Security
{
    /// <summary>
    /// 分页查询基类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    public abstract class UserQuery<TUser> : QueryBase<TUser>
        where TUser : UserBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 注册开始时间。
        /// </summary>
        public DateTime? Start { get; set; }

        /// <summary>
        /// 注册结束时间。
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// 登录开始时间。
        /// </summary>
        public DateTime? LoginStart { get; set; }

        /// <summary>
        /// 登录结束时间。
        /// </summary>
        public DateTime? LoginEnd { get; set; }

        /// <summary>
        /// 电话号码。
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 电子邮件。
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 父级Id。
        /// </summary>
        public int Pid { get; set; }

        /// <summary>
        /// 获取所有子账户的用户Id，设置此用户Id将获取当前Id下的所有级联子账户。
        /// </summary>
        public int Sid { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<TUser> context)
        {
            base.Init(context);
            if (!string.IsNullOrWhiteSpace(Name))
                context.Where(x => x.NickName.Contains(Name) || x.NormalizedUserName.Contains(Name));
            if (Start != null)
                context.Where(x => x.CreatedDate >= Start);
            if (End != null)
                context.Where(x => x.CreatedDate <= End);
            if (LoginStart != null)
                context.Where(x => x.LastLoginDate >= LoginStart);
            if (LoginEnd != null)
                context.Where(x => x.LastLoginDate <= LoginEnd);
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
                context.Where(x => x.PhoneNumber == PhoneNumber);
            if (!string.IsNullOrWhiteSpace(Email))
                context.Where(x => x.NormalizedEmail.Contains(Email));
            if (Pid > 0)
                context.Where(x => x.ParentId == Pid);
            if (Sid > 0)
                context.InnerJoin<UserIndex>((u, ui) => u.Id == ui.Id)
                    .Where<UserIndex>(x => x.ParentId == Sid);
        }
    }
}
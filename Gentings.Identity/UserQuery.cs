using System;
using System.Text.Json.Serialization;
using Gentings.Data;
using Gentings.Identity.Roles;

namespace Gentings.Identity
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
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<TUser> context)
        {
            context.WithNolock();
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
        }
    }

    /// <summary>
    /// 分页查询基类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    public abstract class UserQuery<TUser, TRole> : UserQuery<TUser>
        where TUser : UserBase
        where TRole : RoleBase
    {
        /// <summary>
        /// 当前用户角色等级。
        /// </summary>
        [JsonIgnore]
        public int MaxRoleLevel { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<TUser> context)
        {
            base.Init(context);
            if (MaxRoleLevel > 0)
            {
                context.Select()
                    .LeftJoin<TRole>((u, r) => u.RoleId == r.Id)
                    .Where<TRole>(x => x.RoleLevel <= MaxRoleLevel);
            }
        }
    }
}
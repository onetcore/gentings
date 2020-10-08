using Gentings.Data;
using Gentings.SaaS.Security.Roles;

namespace Gentings.SaaS.Security
{
    /// <summary>
    /// 分页查询基类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    public abstract class UserQuery<TUser> : Identity.UserQuery<TUser>
        where TUser : UserBase
    {

    }

    /// <summary>
    /// 分页查询基类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    public abstract class UserQuery<TUser, TRole> : Identity.UserQuery<TUser, TRole>
        where TUser : UserBase
        where TRole : RoleBase
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        public int Sid { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<TUser> context)
        {
            base.Init(context);
            if (Sid > 0 && MaxRoleLevel > 0)
                context.Where<TRole>(x => x.SiteId == Sid);
        }
    }
}
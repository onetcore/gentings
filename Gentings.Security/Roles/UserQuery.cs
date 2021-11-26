namespace Gentings.Security.Roles
{
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
                    .Where<TRole>(x => x.RoleLevel < MaxRoleLevel);
            }
        }
    }
}
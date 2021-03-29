using System;
using System.Linq.Expressions;
using Gentings.Data;
using Gentings.Security.Roles;

namespace Gentings.Security
{
    /// <summary>
    /// 用户列扩展类。
    /// </summary>
    public static class UserFieldExtensions
    {
        /// <summary>
        /// 选择用户相关联字段。
        /// </summary>
        /// <typeparam name="TModel">当前实例模型。</typeparam>
        /// <typeparam name="TUser">用户类型。</typeparam>
        /// <typeparam name="TRole">角色类型。</typeparam>
        /// <param name="queryable">查询实例。</param>
        /// <param name="expression">关联表达式。</param>
        /// <returns>返回当前查询实例。</returns>
        public static IQueryable<TModel> JoinSelect<TModel, TUser, TRole>(this IQueryable<TModel> queryable,
            Expression<Func<TModel, TUser, bool>> expression)
            where TModel : UserFieldBase
            where TUser : UserBase
            where TRole : RoleBase
            => queryable
                .WithNolock()
                .InnerJoin<TUser>(expression)
                .InnerJoin<TUser, TRole>((u, r) => u.RoleId == r.Id)
                .Select<TUser>(x => new { x.NickName, x.UserName, x.RoleId, x.Avatar })
                .Select<TRole>(x => x.Color, "RoleColor")
                .Select<TRole>(x => x.Name, "RoleName")
                .Select<TRole>(x => x.IconUrl, "RoleIcon");
    }
}
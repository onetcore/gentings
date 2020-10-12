using Gentings.Data.Migrations;
using Gentings.Identity;
using Gentings.SaaS.Security.Roles;

namespace Gentings.SaaS.Security
{
    /// <summary>
    /// 数据库迁移。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TUserClaim">用户声明类型。</typeparam>
    /// <typeparam name="TUserLogin">用户登录类型。</typeparam>
    /// <typeparam name="TUserToken">用户标识类型。</typeparam>
    public abstract class SecurityDataMigration<TUser, TUserClaim, TUserLogin, TUserToken> : IdentityDataMigration<TUser, TUserClaim, TUserLogin, TUserToken>
        where TUser : UserBase, new()
        where TUserClaim : UserClaimBase, new()
        where TUserLogin : UserLoginBase, new()
        where TUserToken : UserTokenBase, new()
    {
        /// <summary>
        /// 建立索引。
        /// </summary>
        /// <param name="builder">数据迁移构建实例。</param>
        public override void Up1(MigrationBuilder builder)
        {
            base.Up1(builder);
            builder.AddColumn<TUser>(x => x.SiteId);
            builder.AddUniqueConstraint<TUser>(x => new { x.SiteId, x.NickName });
        }
    }

    /// <summary>
    /// 数据库迁移。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    /// <typeparam name="TUserClaim">用户声明类型。</typeparam>
    /// <typeparam name="TUserLogin">用户登录类型。</typeparam>
    /// <typeparam name="TUserRole">用户所在组类型。</typeparam>
    /// <typeparam name="TRoleClaim">角色声明类型。</typeparam>
    /// <typeparam name="TUserToken">用户标识类型。</typeparam>
    public abstract class SecurityDataMigration<TUser, TRole, TUserClaim, TRoleClaim, TUserLogin, TUserRole, TUserToken> :
        IdentityDataMigration<TUser, TRole, TUserClaim, TRoleClaim, TUserLogin, TUserRole, TUserToken>
        where TUser : UserBase, new()
        where TRole : RoleBase, new()
        where TUserClaim : UserClaimBase, new()
        where TRoleClaim : RoleClaimBase, new()
        where TUserRole : IUserRole, new()
        where TUserLogin : UserLoginBase, new()
        where TUserToken : UserTokenBase, new()
    {
        /// <summary>
        /// 建立索引。
        /// </summary>
        /// <param name="builder">数据迁移构建实例。</param>
        public override void Up1(MigrationBuilder builder)
        {
            base.Up1(builder);
            builder.AddColumn<TUser>(x => x.SiteId);
            builder.AddUniqueConstraint<TUser>(x => new { x.SiteId, x.NickName });
            builder.AddColumn<TRole>(x => x.SiteId);
            builder.AddUniqueConstraint<TRole>(x => new { x.SiteId, x.Name });
            builder.AddForeignKey<TRole, Site>(x => x.SiteId, x => x.Id, onDelete: ReferentialAction.Cascade);
        }
    }
}
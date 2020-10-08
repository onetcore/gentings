using Gentings.Data.Migrations;
using Gentings.Data.Migrations.Builders;
using Gentings.Identity.Roles;

namespace Gentings.Identity
{
    /// <summary>
    /// 数据库迁移。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TUserClaim">用户声明类型。</typeparam>
    /// <typeparam name="TUserLogin">用户登录类型。</typeparam>
    /// <typeparam name="TUserToken">用户标识类型。</typeparam>
    public abstract class IdentityDataMigration<TUser, TUserClaim, TUserLogin, TUserToken> : DataMigration
        where TUser : UserBase, new()
        where TUserClaim : UserClaimBase, new()
        where TUserLogin : UserLoginBase, new()
        where TUserToken : UserTokenBase, new()
    {
        /// <summary>
        /// 创建操作。
        /// </summary>
        /// <param name="builder">迁移构建实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            //用户
            builder.CreateTable<TUser>(table =>
            {
                table.Column(x => x.Id)
                    .Column(x => x.UserName, nullable: false)
                    .Column(x => x.NormalizedUserName, nullable: false)
                    .Column(x => x.PasswordHash, nullable: false)
                    .Column(x => x.NickName)
                    .Column(x => x.Email)
                    .Column(x => x.NormalizedEmail)
                    .Column(x => x.EmailConfirmed)
                    .Column(x => x.SecurityStamp)
                    .Column(x => x.PhoneNumber)
                    .Column(x => x.PhoneNumberConfirmed)
                    .Column(x => x.TwoFactorEnabled)
                    .Column(x => x.LockoutEnd)
                    .Column(x => x.LockoutEnabled)
                    .Column(x => x.AccessFailedCount)
                    .Column(x => x.ConcurrencyStamp)
                    .Column(x => x.CreatedIP)
                    .Column(x => x.CreatedDate)
                    .Column(x => x.UpdatedDate)
                    .Column(x => x.Avatar)
                    .Column(x => x.RoleId, defaultValue: 0)
                    .Column(x => x.LoginIP)
                    .Column(x => x.LastLoginDate)
                    .Column(x => x.ParentId);
                Create(table);
            });

            builder.CreateTable<IndexedUser>(table => table
                .Column(x => x.UserId)
                .Column(x => x.IndexedId)
                .ForeignKey<TUser>(x => x.UserId, x => x.Id, onDelete: ReferentialAction.Cascade));

            builder.CreateTable<TUserClaim>(table => table
                .Column(x => x.Id)
                .Column(x => x.ClaimType, nullable: false)
                .Column(x => x.ClaimValue)
                .Column(x => x.UserId)
                .ForeignKey<TUser>(x => x.UserId, x => x.Id, onDelete: ReferentialAction.Cascade));

            builder.CreateTable<TUserLogin>(table => table
                .Column(x => x.LoginProvider, nullable: false)
                .Column(x => x.ProviderKey, nullable: false)
                .Column(x => x.ProviderDisplayName)
                .Column(x => x.UserId)
                .ForeignKey<TUser>(x => x.UserId, x => x.Id, onDelete: ReferentialAction.Cascade));

            builder.CreateTable<TUserToken>(table => table
                .Column(x => x.LoginProvider, nullable: false)
                .Column(x => x.Name, nullable: false)
                .Column(x => x.Value)
                .Column(x => x.UserId)
                .ForeignKey<TUser>(x => x.UserId, x => x.Id, onDelete: ReferentialAction.Cascade));
        }

        /// <summary>
        /// 建索引。
        /// </summary>
        /// <param name="builder">构建实例。</param>
        protected virtual void CreateIndex(MigrationBuilder builder)
        {
            //索引。
            builder.CreateIndex<TUser>(x => x.NormalizedUserName, true);
            builder.CreateIndex<TUser>(x => x.NormalizedEmail);
            builder.CreateIndex<TUser>(x => x.RoleId);
        }

        /// <summary>
        /// 建立索引。
        /// </summary>
        /// <param name="builder">数据迁移构建实例。</param>
        public virtual void Up1(MigrationBuilder builder)
        {
            CreateIndex(builder);
        }

        /// <summary>
        /// 添加用户定义列。
        /// </summary>
        /// <param name="builder">用户表格定义实例。</param>
        protected virtual void Create(CreateTableBuilder<TUser> builder) { }
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
    public abstract class IdentityDataMigration<TUser, TRole, TUserClaim, TRoleClaim, TUserLogin, TUserRole, TUserToken> :
        IdentityDataMigration<TUser, TUserClaim, TUserLogin, TUserToken>
        where TUser : UserBase, new()
        where TRole : RoleBase, new()
        where TUserClaim : UserClaimBase, new()
        where TRoleClaim : RoleClaimBase, new()
        where TUserRole : IUserRole, new()
        where TUserLogin : UserLoginBase, new()
        where TUserToken : UserTokenBase, new()
    {
        /// <summary>
        /// 创建操作。
        /// </summary>
        /// <param name="builder">迁移构建实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            base.Create(builder);
            //角色
            builder.CreateTable<TRole>(table => table
                .Column(x => x.Id)
                .Column(x => x.Name, nullable: false)
                .Column(x => x.NormalizedName, nullable: false)
                .Column(x => x.IsSystem)
                .Column(x => x.ConcurrencyStamp)
                .Column(x => x.RoleLevel)
                .Column(x => x.Color)
                .Column(x => x.IconUrl)
                .Column(x => x.IsDefault));

            //判断TUserRole是否单独一个表格，也可以把这个表格合并到TUser中，每一个用户只是应对一个角色
            if (typeof(UserRoleBase).IsAssignableFrom(typeof(TUserRole)))
            {
                builder.CreateTable<TUserRole>(table => table
                    .Column(x => x.UserId)
                    .Column(x => x.RoleId)
                    .ForeignKey<TUser>(x => x.UserId, x => x.Id, onDelete: ReferentialAction.Cascade)
                    .ForeignKey<TRole>(x => x.RoleId, x => x.Id, onDelete: ReferentialAction.Cascade));
            }

            builder.CreateTable<TRoleClaim>(table => table
                .Column(x => x.Id)
                .Column(x => x.ClaimType, nullable: false)
                .Column(x => x.ClaimValue)
                .Column(x => x.RoleId)
                .ForeignKey<TRole>(x => x.RoleId, x => x.Id, onDelete: ReferentialAction.Cascade));
        }
    }
}
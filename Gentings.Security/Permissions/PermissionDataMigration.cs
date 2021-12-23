using Gentings.Data.Migrations;
using Gentings.Security.Roles;

namespace Gentings.Security.Permissions
{
    /// <summary>
    /// 数据库迁移类，设置为抽象类，防止自动注册无法识别<seealso cref="TRole"/>类型。
    /// </summary>
    /// <typeparam name="TRole">角色类型。</typeparam>
    public abstract class PermissionDataMigration<TRole> : DataMigration
        where TRole : RoleBase
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            //权限
            builder.CreateTable<Permission>(table => table
                .Column(x => x.Id)
                .Column(x => x.Category)
                .Column(x => x.Name)
                .Column(x => x.Text)
                .Column(x => x.Description)
                .Column(x => x.Order)
                .UniqueConstraint(x => new { x.Category, x.Name }));

            builder.CreateTable<PermissionInRole>(table => table
                .Column(x => x.PermissionId)
                .Column(x => x.RoleId)
                .Column(x => x.Value)
                .ForeignKey<Permission>(x => x.PermissionId, x => x.Id, onDelete: ReferentialAction.Cascade)
                .ForeignKey<TRole>(x => x.RoleId, x => x.Id, onDelete: ReferentialAction.Cascade));
        }
    }

    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    /// <typeparam name="TRole">角色类型。</typeparam>
    internal class DefaultPermissionDataMigration<TRole> : PermissionDataMigration<TRole>
        where TRole : RoleBase
    { }
}


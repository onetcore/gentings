using Gentings.Data.Migrations;

namespace Gentings.AspNetCore.OpenServices
{
    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    internal class ApplicationDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Application>(table => table
                .Column(x => x.Id)
                .Column(x => x.AppSecret)
                .Column(x => x.Name)
                .Column(x => x.Summary)
                .Column(x => x.CreatedDate)
                .Column(x => x.ExpiredDate)
                .Column(x => x.Status)
                .Column(x => x.UserId)
                .Column(x => x.ExtendProperties)
                .UniqueConstraint(x => new { x.UserId, x.Name })
            );
            builder.CreateTable<OpenService>(table => table
                .Column(x => x.Id)
                .Column(x => x.Route)
                .Column(x => x.HttpMethod)
                .Column(x => x.Category)
                .Column(x => x.Description)
                .Column(x => x.Disabled)
            );
            builder.CreateTable<ApplicationService>(table => table
                .Column(x => x.AppId)
                .Column(x => x.ServiceId)
                .ForeignKey<Application>(x => x.AppId, x => x.Id, onDelete: ReferentialAction.Cascade)
                .ForeignKey<OpenService>(x => x.ServiceId, x => x.Id, onDelete: ReferentialAction.Cascade)
            );
            builder.CreateIndex<OpenService>(x => new {x.HttpMethod, x.Route}, true);
        }
    }
}


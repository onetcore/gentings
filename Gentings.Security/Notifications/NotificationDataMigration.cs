namespace Gentings.Security.Notifications
{
    using Gentings.Data.Migrations;

    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public abstract class NotificationDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Notification>(table => table
                .Column(x => x.Id)
                .Column(x => x.UserId)
                .Column(x => x.SendId)
                .Column(x => x.CreatedDate)
                .Column(x => x.Status)
                .Column(x => x.Title)
                .Column(x => x.ExtendProperties)
            );
        }
    }
}


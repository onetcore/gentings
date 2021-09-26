using Gentings.Data.Migrations;

namespace Gentings.Extensions.Emails
{
    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class EmailDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Email>(table => table
                .Column(x => x.Id)
                .Column(x => x.SettingsId)
                .Column(x => x.UserId)
                .Column(x => x.To)
                .Column(x => x.Title)
                .Column(x => x.Content)
                .Column(x => x.Status)
                .Column(x => x.TryTimes)
                .Column(x => x.CreatedDate)
                .Column(x => x.ConfirmDate)
                .Column(x => x.HashKey)
                .Column(x => x.Result)
                .Column(x => x.ExtendProperties)
            );
            builder.CreateTable<EmailSettings>(table => table
                .Column(x => x.Id)
                .Column(x => x.Enabled)
                .Column(x => x.SmtpServer)
                .Column(x => x.SmtpUserName)
                .Column(x => x.SmtpPort)
                .Column(x => x.UseSsl)
                .Column(x => x.SmtpPassword)
                .Column(x => x.Count)
                .Column(x => x.Summary)
            );
            builder.CreateIndex<Email>(x => x.HashKey);
        }
    }
}
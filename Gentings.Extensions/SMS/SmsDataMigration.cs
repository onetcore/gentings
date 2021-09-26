using Gentings.Data.Migrations;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 短信数据库迁移类。
    /// </summary>
    public abstract class SmsDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<SmsMessage>(table => table
                .Column(x => x.Id)
                .Column(x => x.Client)
                .Column(x => x.Status)
                .Column(x => x.Message)
                .Column(x => x.Count)
                .Column(x => x.PhoneNumber)
                .Column(x => x.ServiceType)
                .Column(x => x.CreatedDate)
                .Column(x => x.HashKey)
                .Column(x => x.TryTimes)
                .Column(x => x.MsgId)
                .Column(x => x.SentDate)
                .Column(x => x.TemplateId)
                .Column(x => x.TemplateParameters)
                .Column(x => x.DeliveredDate)
                .Column(x => x.DeliveredStatus)
            );
            builder.CreateIndex<SmsMessage>(x => x.HashKey);
            builder.CreateIndex<SmsMessage>(x => new { x.MsgId, x.PhoneNumber });
            builder.CreateIndex<SmsMessage>(x => new { x.Status, x.SentDate });
            builder.CreateTable<SmsSettings>(table => table
                .Column(x => x.Id)
                .Column(x => x.Client)
                .Column(x => x.AppId)
                .Column(x => x.AppSecret)
                .Column(x => x.ExtendProperties)
                .UniqueConstraint(x => x.Client)
            );
        }
    }
}
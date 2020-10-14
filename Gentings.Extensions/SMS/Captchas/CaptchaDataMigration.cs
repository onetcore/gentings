using Gentings.Data.Migrations;

namespace Gentings.Extensions.SMS.Captchas
{
    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public abstract class CaptchaDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Captcha>(table => table
                .Column(x => x.Id)
                .Column(x => x.PhoneNumber)
                .Column(x => x.Type)
                .Column(x => x.Code)
                .Column(x => x.ExpiredDate)
            );
        }
    }
}


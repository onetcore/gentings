namespace Gentings.Extensions.SensitiveWords
{
    using Data.Migrations;

    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class SensitiveWordDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<SensitiveWord>(table => table
                .Column(x => x.Id)
                .Column(x => x.Word)
                .Column(x => x.CreatedDate)
                .UniqueConstraint(x=>x.Word)
            );
            builder.CreateIndex<SensitiveWord>(x => x.Word, true);
        }
    }
}


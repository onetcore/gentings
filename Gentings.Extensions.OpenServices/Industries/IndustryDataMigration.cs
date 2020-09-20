namespace Gentings.Extensions.OpenServices.Industries
{
    using Data.Migrations;

    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class IndustryDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Industry>(table => table
                .Column(x => x.Id)
                .Column(x => x.ParentId)
                .Column(x => x.Name)
                .Column(x => x.Kind)
                .Column(x => x.Group)
                .Column(x => x.Category)
                .Column(x => x.Type)
                .Column(x => x.Summary)
            );
        }
    }
}


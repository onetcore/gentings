using Gentings.Data.Migrations;

namespace Gentings.Data.Initializers
{
    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class InitializerDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Lisence>(table => table.Column(x => x.Registration));
        }
    }
}
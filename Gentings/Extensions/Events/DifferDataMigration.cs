using Gentings.Data.Migrations;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 对象属性更改日志记录数据迁移类。
    /// </summary>
    public abstract class DifferDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            // 对象实例修改表格
            builder.CreateTable<Differ>(table => table
                .Column(x => x.Id)
                .Column(x => x.Action)
                .Column(x => x.TypeName)
                .Column(x => x.PropertyName)
                .Column(x => x.Source)
                .Column(x => x.Value)
                .Column(x => x.UserId)
                .Column(x => x.CreatedDate)
            );
            // 索引
            builder.CreateIndex<Differ>(x => new { x.UserId, x.TypeName });
        }
    }
}
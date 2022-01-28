using Gentings.Data.Migrations;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class PageDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Page>(table => table
               .Column(x => x.Id)
               .Column(x => x.Key)
               .Column(x => x.Name)
               .Column(x => x.Disabled)
               .Column(x => x.DisplayMode)
               .Column(x => x.Title)
               .Column(x => x.MenuId)
               .Column(x => x.TemplateName)
               .Column(x => x.CreatedDate)
               .Column(x => x.UpdatedDate)
               .Column(x => x.ExtendProperties)
            );
            builder.CreateIndex<Page>(x => x.Key);
        }
    }
}


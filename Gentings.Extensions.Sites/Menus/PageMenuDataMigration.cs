using Gentings.Data.Migrations;
using Gentings.Data.Migrations.Builders;
using Gentings.Extensions.Categories;

namespace Gentings.Extensions.Sites.Menus
{
    /// <summary>
    /// 页面菜单数据库迁移类。
    /// </summary>
    public class PageMenuDataMigration : CategoryDataMigration<MenuCategory>
    {
        /// <summary>
        /// 编辑表格其他属性列。
        /// </summary>
        /// <param name="table">当前表格构建实例对象。</param>
        protected override void Create(CreateTableBuilder<MenuCategory> table)
        {
            base.Create(table);
            table.Column(x => x.DisplayName);
            table.Column(x => x.Description);
        }

        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            base.Create(builder);
            builder.CreateTable<PageMenu>(table => table
                .Column(x => x.Id)
                .Column(x => x.ParentId)
                .Column(x => x.Name)
                .Column(x => x.CategoryId)
                .Column(x => x.DisplayMode)
                .Column(x => x.LinkUrl)
                .Column(x => x.Rel)
                .Column(x => x.Target)
                .Column(x => x.FrameName)
                .Column(x => x.DisplayName)
                .Column(x => x.Order)
                .ForeignKey<MenuCategory>(x => x.CategoryId, x => x.Id, onDelete: ReferentialAction.Cascade)
            );
        }
    }
}
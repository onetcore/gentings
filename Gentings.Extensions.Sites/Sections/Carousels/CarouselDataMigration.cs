using Gentings.Data.Migrations;

namespace Gentings.Extensions.Sites.Sections.Carousels
{
    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class CarouselDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Carousel>(table => table
               .Column(x => x.Id)
               .Column(x => x.SectionId)
               .Column(x => x.Order)
               .Column(x => x.Title)
               .Column(x => x.Disabled)
               .Column(x => x.DisplayMode)
               .Column(x => x.CreatedDate)
               .Column(x => x.UpdatedDate)
               .Column(x => x.ExtendProperties)
               .ForeignKey<Section>(x => x.SectionId, x => x.Id, onDelete: ReferentialAction.Cascade)
            );
        }
    }
}


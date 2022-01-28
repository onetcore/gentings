﻿using Gentings.Data.Migrations;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class SectionDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Section>(table => table
               .Column(x => x.Id)
               .Column(x => x.Name)
               .Column(x => x.DisplayName)
               .Column(x => x.Disabled)
               .Column(x => x.DisplayMode)
               .Column(x => x.PageId)
               .Column(x => x.Order)
               .Column(x => x.IsPaged)
               .Column(x => x.CreatedDate)
               .Column(x => x.UpdatedDate)
               .Column(x => x.ExtendProperties)
            );
        }
    }
}

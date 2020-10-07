﻿namespace Gentings.SaaS
{
    using Data.Migrations;

    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class SiteDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<SiteAdapter>(table => table
                .Column(x => x.Id)
                .Column(x => x.SiteKey)
                .Column(x => x.SiteName)
                .Column(x => x.Description)
                .Column(x => x.Disabled)
                .Column(x => x.SettingValue)
                .UniqueConstraint(x => x.SiteKey)
            );

            builder.CreateTable<SiteDomain>(table => table
                .Column(x => x.SiteId)
                .Column(x => x.Domain)
                .ForeignKey<SiteAdapter>(x => x.SiteId, x => x.Id, onDelete: ReferentialAction.Cascade)
            );

            builder.CreateTable<SiteSettingsAdapter>(table => table
                .Column(x => x.SiteId)
                .Column(s => s.SettingKey)
                .Column(s => s.SettingValue)
                .ForeignKey<SiteAdapter>(x => x.SiteId, x => x.Id, onDelete: ReferentialAction.Cascade)
            );
        }
    }
}

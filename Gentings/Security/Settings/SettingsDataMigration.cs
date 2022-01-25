using Gentings.Data.Migrations;

namespace Gentings.Security.Settings
{
    /// <summary>
    /// 数据库迁移。
    /// </summary>
    public abstract class SettingsDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            // 配置表格
            builder.CreateTable<SettingsAdapter>(table => table
                .Column(s => s.UserId)
                .Column(s => s.SettingKey)
                .Column(s => s.SettingValue)
            );
        }
    }
}
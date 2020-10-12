using Gentings.Data.Migrations;

namespace Gentings.Extensions.Settings
{
    /// <summary>
    /// 数据库迁移。
    /// </summary>
    internal class SettingDictionaryDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<SettingDictionary>(table =>
                table.Column(x => x.Id).Column(x => x.ParentId).Column(x => x.Name).Column(x => x.Value));
        }
    }
}
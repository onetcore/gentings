using Gentings.Data.Migrations.Builders;
using Gentings.Extensions.Groups;

namespace Gentings.Extensions.Settings
{
    /// <summary>
    /// 数据库迁移。
    /// </summary>
    public abstract class NamedStringDataMigration : GroupDataMigration<NamedString>
    {
        /// <summary>
        /// 编辑表格其他属性列。
        /// </summary>
        /// <param name="table">当前表格构建实例对象。</param>
        protected override void Create(CreateTableBuilder<NamedString> table)
        {
            table.Column(x => x.Value);
        }
    }
}
using Gentings.Data.Migrations;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件数据库迁移类。
    /// </summary>
    public abstract class EventDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            // 事件类型
            builder.CreateTable<EventType>(table => table
                .Column(x => x.Id)
                .Column(x => x.Name)
                .Column(x => x.BgColor)
                .Column(x => x.Color)
                .Column(x => x.IconUrl));
            // 事件实例
            builder.CreateTable<Event>(table => table
                .Column(x => x.Id)
                .Column(x => x.EventId, defaultValue: 0)
                .Column(x => x.UserId)
                .Column(x => x.Level)
                .Column(x => x.Source)
                .Column(x => x.IPAdress)
                .Column(x => x.CreatedDate)
                .Column(x => x.ExtendProperties)
                .ForeignKey<EventType>(x => x.EventId, x => x.Id, onDelete: ReferentialAction.SetDefault)
            );
            // 索引
            builder.CreateIndex<Event>(x => new { x.UserId });

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
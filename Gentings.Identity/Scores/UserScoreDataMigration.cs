namespace Gentings.Identity.Scores
{
    using Gentings.Data.Migrations;

    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class UserScoreDataMigration : DataMigration
    {
        /// <summary>
        /// 优先级，在两个迁移数据需要先后时候使用。
        /// </summary>
        public override int Priority { get; } = -1;

        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<UserScore>(table => table
                .Column(x => x.UserId)
                .Column(x => x.Score)
                .Column(x => x.RowVersion)
                .Column(x => x.ScoredDate)
            );
            builder.CreateTable<ScoreLog>(table => table
                .Column(x => x.Id)
                .Column(x => x.UserId)
                .Column(x => x.Score)
                .Column(x => x.ScoreType)
                .Column(x => x.BeforeScore)
                .Column(x => x.AfterScore)
                .Column(x => x.SecurityKey)
                .Column(x => x.CreatedDate)
                .Column(x => x.Remark)
                .Column(x => x.TargetId));
            builder.CreateIndex<ScoreLog>(x => new { x.UserId, x.ScoreType, x.CreatedDate });
            builder.CreateTable<ScoreReporter>(table => table
                .Column(x => x.Id)
                .Column(x => x.UserId)
                .Column(x => x.Mode)
                .Column(x => x.Date)
                .Column(x => x.ScoreType)
                .Column(x => x.Score)
                .Column(x => x.CreatedDate)
            );
            builder.CreateIndex<ScoreReporter>(x => new { x.UserId, x.Mode, x.Date, x.ScoreType }, true);
        }
    }
}


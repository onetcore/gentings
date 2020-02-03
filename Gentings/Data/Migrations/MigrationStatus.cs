namespace Gentings.Data.Migrations
{
    /// <summary>
    /// 数据迁移状态。
    /// </summary>
    public enum MigrationStatus
    {
        /// <summary>
        /// 正在迁移。
        /// </summary>
        Normal,
        /// <summary>
        /// 完成。
        /// </summary>
        Completed,
        /// <summary>
        /// 错误。
        /// </summary>
        Error,
    }
}
namespace Gentings.Identity.Scores
{
    /// <summary>
    /// 更新积分状态。
    /// </summary>
    public enum UpdateScoreStatus
    {
        /// <summary>
        /// 成功。
        /// </summary>
        Success,

        /// <summary>
        /// 积分不足。
        /// </summary>
        NotEnough,

        /// <summary>
        /// 更新积分失败。
        /// </summary>
        ScoreError,

        /// <summary>
        /// 更新积分日志失败。
        /// </summary>
        LogError,
    }
}
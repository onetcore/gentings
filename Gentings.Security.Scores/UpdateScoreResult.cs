namespace Gentings.Security.Scores
{
    /// <summary>
    /// 更新积分结果。
    /// </summary>
    public class UpdateScoreResult
    {
        private UpdateScoreResult(UpdateScoreStatus status, int logId = 0)
        {
            Status = status;
            LogId = logId;
        }

        /// <summary>
        /// 状态。
        /// </summary>
        public UpdateScoreStatus Status { get; }

        /// <summary>
        /// 日志Id。
        /// </summary>
        public int LogId { get; }

        /// <summary>
        /// 隐式转换为布尔值。
        /// </summary>
        /// <param name="result">更新积分结果。</param>
        public static implicit operator bool(UpdateScoreResult result) => result.Status == UpdateScoreStatus.Success;

        /// <summary>
        /// 隐式转换为更新结果。
        /// </summary>
        /// <param name="status">更新积分状态。</param>
        public static implicit operator UpdateScoreResult(UpdateScoreStatus status) => new UpdateScoreResult(status);

        /// <summary>
        /// 隐式转换为更新结果。
        /// </summary>
        /// <param name="logId">更新积分日志Id。</param>
        public static implicit operator UpdateScoreResult(int logId) => new UpdateScoreResult(UpdateScoreStatus.Success, logId);
    }
}
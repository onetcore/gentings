using System;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data.Internal;

namespace Gentings.Identity.Scores
{
    /// <summary>
    /// 用户积分扩展类。
    /// </summary>
    public static class UserScoreExtensions
    {
        /// <summary>
        /// 更新用户积分。
        /// </summary>
        /// <param name="db">数据库事务接口实例。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">描述。</param>
        /// <param name="scoreType">积分使用类型。</param>
        /// <returns>返回添加结果。</returns>
        public static UpdateScoreResult UpdateScore(this IDbTransactionContext<UserScore> db, int userId, int score, string remark = null, ScoreType? scoreType = null)
        {
            var userScore = db.AsQueryable().WithNolock().Where(x => x.UserId == userId).FirstOrDefault();
            if (userScore == null || userScore.Score < score)
                return UpdateScoreStatus.NotEnough;

            var log = new ScoreLog();
            log.BeforeScore = userScore.Score;
            log.Score = -score;
            userScore.Score -= score;
            userScore.ScoredDate = DateTimeOffset.Now;
            if (scoreType == null)
                scoreType = score > 0 ? ScoreType.Consume : ScoreType.Recharge;
            log.ScoreType = scoreType.Value;
            if (!db.Update(x => x.UserId == userId && x.RowVersion == userScore.RowVersion, new { userScore.Score, userScore.ScoredDate }))
                return UpdateScoreStatus.ScoreError;

            log.AfterScore = userScore.Score;
            log.Remark = remark;
            log.UserId = userId;

            var sdb = db.As<ScoreLog>();
            if (sdb.Create(log))
                return log.Id;
            return UpdateScoreStatus.LogError;
        }

        /// <summary>
        /// 更新用户积分。
        /// </summary>
        /// <param name="db">数据库事务接口实例。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">描述。</param>
        /// <param name="scoreType">积分使用类型。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回添加结果。</returns>
        public static async Task<UpdateScoreResult> UpdateScoreAsync(this IDbTransactionContext<UserScore> db, int userId, int score, string remark = null, ScoreType? scoreType = null, CancellationToken cancellationToken = default)
        {
            var userScore = await db.AsQueryable().WithNolock().Where(x => x.UserId == userId).FirstOrDefaultAsync(cancellationToken);
            if (userScore == null || userScore.Score < score)
                return UpdateScoreStatus.NotEnough;

            var log = new ScoreLog();
            log.BeforeScore = userScore.Score;
            log.Score = -score;
            userScore.Score -= score;
            userScore.ScoredDate = DateTimeOffset.Now;
            if (scoreType == null)
                scoreType = score > 0 ? ScoreType.Consume : ScoreType.Recharge;
            log.ScoreType = scoreType.Value;
            if (!await db.UpdateAsync(x => x.UserId == userId && x.RowVersion == userScore.RowVersion, new { userScore.Score, userScore.ScoredDate }, cancellationToken))
                return UpdateScoreStatus.ScoreError;

            log.AfterScore = userScore.Score;
            log.Remark = remark;
            log.UserId = userId;

            var sdb = db.As<ScoreLog>();
            if (await sdb.CreateAsync(log, cancellationToken))
                return log.Id;
            return UpdateScoreStatus.LogError;
        }
    }
}
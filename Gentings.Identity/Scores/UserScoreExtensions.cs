﻿using System;
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
        public static bool UpdateScore(this IDbTransactionContext<UserScore> db, int userId, int score, string remark = null, ScoreType? scoreType = null)
        {
            var userScore = db.AsQueryable().WithNolock().Where(x => x.UserId == userId).FirstOrDefault();
            if (userScore == null || userScore.Score < score)
                return false;

            var log = new ScoreLog();
            log.BeforeScore = userScore.Score;
            log.Score = -score;
            userScore.Score -= score;
            userScore.ScoredDate = DateTimeOffset.Now;
            if (scoreType == null)
                scoreType = score > 0 ? ScoreType.Consume : ScoreType.Recharge;
            log.ScoreType = scoreType.Value;
            if (!db.Update(userId, new { userScore.Score, userScore.ScoredDate }))
                return false;

            log.AfterScore = userScore.Score;
            log.Remark = remark;
            log.UserId = userId;

            var sdb = db.As<ScoreLog>();
            return sdb.Create(log);
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
        public static async Task<bool> UpdateScoreAsync(this IDbTransactionContext<UserScore> db, int userId, int score, string remark = null, ScoreType? scoreType = null, CancellationToken cancellationToken = default)
        {
            var userScore = await db.AsQueryable().WithNolock().Where(x => x.UserId == userId).FirstOrDefaultAsync(cancellationToken);
            if (userScore == null || userScore.Score < score)
                return false;

            var log = new ScoreLog();
            log.BeforeScore = userScore.Score;
            log.Score = -score;
            userScore.Score -= score;
            userScore.ScoredDate = DateTimeOffset.Now;
            if (scoreType == null)
                scoreType = score > 0 ? ScoreType.Consume : ScoreType.Recharge;
            log.ScoreType = scoreType.Value;
            if (!await db.UpdateAsync(userId, new { userScore.Score, userScore.ScoredDate }, cancellationToken))
                return false;

            log.AfterScore = userScore.Score;
            log.Remark = remark;
            log.UserId = userId;

            var sdb = db.As<ScoreLog>();
            return await sdb.CreateAsync(log, cancellationToken);
        }
    }
}
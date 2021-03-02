using System;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Extensions;

namespace Gentings.Identity.Scores
{
    /// <summary>
    /// 积分管理实现类。
    /// </summary>
    public class ScoreManager : IScoreManager
    {
        /// <summary>
        /// 日志数据库操作接口。
        /// </summary>
        protected IDbContext<ScoreLog> LogContext { get; }

        /// <summary>
        /// 数据库操作接口。
        /// </summary>
        protected IDbContext<UserScore> Context { get; }

        /// <summary>
        /// 初始化类<see cref="ScoreManager"/>。
        /// </summary>
        /// <param name="context">数据库操作接口。</param>
        /// <param name="logContext">日志数据库操作接口。</param>
        public ScoreManager(IDbContext<UserScore> context, IDbContext<ScoreLog> logContext)
        {
            LogContext = logContext;
            Context = context;
        }

        /// <summary>
        /// 获取用户积分。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户积分。</returns>
        public virtual int GetScore(int userId)
        {
            return Context.AsQueryable()
                .WithNolock()
                .Select(x => x.Score)
                .Where(x => x.UserId == userId)
                .FirstOrDefault(reader => reader.GetInt32(0));
        }

        /// <summary>
        /// 获取用户积分。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户积分。</returns>
        public virtual Task<int> GetScoreAsync(int userId)
        {
            return Context.AsQueryable()
                .WithNolock()
                .Select(x => x.Score)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(reader => reader.GetInt32(0));
        }

        /// <summary>
        /// 判断消费积分是否足够。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsEnough(int userId, int score)
        {
            return GetScore(userId) > score;
        }

        /// <summary>
        /// 判断消费积分是否足够。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <returns>返回判断结果。</returns>
        public virtual async Task<bool> IsEnoughAsync(int userId, int score)
        {
            return await GetScoreAsync(userId) > score;
        }

        /// <summary>
        /// 充值积分。
        /// </summary>
        /// <param name="sourceId">原始用户Id。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">备注。</param>
        /// <returns>返回充值结果。</returns>
        public virtual UpdateScoreResult Recharge(int sourceId, int userId, int score, string remark = null)
        {
            UpdateScoreResult result = UpdateScoreStatus.ScoreError;
            Context.BeginTransaction(db =>
            {
                result = db.UpdateScore(sourceId, score, remark);
                if (result)
                    result = db.UpdateScore(userId, -score, remark);
                return result;
            });
            return result;
        }

        /// <summary>
        /// 充值积分。
        /// </summary>
        /// <param name="sourceId">原始用户Id。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">备注。</param>
        /// <returns>返回充值结果。</returns>
        public virtual async Task<UpdateScoreResult> RechargeAsync(int sourceId, int userId, int score, string remark = null)
        {
            UpdateScoreResult result = UpdateScoreStatus.ScoreError;
            await Context.BeginTransactionAsync(async db =>
            {
                result = await db.UpdateScoreAsync(sourceId, score, remark);
                if (result)
                    result = await db.UpdateScoreAsync(userId, -score, remark);
                return result;
            });
            return result;
        }

        /// <summary>
        /// 分页加载用户积分。
        /// </summary>
        /// <param name="query">用户积分查询实例。</param>
        /// <returns>返回用户积分列表。</returns>
        public virtual IPageEnumerable<ScoreLog> LoadScores(ScoreLogQuery query)
        {
            return LogContext.Load(query);
        }

        /// <summary>
        /// 分页加载用户积分。
        /// </summary>
        /// <param name="query">用户积分查询实例。</param>
        /// <returns>返回用户积分列表。</returns>
        public virtual Task<IPageEnumerable<ScoreLog>> LoadScoresAsync(ScoreLogQuery query)
        {
            return LogContext.LoadAsync(query);
        }

        /// <summary>
        /// 更新用户积分。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">描述。</param>
        /// <param name="scoreType">积分使用类型。</param>
        /// <returns>返回添加结果。</returns>
        public virtual UpdateScoreResult UpdateScore(int userId, int score, string remark = null, ScoreType? scoreType = null)
        {
            UpdateScoreResult result = UpdateScoreStatus.ScoreError;
            Context.BeginTransaction(db =>
            {
                result = db.UpdateScore(userId, score, remark, scoreType);
                return result;
            });
            return result;
        }

        /// <summary>
        /// 更新用户积分。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">描述。</param>
        /// <param name="scoreType">积分使用类型。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回添加结果。</returns>
        public virtual async Task<UpdateScoreResult> UpdateScoreAsync(int userId, int score, string remark = null, ScoreType? scoreType = null, CancellationToken cancellationToken = default)
        {
            UpdateScoreResult result = UpdateScoreStatus.ScoreError;
            await Context.BeginTransactionAsync(async db =>
            {
                result = await db.UpdateScoreAsync(userId, score, remark, scoreType, cancellationToken);
                return result;
            }, cancellationToken: cancellationToken);
            return result;
        }

        /// <summary>
        /// 回滚积分日志，将积分返还给用户，并删除日志。
        /// </summary>
        /// <param name="logId">日志Id。</param>
        /// <returns>返回回滚结果。</returns>
        public virtual bool RollbackScore(int logId)
        {
            return Context.BeginTransaction(db =>
            {
                var logDb = db.As<ScoreLog>();
                var log = logDb.Find(logId);
                if (log == null)
                    return false;
                var score = db.AsQueryable()
                    .WithNolock()
                    .Where(x => x.UserId == log.UserId)
                    .FirstOrDefault();
                score.Score += log.Score;
                score.ScoredDate = DateTimeOffset.Now;
                if (db.Update(x => x.UserId == score.UserId && x.RowVersion == score.RowVersion,
                    new { score.Score, score.ScoredDate }))
                    return logDb.Delete(logId);
                return false;
            });
        }

        /// <summary>
        /// 回滚积分日志，将积分返还给用户，并删除日志。
        /// </summary>
        /// <param name="logId">日志Id。</param>
        /// <returns>返回回滚结果。</returns>
        public virtual Task<bool> RollbackScoreAsync(int logId)
        {
            return Context.BeginTransactionAsync(async db =>
            {
                var logDb = db.As<ScoreLog>();
                var log = await logDb.FindAsync(logId);
                if (log == null)
                    return false;
                var score = await db.AsQueryable()
                    .WithNolock()
                    .Where(x => x.UserId == log.UserId)
                    .FirstOrDefaultAsync();
                score.Score -= log.Score;
                score.ScoredDate = DateTimeOffset.Now;
                if (await db.UpdateAsync(x => x.UserId == score.UserId && x.RowVersion == score.RowVersion,
                    new { score.Score, score.ScoredDate }))
                    return await logDb.DeleteAsync(logId);
                return false;
            });
        }
    }
}
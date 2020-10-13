﻿using System.Threading;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Extensions;

namespace Gentings.Identity.Scores
{
    /// <summary>
    /// 积分管理接口。
    /// </summary>
    public interface IScoreManager : ISingletonService
    { /// <summary>
        /// 判断消费积分是否足够。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsEnoughAsync(int userId, int score);

        /// <summary>
        /// 充值积分。
        /// </summary>
        /// <param name="sourceId">原始用户Id。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">备注。</param>
        /// <returns>返回充值结果。</returns>
        Task<bool> RechargeAsync(int sourceId, int userId, int score, string remark = null);

        /// <summary>
        /// 分页加载用户积分。
        /// </summary>
        /// <param name="query">用户积分查询实例。</param>
        /// <returns>返回用户积分列表。</returns>
        Task<IPageEnumerable<ScoreLog>> LoadScoresAsync(ScoreLogQuery query);

        /// <summary>
        /// 更新用户积分。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">描述。</param>
        /// <param name="scoreType">积分使用类型。</param>
        /// <returns>返回添加结果。</returns>
        bool UpdateScore(int userId, int score, string remark = null, ScoreType? scoreType = null);

        /// <summary>
        /// 更新用户积分。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">描述。</param>
        /// <param name="scoreType">积分使用类型。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> UpdateScoreAsync(int userId, int score, string remark = null, ScoreType? scoreType = null, CancellationToken cancellationToken = default);
    }

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
        /// 判断消费积分是否足够。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <returns>返回判断结果。</returns>
        public virtual async Task<bool> IsEnoughAsync(int userId, int score)
        {
            return await Context.AsQueryable()
                .WithNolock()
                .Select(x => x.Score)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(reader => reader.GetInt32(0)) > score;
        }

        /// <summary>
        /// 充值积分。
        /// </summary>
        /// <param name="sourceId">原始用户Id。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="score">用户积分。</param>
        /// <param name="remark">备注。</param>
        /// <returns>返回充值结果。</returns>
        public virtual Task<bool> RechargeAsync(int sourceId, int userId, int score, string remark = null)
        {
            return Context.BeginTransactionAsync(async db =>
            {
                if (await db.UpdateScoreAsync(sourceId, score, remark))
                    return await db.UpdateScoreAsync(userId, -score, remark);
                return false;
            });
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
        public virtual bool UpdateScore(int userId, int score, string remark = null, ScoreType? scoreType = null)
        {
            return Context.BeginTransaction(db => db.UpdateScore(userId, score, remark, scoreType));
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
        public virtual Task<bool> UpdateScoreAsync(int userId, int score, string remark = null, ScoreType? scoreType = null, CancellationToken cancellationToken = default)
        {
            return Context.BeginTransactionAsync(db => db.UpdateScoreAsync(userId, score, remark, scoreType, cancellationToken), cancellationToken: cancellationToken);
        }
    }
}
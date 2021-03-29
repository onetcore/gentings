using Gentings.Data;

namespace Gentings.Security.Scores
{
    /// <summary>
    /// 用户积分查询。
    /// </summary>
    public class ScoreLogQuery : QueryBase<ScoreLog>
    {
        /// <summary>
        /// 积分使用类型。
        /// </summary>
        public ScoreType? ScoreType { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<ScoreLog> context)
        {
            context.WithNolock().Where(x => x.UserId == UserId);
            if (ScoreType != null)
                context.Where(x => x.ScoreType == ScoreType);
            context.OrderByDescending(x => x.Id);
        }

        /// <summary>
        /// 当前用户Id。
        /// </summary>
        public int UserId { get; set; }
    }
}
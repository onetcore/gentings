using System.Data;
using Microsoft.Extensions.Logging;
using Gentings.Data.Query;

namespace Gentings.Data.Internal
{
    /// <summary>
    /// 数据库操作实现类。
    /// </summary>
    /// <typeparam name="TModel">实体模型。</typeparam>
    public class DbContext<TModel> : DbContextBase<TModel>, IDbContext<TModel>
    {
        private readonly IDatabase _executor;
        private readonly ILogger<IDatabase> _logger;

        /// <summary>
        /// 初始化类<see cref="DbContextBase{TModel}"/>。
        /// </summary>
        /// <param name="executor">数据库执行接口。</param>
        /// <param name="logger">日志接口。</param>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        /// <param name="sqlGenerator">脚本生成器。</param>
        /// <param name="visitorFactory">条件表达式解析器工厂实例。</param>
        public DbContext(IDatabase executor, ILogger<IDatabase> logger, ISqlHelper sqlHelper,
            IQuerySqlGenerator sqlGenerator, IExpressionVisitorFactory visitorFactory)
            : base(executor, logger, sqlHelper, sqlGenerator, visitorFactory)
        {
            _executor = executor;
            _logger = logger;
        }

        /// <summary>
        /// 开启一个事务并执行。
        /// </summary>
        /// <param name="executor">事务执行的方法。</param>
        /// <param name="timeout">等待命令执行所需的时间（以秒为单位）。默认值为 30 秒。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回事务实例对象。</returns>
        public virtual async Task<bool> BeginTransactionAsync(Func<IDbTransactionContext<TModel>, Task<bool>> executor,
            int timeout = 30, CancellationToken cancellationToken = default)
        {
            return
                await
                    _executor.BeginTransactionAsync(
                        async transaction =>
                            await
                                executor(new DbTransactionContext<TModel>(transaction, Logger, SqlHelper, SqlGenerator,
                                    VisitorFactory)), timeout, cancellationToken);
        }

        /// <summary>
        /// 开启一个事务并执行。
        /// </summary>
        /// <param name="executor">事务执行的方法。</param>
        /// <param name="timeout">等待命令执行所需的时间（以秒为单位）。默认值为 30 秒。</param>
        /// <returns>返回事务实例对象。</returns>
        public virtual bool BeginTransaction(Func<IDbTransactionContext<TModel>, bool> executor, int timeout = 30)
        {
            return
                _executor.BeginTransaction(
                    transaction =>
                        executor(new DbTransactionContext<TModel>(transaction, Logger, SqlHelper, SqlGenerator,
                            VisitorFactory)), timeout);
        }

        /// <summary>
        /// 批量插入数据。
        /// </summary>
        /// <param name="table">模型列表。</param>
        public virtual Task ImportAsync(DataTable table)
        {
            return _executor.ImportAsync(table);
        }

        /// <summary>
        /// 实例化一个查询实例。
        /// </summary>
        /// <typeparam name="TOther">模型类型。</typeparam>
        /// <returns>返回其他模型的查询实例。</returns>
        public virtual IDbContext<TOther> As<TOther>()
        {
            return new DbContext<TOther>(_executor, _logger, SqlHelper, SqlGenerator, VisitorFactory);
        }
    }
}
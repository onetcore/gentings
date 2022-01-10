using System.Data;
using Gentings.Data.Internal;

namespace Gentings.Data
{
    /// <summary>
    /// 实体数据库操作接口。
    /// </summary>
    /// <typeparam name="TModel">实体模型。</typeparam>
    public interface IDbContext<TModel> : IDbContextBase<TModel>
    {
        /// <summary>
        /// 开启一个事务并执行。
        /// </summary>
        /// <param name="executor">事务执行的方法。</param>
        /// <param name="timeout">等待命令执行所需的时间（以秒为单位）。默认值为 30 秒。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回事务实例对象。</returns>
        Task<bool> BeginTransactionAsync(Func<IDbTransactionContext<TModel>, Task<bool>> executor, int timeout = 30,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 开启一个事务并执行。
        /// </summary>
        /// <param name="executor">事务执行的方法。</param>
        /// <param name="timeout">等待命令执行所需的时间（以秒为单位）。默认值为 30 秒。</param>
        /// <returns>返回事务实例对象。</returns>
        bool BeginTransaction(Func<IDbTransactionContext<TModel>, bool> executor, int timeout = 30);

        /// <summary>
        /// 批量插入数据。
        /// </summary>
        /// <param name="table">模型列表。</param>
        Task ImportAsync(DataTable table);

        /// <summary>
        /// 实例化一个查询实例。
        /// </summary>
        /// <typeparam name="TOther">模型类型。</typeparam>
        /// <returns>返回其他模型的查询实例。</returns>
        IDbContext<TOther> As<TOther>();
    }
}
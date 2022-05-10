using System.Linq.Expressions;

namespace Gentings.Extensions
{
    /// <summary>
    /// 对象管理基础接口。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public interface IObjectManagerBase<TModel>
    {
        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <returns>返回添加结果。</returns>
        bool Create(TModel model);

        /// <summary>
        /// 更新特定的实例。
        /// </summary>
        /// <param name="model">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        bool Update(TModel model);

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="fields">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        DataResult Update(Expression<Predicate<TModel>> expression, object fields);

        /// <summary>
        /// 根据条件删除实例。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(Expression<Predicate<TModel>> expression);

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回当前模型实例。</returns>
        TModel? Find(Expression<Predicate<TModel>> expression);

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例列表。</returns>
        IEnumerable<TModel> Fetch(Expression<Predicate<TModel>>? expression = null);

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> CreateAsync(TModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新特定的实例。
        /// </summary>
        /// <param name="model">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="fields">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        Task<DataResult> UpdateAsync(Expression<Predicate<TModel>> expression, object fields,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        void Clear();

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        Task ClearAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件删除实例。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        Task<DataResult> DeleteAsync(Expression<Predicate<TModel>> expression,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前模型实例。</returns>
        Task<TModel?> FindAsync(Expression<Predicate<TModel>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回模型实例列表。</returns>
        Task<IEnumerable<TModel>> FetchAsync(Expression<Predicate<TModel>>? expression = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 实例化一个查询实例，这个实例相当于实例化一个查询类，不能当作属性直接调用。
        /// </summary>
        /// <returns>返回模型的一个查询实例。</returns>
        Data.IQueryable<TModel> AsQueryable();

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <returns>返回分页实例列表。</returns>
        IPageEnumerable<TModel> Load<TQuery>(TQuery query, Expression<Func<TModel, object?>>? countExpression = null)
            where TQuery : QueryBase<TModel>;

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <returns>返回分页实例列表。</returns>
        IPageEnumerable<TObject> Load<TQuery, TObject>(TQuery query,
            Expression<Func<TModel, object?>>? countExpression = null) where TQuery : QueryBase<TModel>;

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回分页实例列表。</returns>
        Task<IPageEnumerable<TModel>> LoadAsync<TQuery>(TQuery query,
            Expression<Func<TModel, object?>>? countExpression = null, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TModel>;

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回分页实例列表。</returns>
        Task<IPageEnumerable<TObject>> LoadAsync<TQuery, TObject>(TQuery query,
            Expression<Func<TModel, object?>>? countExpression = null, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TModel>;
    }
}
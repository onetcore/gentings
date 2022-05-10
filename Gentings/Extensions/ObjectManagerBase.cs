using System.Linq.Expressions;
using Gentings.Data;

namespace Gentings.Extensions
{
    /// <summary>
    /// 对象管理实现基类。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public abstract class ObjectManagerBase<TModel> : IObjectManagerBase<TModel>
    {
        /// <summary>
        /// 数据库操作实例。
        /// </summary>
        protected IDbContext<TModel> Context { get; }

        /// <summary>
        /// 初始化类<see cref="ObjectManagerBase{TModel}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        protected ObjectManagerBase(IDbContext<TModel> context)
        {
            Context = context;
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="fields">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public virtual DataResult Update(Expression<Predicate<TModel>> expression, object fields)
        {
            return DataResult.FromResult(Context.Update(expression, fields), DataAction.Updated);
        }

        /// <summary>
        /// 根据条件删除实例。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(Expression<Predicate<TModel>> expression)
        {
            return DataResult.FromResult(Context.Delete(expression), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回当前模型实例。</returns>
        public virtual TModel? Find(Expression<Predicate<TModel>> expression)
        {
            return Context.Find(expression);
        }

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例列表。</returns>
        public virtual IEnumerable<TModel> Fetch(Expression<Predicate<TModel>>? expression = null)
        {
            return Context.Fetch(expression);
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="fields">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public virtual async Task<DataResult> UpdateAsync(Expression<Predicate<TModel>> expression, object fields,
            CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.UpdateAsync(expression, fields, cancellationToken),
                DataAction.Updated);
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public virtual void Clear()
        {
            Context.Delete();
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public virtual Task ClearAsync(CancellationToken cancellationToken = default)
        {
            return Context.DeleteAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 根据条件删除实例。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(Expression<Predicate<TModel>> expression,
            CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.DeleteAsync(expression, cancellationToken), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前模型实例。</returns>
        public virtual Task<TModel?> FindAsync(Expression<Predicate<TModel>> expression,
            CancellationToken cancellationToken = default)
        {
            return Context.FindAsync(expression, cancellationToken);
        }

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回模型实例列表。</returns>
        public virtual Task<IEnumerable<TModel>> FetchAsync(Expression<Predicate<TModel>>? expression = null,
            CancellationToken cancellationToken = default)
        {
            return Context.FetchAsync(expression, cancellationToken);
        }

        /// <summary>
        /// 实例化一个查询实例，这个实例相当于实例化一个查询类，不能当作属性直接调用。
        /// </summary>
        /// <returns>返回模型的一个查询实例。</returns>
        public virtual Data.IQueryable<TModel> AsQueryable()
        {
            return Context.AsQueryable();
        }

        /// <summary>
        /// 更新特定的实例。
        /// </summary>
        /// <param name="model">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public virtual bool Update(TModel model)
        {
            return Context.Update(model);
        }

        /// <summary>
        /// 更新特定的实例。
        /// </summary>
        /// <param name="model">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public virtual Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Context.UpdateAsync(model, cancellationToken);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool Create(TModel model)
        {
            return Context.Create(model);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回添加结果。</returns>
        public virtual Task<bool> CreateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Context.CreateAsync(model, cancellationToken);
        }

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <returns>返回分页实例列表。</returns>
        public virtual IPageEnumerable<TModel> Load<TQuery>(TQuery query,
            Expression<Func<TModel, object?>>? countExpression = null) where TQuery : QueryBase<TModel>
        {
            return Context.Load(query, countExpression);
        }

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <returns>返回分页实例列表。</returns>
        public virtual IPageEnumerable<TObject> Load<TQuery, TObject>(TQuery query,
            Expression<Func<TModel, object?>>? countExpression = null) where TQuery : QueryBase<TModel>
        {
            return Context.Load<TQuery, TObject>(query, countExpression);
        }

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回分页实例列表。</returns>
        public virtual Task<IPageEnumerable<TModel>> LoadAsync<TQuery>(TQuery query,
            Expression<Func<TModel, object?>>? countExpression = null, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TModel>
        {
            return Context.LoadAsync(query, countExpression, cancellationToken);
        }

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回分页实例列表。</returns>
        public virtual Task<IPageEnumerable<TObject>> LoadAsync<TQuery, TObject>(TQuery query,
            Expression<Func<TModel, object?>>? countExpression = null, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TModel>
        {
            return Context.LoadAsync<TQuery, TObject>(query, countExpression, cancellationToken);
        }
    }
}
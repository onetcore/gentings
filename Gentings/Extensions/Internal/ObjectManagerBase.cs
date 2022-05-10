using Gentings.Data;

namespace Gentings.Extensions.Internal
{
    /// <summary>
    /// 对象管理基类。
    /// </summary>
    /// <typeparam name="TModel">当前模型实例。</typeparam>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public abstract class ObjectManagerBase<TModel, TKey> : ObjectManagerBase<TModel>, IObjectManagerBase<TModel, TKey>
        where TModel : IIdObject<TKey>
    {
        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public virtual DataResult Save(TModel model)
        {
            if (IsDuplicated(model))
            {
                return DataAction.Duplicate;
            }

            if (Context.Any(model.Id!))
            {
                return DataResult.FromResult(Update(model), DataAction.Updated);
            }

            return DataResult.FromResult(Create(model), DataAction.Created);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsDuplicated(TModel model)
        {
            return false;
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <param name="fields">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public virtual DataResult Update(TKey id, object fields)
        {
            return DataResult.FromResult(Context.Update(id!, fields), DataAction.Updated);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(TKey id)
        {
            return DataResult.FromResult(Context.Delete(id!), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <returns>返回当前模型实例。</returns>
        public virtual TModel? Find(TKey id)
        {
            return Context.Find(id!);
        }

        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        public virtual async Task<DataResult> SaveAsync(TModel model, CancellationToken cancellationToken = default)
        {
            if (await IsDuplicatedAsync(model, cancellationToken))
            {
                return DataAction.Duplicate;
            }

            if (Context.Any(model.Id!))
            {
                return DataResult.FromResult(await UpdateAsync(model, cancellationToken), DataAction.Updated);
            }

            return DataResult.FromResult(await CreateAsync(model, cancellationToken), DataAction.Created);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        public virtual Task<bool> IsDuplicatedAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <param name="fields">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public virtual async Task<DataResult> UpdateAsync(TKey id, object fields,
            CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.UpdateAsync(id!, fields, cancellationToken),
                DataAction.Updated);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="ids">唯一Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(IEnumerable<TKey> ids)
        {
            return DataResult.FromResult(Context.Delete(x => x.Id!.Included(ids)), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="ids">唯一Id集合。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(IEnumerable<TKey> ids,
            CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.DeleteAsync(x => x.Id!.Included(ids), cancellationToken),
                DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.DeleteAsync(id!, cancellationToken), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前模型实例。</returns>
        public virtual Task<TModel?> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Context.FindAsync(id!, cancellationToken);
        }

        /// <summary>
        /// 初始化类<see cref="ObjectManagerBase{TModel, TKey}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        protected ObjectManagerBase(IDbContext<TModel> context) : base(context)
        {
        }
    }
}
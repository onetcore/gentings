namespace Gentings.Extensions.Internal
{
    /// <summary>
    /// 对象管理接口。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public interface IObjectManagerBase<TModel, TKey> : IObjectManagerBase<TModel>
        where TModel : IIdObject<TKey>
    {
        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        DataResult Save(TModel model);

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        bool IsDuplicated(TModel model);

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <param name="fields">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        DataResult Update(TKey id, object fields);

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(TKey id);

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <returns>返回当前模型实例。</returns>
        TModel? Find(TKey id);

        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        Task<DataResult> SaveAsync(TModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        Task<bool> IsDuplicatedAsync(TModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <param name="fields">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        Task<DataResult> UpdateAsync(TKey id, object fields, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="ids">唯一Id集合。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(IEnumerable<TKey> ids);

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="ids">唯一Id集合。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        Task<DataResult> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        Task<DataResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="id">唯一Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前模型实例。</returns>
        Task<TModel?> FindAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
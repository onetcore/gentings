namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 对象属性更改管理接口。
    /// </summary>
    public interface IDifferManager
    {
        /// <summary>
        /// 添加对象对比实例。
        /// </summary>
        /// <param name="differ">对象对比实例。</param>
        /// <returns>返回添加结果。</returns>
        bool Create(IObjectDiffer differ);

        /// <summary>
        /// 添加对象对比实例。
        /// </summary>
        /// <param name="differ">对象对比实例。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> CreateAsync(IObjectDiffer differ);

        /// <summary>
        /// 分页加载对象对比实例列表。
        /// </summary>
        /// <param name="query">对象对比查询实例。</param>
        /// <returns>返回对象对比实例列表。</returns>
        IPageEnumerable<Differ> Load(DifferQuery query);

        /// <summary>
        /// 分页加载对象对比实例列表。
        /// </summary>
        /// <param name="query">对象对比查询实例。</param>
        /// <returns>返回对象对比实例列表。</returns>
        Task<IPageEnumerable<Differ>> LoadAsync(DifferQuery query);

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="ids">对象对比实例Id集合。</param>
        /// <returns>返回删除结果。</returns>
        bool Delete(int[] ids);

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="ids">对象对比实例Id集合。</param>
        /// <returns>返回删除结果。</returns>
        Task<bool> DeleteAsync(int[] ids);

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="typeName">类型名称。</param>
        /// <returns>返回删除结果。</returns>
        bool Delete(string typeName);

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="typeName">类型名称。</param>
        /// <returns>返回删除结果。</returns>
        Task<bool> DeleteAsync(string typeName);

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        bool Delete(int userId);

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        Task<bool> DeleteAsync(int userId);

        /// <summary>
        /// 查询对象对比实例。
        /// </summary>
        /// <param name="id">对象对比Id。</param>
        /// <returns>返回对象对比实例。</returns>
        Differ GetDiffer(int id);

        /// <summary>
        /// 查询对象对比实例。
        /// </summary>
        /// <param name="id">对象对比Id。</param>
        /// <returns>返回对象对比实例。</returns>
        Task<Differ> GetDifferAsync(int id);
    }
}
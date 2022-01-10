namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 对象变更对比接口。
    /// </summary>
    public interface IObjectDiffer : IEnumerable<Differ>
    {
        /// <summary>
        /// 对象新的对象，判断是否已经变更。
        /// </summary>
        /// <param name="instance">新对象实例。</param>
        /// <returns>返回对比结果。</returns>
        bool IsDifference(object instance);

        /// <summary>
        /// 保存当前对象对比实例。
        /// </summary>
        /// <returns>返回保存结果。</returns>
        bool Save();

        /// <summary>
        /// 保存当前对象对比实例。
        /// </summary>
        /// <returns>返回保存结果。</returns>
        Task<bool> SaveAsync();
    }
}
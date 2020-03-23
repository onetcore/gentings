namespace Gentings.Extensions
{
    /// <summary>
    /// 对象变更对比接口。
    /// </summary>
    public interface IObjectDiffer : IService
    {
        /// <summary>
        /// 存储对象的属性，一般为原有对象实例。
        /// </summary>
        /// <typeparam name="T">当前对象类型。</typeparam>
        /// <param name="oldInstance">原有对象实例。</param>
        /// <returns>返回当前实例。</returns>
        T Stored<T>(T oldInstance);

        /// <summary>
        /// 对象新的对象，判断是否已经变更。
        /// </summary>
        /// <param name="newInstance">新对象实例。</param>
        /// <returns>返回对比结果。</returns>
        bool IsDifference(object newInstance);
    }
}
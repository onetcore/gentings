namespace Gentings.Blazored
{
    /// <summary>
    /// 包含数据的结果。
    /// </summary>
    /// <typeparam name="TData">数据类型。</typeparam>
    public class ServiceDataResult<TData> : ServiceResult
    {
        /// <summary>
        /// 数据实例。
        /// </summary>
        public TData Data { get; set; }
    }
}
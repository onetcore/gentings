namespace Gentings.AspNetCore
{
    /// <summary>
    /// 包含数据的结果。
    /// </summary>
    public class ApiDataResult : ApiResult
    {
        /// <summary>
        /// 初始化类<see cref="ApiDataResult"/>。
        /// </summary>
        /// <param name="data">数据实例。</param>
        public ApiDataResult(object data)
        {
            Data = data;
        }

        /// <summary>
        /// 数据实例。
        /// </summary>
        public object Data { get; }
    }
}
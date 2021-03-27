namespace Gentings.AspNetCore.OpenServices.Controllers
{
    /// <summary>
    /// 输出模型。
    /// </summary>
    public class TokenResult : ApiDataResult<string>
    {
        /// <summary>
        /// 初始化类<see cref="TokenResult"/>。
        /// </summary>
        /// <param name="data">数据实例。</param>
        public TokenResult(string data) : base(data)
        {
        }

        /// <summary>
        /// 用于返回特性使用。
        /// </summary>
        public TokenResult() : base(Cores.GeneralKey(512)) { }
    }
}
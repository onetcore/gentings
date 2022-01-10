using Gentings.AspNetCore;

namespace Gentings.Extensions.OpenServices.Controllers
{
    /// <summary>
    /// 输出模型。
    /// </summary>
    public class TokenResult : ApiDataResult
    {
        /// <summary>
        /// 初始化类<see cref="TokenResult"/>。
        /// </summary>
        /// <param name="data">数据实例。</param>
        public TokenResult(string data) : base(data)
        {
        }

        /// <summary>
        /// 初始化类<see cref="TokenResult"/>。
        /// </summary>
        public TokenResult() : base(Cores.GeneralKey(128)) { }
    }
}
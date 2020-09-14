using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 敏感词汇管理接口。
    /// </summary>
    public interface ISensitiveWordManager : IObjectManager<SensitiveWord>, ISingletonService
    {
        /// <summary>
        /// 导入敏感词汇。
        /// </summary>
        /// <param name="words">敏感词汇列表。</param>
        /// <returns>返回导入结果。</returns>
        Task<bool> ImportAsync(IEnumerable<string> words);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 敏感词汇管理接口。
    /// </summary>
    public interface ISensitiveWordManager : ISingletonService
    {
        /// <summary>
        /// 导入敏感词汇。
        /// </summary>
        /// <param name="words">敏感词汇列表。</param>
        /// <returns>返回导入结果。</returns>
        Task<bool> ImportAsync(IEnumerable<string> words);

        /// <summary>
        /// 通过文件导入敏感词汇。
        /// </summary>
        /// <param name="file">上传文件实例。</param>
        /// <returns>返回导入词汇数量。</returns>
        Task<int> ImportAsync(IFormFile file);

        /// <summary>
        /// 是否包含敏感词。
        /// </summary>
        /// <param name="text">判断的字符串。</param>
        /// <returns>返回判断结果。</returns>
        bool Contains(string text);

        /// <summary>
        /// 是否包含敏感词。
        /// </summary>
        /// <param name="text">判断的字符串。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> ContainsAsync(string text);

        /// <summary>
        /// 在文本中替换所有的关键字。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <param name="replaceChar">替换字符。</param>
        /// <returns>返回替换结果。</returns>
        string Replace(string text, char replaceChar = '*');

        /// <summary>
        /// 在文本中替换所有的关键字。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <param name="replaceChar">替换字符。</param>
        /// <returns>返回替换结果。</returns>
        Task<string> ReplaceAsync(string text, char replaceChar = '*');

        /// <summary>
        /// 在文本中查找第一个关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回第一个关键词。</returns>
        string FindFirst(string text);

        /// <summary>
        /// 在文本中查找第一个关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回第一个关键词。</returns>
        Task<string> FindFirstAsync(string text);

        /// <summary>
        /// 在文本中查找所有的关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回所有关键词列表。</returns>
        List<string> FindAll(string text);

        /// <summary>
        /// 在文本中查找所有的关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回所有关键词列表。</returns>
        Task<List<string>> FindAllAsync(string text);

        /// <summary>
        /// 是否为敏感词。
        /// </summary>
        /// <param name="word">敏感词。</param>
        /// <returns>返回判断结果。</returns>
        bool IsSensitivity(string word);

        /// <summary>
        /// 是否为敏感词。
        /// </summary>
        /// <param name="word">敏感词。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsSensitivityAsync(string word);
    }
}
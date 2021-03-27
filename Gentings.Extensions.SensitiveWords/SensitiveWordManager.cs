using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Storages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 敏感词汇管理实现类。
    /// </summary>
    public class SensitiveWordManager : ISensitiveWordManager
    {
        /// <summary>
        /// 数据库上下文接口实例。
        /// </summary>
        public IDbContext<SensitiveWord> Context { get; }
        private readonly IStorageDirectory _storageDirectory;
        private readonly IMemoryCache _cache;
        private readonly Type _cacheKey = typeof(SensitiveWord);

        /// <summary>
        /// 初始化类<see cref="SensitiveWordManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="storageDirectory">存储文件夹接口实例。</param>
        /// <param name="cache">缓存接口。</param>
        public SensitiveWordManager(IDbContext<SensitiveWord> context, IStorageDirectory storageDirectory, IMemoryCache cache) 
        {
            Context = context;
            _storageDirectory = storageDirectory;
            _cache = cache;
        }

        /// <summary>
        /// 导入敏感词汇。
        /// </summary>
        /// <param name="words">敏感词汇列表。</param>
        /// <returns>返回导入结果。</returns>
        public virtual Task<bool> ImportAsync(IEnumerable<string> words)
        {
            return Context.BeginTransactionAsync(async db =>
            {
                foreach (var word in words)
                {
                    if (await db.AnyAsync(x => x.Word == word))
                        continue;
                    await db.CreateAsync(new SensitiveWord { Word = word });
                }

                return true;
            }, 600);
        }

        /// <summary>
        /// 通过文件导入敏感词汇。
        /// </summary>
        /// <param name="file">上传文件实例。</param>
        /// <returns>返回导入词汇数量。</returns>
        public virtual async Task<int> ImportAsync(IFormFile file)
        {
            var tempFile = await _storageDirectory.SaveToTempAsync(file);
            var text = await FileHelper.ReadTextAsync(tempFile.FullName);
            if (string.IsNullOrWhiteSpace(text))
                return -1;
            var words = text.Trim().Split(new[] { "\r\n", "\n", "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length < 32)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (await ImportAsync(words))
                return words.Count;
            return 0;
        }

        /// <summary>
        /// 获取字符串检索实例对象。
        /// </summary>
        /// <returns>返回字符串检索实例对象。</returns>
        protected virtual StringSearch GetSearchInstance()
        {
            return _cache.GetOrCreate(_cacheKey, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var search = new StringSearch();
                var words = Context.AsQueryable().WithNolock()
                    .Select(x => x.Word)
                    .AsEnumerable(reader => reader.GetString(0));
                search.SetKeywords(words);
                return search;
            });
        }

        /// <summary>
        /// 获取字符串检索实例对象。
        /// </summary>
        /// <returns>返回字符串检索实例对象。</returns>
        protected virtual Task<StringSearch> GetSearchInstanceAsync()
        {
            return _cache.GetOrCreateAsync(_cacheKey, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var search = new StringSearch();
                var words = await Context.AsQueryable().WithNolock()
                    .Select(x => x.Word)
                    .AsEnumerableAsync(reader => reader.GetString(0));
                search.SetKeywords(words);
                return search;
            });
        }

        /// <summary>
        /// 是否包含敏感词。
        /// </summary>
        /// <param name="text">判断的字符串。</param>
        /// <returns>返回判断结果。</returns>
        public virtual bool Contains(string text) => GetSearchInstance().Contains(text);

        /// <summary>
        /// 是否包含敏感词。
        /// </summary>
        /// <param name="text">判断的字符串。</param>
        /// <returns>返回判断结果。</returns>
        public virtual async Task<bool> ContainsAsync(string text)
        {
            var search = await GetSearchInstanceAsync();
            return search.Contains(text);
        }

        /// <summary>
        /// 在文本中替换所有的关键字。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <param name="replaceChar">替换字符。</param>
        /// <returns>返回替换结果。</returns>
        public virtual string Replace(string text, char replaceChar = '*')
        {
            var search = GetSearchInstance();
            return search.Replace(text, replaceChar);
        }

        /// <summary>
        /// 在文本中替换所有的关键字。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <param name="replaceChar">替换字符。</param>
        /// <returns>返回替换结果。</returns>
        public virtual async Task<string> ReplaceAsync(string text, char replaceChar = '*')
        {
            var search = await GetSearchInstanceAsync();
            return search.Replace(text, replaceChar);
        }

        /// <summary>
        /// 在文本中查找第一个关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回第一个关键词。</returns>
        public virtual string FindFirst(string text)
        {
            var search = GetSearchInstance();
            return search.FindFirst(text);
        }

        /// <summary>
        /// 在文本中查找第一个关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回第一个关键词。</returns>
        public virtual async Task<string> FindFirstAsync(string text)
        {
            var search = await GetSearchInstanceAsync();
            return search.FindFirst(text);
        }

        /// <summary>
        /// 在文本中查找所有的关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回所有关键词列表。</returns>
        public virtual List<string> FindAll(string text)
        {
            var search = GetSearchInstance();
            return search.FindAll(text);
        }

        /// <summary>
        /// 在文本中查找所有的关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回所有关键词列表。</returns>
        public virtual async Task<List<string>> FindAllAsync(string text)
        {
            var search = await GetSearchInstanceAsync();
            return search.FindAll(text);
        }

        /// <summary>
        /// 是否为敏感词。
        /// </summary>
        /// <param name="word">敏感词。</param>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsSensitivity(string word)
        {
            return Context.Any(x => x.Word.Contains(word));
        }

        /// <summary>
        /// 是否为敏感词。
        /// </summary>
        /// <param name="word">敏感词。</param>
        /// <returns>返回判断结果。</returns>
        public virtual Task<bool> IsSensitivityAsync(string word)
        {
            return Context.AnyAsync(x => x.Word.Contains(word));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Storages;
using Microsoft.AspNetCore.Http;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 敏感词汇管理实现类。
    /// </summary>
    public class SensitiveWordManager : ObjectManager<SensitiveWord>, ISensitiveWordManager
    {
        private readonly IStorageDirectory _storageDirectory;

        /// <summary>
        /// 初始化类<see cref="SensitiveWordManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="storageDirectory">存储文件夹接口实例。</param>
        public SensitiveWordManager(IDbContext<SensitiveWord> context, IStorageDirectory storageDirectory) : base(context)
        {
            _storageDirectory = storageDirectory;
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
        public async Task<int> ImportAsync(IFormFile file)
        {
            var tempFile = await _storageDirectory.SaveToTempAsync(file);
            var text = await FileHelper.ReadTextAsync(tempFile.FullName);
            if (string.IsNullOrWhiteSpace(text))
                return -1;
            var words = text.Trim().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length < 32)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (await ImportAsync(words))
                return words.Count;
            return 0;
        }
    }
}
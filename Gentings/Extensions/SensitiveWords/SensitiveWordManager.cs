using System.Collections.Generic;
using System.Threading.Tasks;
using Gentings.Data;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 敏感词汇管理实现类。
    /// </summary>
    public class SensitiveWordManager : ObjectManager<SensitiveWord>, ISensitiveWordManager
    {
        /// <summary>
        /// 初始化类<see cref="SensitiveWordManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        public SensitiveWordManager(IDbContext<SensitiveWord> context) : base(context)
        {
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
    }
}
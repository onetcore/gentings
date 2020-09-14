using Gentings.Data;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 敏感词汇查询。
    /// </summary>
    public class SensitiveWordQuery : QueryBase<SensitiveWord>
    {
        /// <summary>
        /// 敏感词。
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected internal override void Init(IQueryContext<SensitiveWord> context)
        {
            context.WithNolock();
            if (!string.IsNullOrEmpty(Word))
                context.Where(x => x.Word.Contains(Word));
        }
    }
}
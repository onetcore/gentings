using Markdig.Parsers;
using Markdig.Syntax;

namespace Gentings.Documents.Markdown.Extensions.QuoteSectionNotes
{
    /// <summary>
    /// 引用节点通知代码块。
    /// </summary>
    public class QuoteSectionNoteBlock : ContainerBlock
    {
        /// <summary>
        /// 初始化类<see cref="QuoteSectionNoteBlock"/>。
        /// </summary>
        /// <param name="parser">代码块解析器。</param>
        public QuoteSectionNoteBlock(BlockParser parser) : base(parser)
        {
        }

        /// <summary>
        /// 引用起始字符。
        /// </summary>
        public char QuoteChar { get; set; }

        /// <summary>
        /// 引用类型。
        /// </summary>
        public QuoteSectionNoteType QuoteType { get; set; }

        /// <summary>
        /// 节点属性字符串。
        /// </summary>
        public string SectionAttributeString { get; set; }

        /// <summary>
        /// 通知类型字符串。
        /// </summary>
        public string NoteTypeString { get; set; }

        /// <summary>
        /// 视频链接地址。
        /// </summary>
        public string VideoLink { get; set; }
    }
}

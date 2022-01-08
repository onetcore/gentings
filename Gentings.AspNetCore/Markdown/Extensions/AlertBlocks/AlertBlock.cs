using Markdig.Parsers;
using Markdig.Syntax;

namespace Gentings.AspNetCore.Markdown.Extensions.AlertBlocks
{
    /// <summary>
    /// 警告代码块。
    /// </summary>
    public class AlertBlock : ContainerBlock
    {
        /// <summary>
        /// 初始化类<see cref="AlertBlock"/>。
        /// </summary>
        /// <param name="parser">代码块解析器。</param>
        public AlertBlock(BlockParser? parser) : base(parser)
        {
        }

        /// <summary>
        /// 起始字符。
        /// </summary>
        public char OpeningCharacter { get; set; }

        /// <summary>
        /// Alert语法实例。
        /// </summary>
        public AlertSyntax.SyntaxEntry Syntax { get; set; }

        /// <summary>
        /// 行数。
        /// </summary>
        public List<QuoteBlockLine> BlockLines { get; } = new();
    }
}

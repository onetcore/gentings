using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Gentings.Documents.Markdown.Extensions.QuoteSectionNotes
{
    /// <summary>
    /// 引用节点通知解析器。
    /// </summary>
    public class QuoteSectionNoteParser : BlockParser
    {
        /// <summary>
        /// 初始化类<see cref="QuoteSectionNoteParser"/>。
        /// </summary>
        public QuoteSectionNoteParser()
        {
            OpeningCharacters = new[] { '>' };
        }

        /// <summary>
        /// 尝试解析当前字符串。
        /// </summary>
        /// <param name="processor">代码块进程。</param>
        /// <returns>返回代码块状态。</returns>
        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            var column = processor.Column;
            var sourcePosition = processor.Start;

            var quoteChar = processor.CurrentChar;
            var c = processor.NextChar();
            if (c.IsSpaceOrTab())
            {
                processor.NextColumn();
            }

            var rawNewBlock = new QuoteSectionNoteBlock(this)
            {
                Line = processor.LineIndex,
                QuoteChar = quoteChar,
                Column = column,
                Span = new SourceSpan(sourcePosition, processor.Line.End),
            };
            TryParseFromLine(processor, rawNewBlock);
            processor.NewBlocks.Push(rawNewBlock);

            if (rawNewBlock.QuoteType == QuoteSectionNoteType.DFMVideo)
            {
                return BlockState.BreakDiscard;
            }
            else
            {
                return BlockState.Continue;
            }
        }

        /// <summary>
        /// 继续解析全部代码块。
        /// </summary>
        /// <param name="processor">代码块进程。</param>
        /// <param name="block">当前块实例对象。</param>
        /// <returns>返回读取的状态。</returns>
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            var quote = (QuoteSectionNoteBlock)block;
            var column = processor.Column;

            if (quote.QuoteType == QuoteSectionNoteType.DFMVideo)
            {
                return BlockState.BreakDiscard;
            }

            var c = processor.CurrentChar;
            if (c != quote.QuoteChar)
            {
                return processor.IsBlankLine ? BlockState.BreakDiscard : BlockState.None;
            }

            c = processor.NextChar(); // Skip opening char
            if (c.IsSpace())
            {
                processor.NextChar(); // Skip following space
            }

            // Check for New DFM block
            if (TryParseFromLine(processor, new QuoteSectionNoteBlock(this)))
            {
                // Meet note or section, close this block, new block will be open in the next steps
                processor.GoToColumn(column);
                return BlockState.None;
            }
            else
            {
                block.UpdateSpanEnd(processor.Line.End);
                return BlockState.Continue;
            }
        }

        private bool TryParseFromLine(BlockProcessor processor, QuoteSectionNoteBlock block)
        {
            int originalColumn = processor.Column;
            block.QuoteType = QuoteSectionNoteType.MarkdownQuote;

            if (processor.CurrentChar != '[')
            {
                return false;
            }

            var stringBuilder = StringBuilderCache.Local();
            var c = processor.CurrentChar;

            var hasEscape = false;
            while (c != '\0' && (c != ']' || hasEscape))
            {
                if (c == '\\' && !hasEscape)
                {
                    hasEscape = true;
                }
                else
                {
                    stringBuilder.Append(c);
                    hasEscape = false;
                }
                c = processor.NextChar();
            }

            stringBuilder.Append(c);
            var infoString = stringBuilder.ToString().Trim();

            if (c == '\0')
            {
                processor.GoToColumn(originalColumn);
                return false;
            }

            if (c == ']')
            {
                // "> [!NOTE] content" is invalid, go to end to see it.
                processor.NextChar();
                while (processor.CurrentChar.IsSpaceOrTab()) processor.NextChar();
                var isNoteVideoDiv = infoString.StartsWith("[!div", StringComparison.OrdinalIgnoreCase) ||
                                     infoString.StartsWith("[!Video", StringComparison.OrdinalIgnoreCase) ||
                                     SectionNoteType.IsNoteType(infoString);
                if (processor.CurrentChar != '\0' && isNoteVideoDiv)
                {
                    processor.GoToColumn(originalColumn);
                    return false;
                }
            }

            if (SectionNoteType.IsNoteType(infoString))
            {
                block.QuoteType = QuoteSectionNoteType.DFMNote;
                block.NoteTypeString = infoString.Substring(2, infoString.Length - 3).ToLowerInvariant();
                return true;
            }

            if (infoString.StartsWith("[!div", StringComparison.OrdinalIgnoreCase))
            {
                block.QuoteType = QuoteSectionNoteType.DFMSection;
                string attribute = infoString.Substring(5, infoString.Length - 6).Trim();
                if (attribute.Length >= 2 && attribute.First() == '`' && attribute.Last() == '`')
                {
                    block.SectionAttributeString = attribute.Substring(1, attribute.Length - 2).Trim();
                }
                if (attribute.Length >= 1 && attribute.First() != '`' && attribute.Last() != '`')
                {
                    block.SectionAttributeString = attribute;
                }
                return true;
            }

            if (infoString.StartsWith("[!Video", StringComparison.OrdinalIgnoreCase))
            {
                string link = infoString.Substring(7, infoString.Length - 8);
                if (link.StartsWith(" http://") || link.StartsWith(" https://"))
                {
                    block.QuoteType = QuoteSectionNoteType.DFMVideo;
                    block.VideoLink = link.Trim();
                    return true;
                }
            }

            processor.GoToColumn(originalColumn);
            return false;
        }
    }
}

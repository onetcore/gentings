using Gentings.Documents.Markdown;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Gentings.Documents.Markdown.Extensions.AlertBlocks
{
    /// <summary>
    /// 语法解析器：
    /// <code>
    /// > [warning]{"警告标题"}
    /// > 内容，这个可以书写内容。
    /// </code>
    /// </summary>
    public class AlertBlockParser : BlockParser
    {
        /// <summary>
        /// 初始化类<see cref="AlertBlockParser"/>。
        /// </summary>
        public AlertBlockParser()
        {
            OpeningCharacters = new[] { '>' };
        }

        /// <summary>
        /// 尝试匹配代码块。
        /// </summary>
        /// <param name="processor">代码块操作实例。</param>
        /// <returns>返回当前代码块状态。</returns>
        public override BlockState TryOpen(BlockProcessor processor)
        {
            //"> ["开头
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            var current = processor.GetStartsWith(true, AlertSyntax.Syntaxs);
            if (current == null)
            {
                return BlockState.None;
            }

            var sourcePosition = processor.Start;

            var quoteChar = processor.CurrentChar;
            var column = processor.Column;
            processor.SkipChars(current.Length);
            var c = processor.NextChar(); // Skip quote marker char
            if (c == ' ')
            {
                processor.NextColumn();
                processor.SkipFirstUnwindSpace = true;
            }
            else if (c == '\t')
            {
                processor.NextColumn();
            }
            var alertBlock = new AlertBlock(this)
            {
                OpeningCharacter = quoteChar,
                Column = column,
                Syntax = AlertSyntax.GetSyntax(current)!,
                Span = new SourceSpan(sourcePosition, processor.Line.End),
                LinesBefore = processor.LinesBefore,
                NewLine = processor.Line.NewLine,
            };
            processor.LinesBefore = null;
            processor.NewBlocks.Push(alertBlock);
            return BlockState.Continue;
        }

        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            var quote = (AlertBlock)block;
            var sourcePosition = processor.Start;

            // 5.1 Block quotes 
            // A block quote marker consists of 0-3 spaces of initial indent, plus (a) the character > together with a following space, or (b) a single character > not followed by a space.
            var c = processor.CurrentChar;
            bool hasSpaceAfterQuoteChar = false;
            if (c != quote.OpeningCharacter)
            {
                if (processor.IsBlankLine)
                {
                    return BlockState.BreakDiscard;
                }
                else
                {
                    quote.BlockLines.Add(new QuoteBlockLine
                    {
                        QuoteChar = false,
                        NewLine = processor.Line.NewLine,
                    });
                    return BlockState.None;
                }
            }
            c = processor.NextChar(); // Skip quote marker char
            if (c == ' ')
            {
                processor.NextColumn();
                hasSpaceAfterQuoteChar = true;
                processor.SkipFirstUnwindSpace = true;
            }
            else if (c == '\t')
            {
                processor.NextColumn();
            }
            var TriviaSpaceBefore = processor.UseTrivia(sourcePosition - 1);
            StringSlice triviaAfter = StringSlice.Empty;
            bool wasEmptyLine = false;
            if (processor.Line.IsEmptyOrWhitespace())
            {
                processor.TriviaStart = processor.Start;
                triviaAfter = processor.UseTrivia(processor.Line.End);
                wasEmptyLine = true;
            }
            quote.BlockLines.Add(new QuoteBlockLine
            {
                QuoteChar = true,
                HasSpaceAfterQuoteChar = hasSpaceAfterQuoteChar,
                TriviaBefore = TriviaSpaceBefore,
                TriviaAfter = triviaAfter,
                NewLine = processor.Line.NewLine,
            });

            if (!wasEmptyLine)
            {
                processor.TriviaStart = processor.Start;
            }
            block.UpdateSpanEnd(processor.Line.End);
            return BlockState.Continue;
        }
    }
}

using Markdig.Helpers;
using Markdig.Parsers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Gentings.Documents.Markdown
{
    /// <summary>
    /// Markdown扩展类。
    /// </summary>
    public static class MarkdownExtensions
    {
        /// <summary>
        /// 截取字符串。
        /// </summary>
        /// <param name="slice">当前字符串片段。</param>
        /// <param name="end">截止字符。</param>
        /// <returns>返回当前截取的字符串。</returns>
        public static string Substring(this StringSlice slice, char end)
        {
            var builder = new StringBuilder();
            var current = slice.CurrentChar;
            while (current != '\0')
            {
                if (current == end)
                {
                    return builder.ToString();
                }
                builder.Append(current);
                current = slice.NextChar();
            }
            return builder.ToString();
        }

        /// <summary>
        /// 截取字符串。
        /// </summary>
        /// <param name="slice">当前字符串片段。</param>
        /// <param name="start">开始偏移量。</param>
        /// <returns>返回当前截取的字符串。</returns>
        public static string Substring(this StringSlice slice, int start)
        {
            var text = slice.Text;
            start = slice.Start + start;
            var length = slice.End - start + 1;
            if (text == null || length <= 0)
            {
                return string.Empty;
            }

            return text.Substring(start, length);
        }

        /// <summary>
        /// 截取非空字符串，直到遇到<paramref name="end"/>字符，并且忽略空白字符。
        /// </summary>
        /// <param name="processor">代码块分析器。</param>
        /// <param name="end">截止字符。</param>
        /// <returns>返回非空字符串。</returns>
        public static string Substring(this BlockProcessor processor, char end)
        {
            var start = processor.Start;
            var current = processor.CurrentChar;
            var builder = new StringBuilder();
            while (current != '\0')
            {
                if (current == end)
                {
                    return builder.ToString();
                }
                if (!current.IsWhiteSpaceOrZero())
                {
                    builder.Append(current);
                }
                current = processor.NextChar();
            }
            processor.Line.Start = start;
            return null;
        }

        /// <summary>
        /// 判断当前行是以<paramref name="start"/>开头。
        /// </summary>
        /// <param name="processor">代码块分析器。</param>
        /// <param name="start">开始字符串。</param>
        /// <returns>返回判断结果。</returns>
        public static bool StartsWith(this BlockProcessor processor, string start, bool ignoreCase = true)
        {
            if (processor.Line.Length < start.Length) return false;
            if (ignoreCase)
            {
                start = start.ToLowerInvariant();
                for (var i = 0; i < start.Length; i++)
                {
                    if (char.ToLowerInvariant(processor.Line.PeekChar(i)) != start[i])
                        return false;
                }
            }
            else
            {
                for (var i = 0; i < start.Length; i++)
                {
                    if (processor.Line.PeekChar(i) != start[i])
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断当前行是以<paramref name="starts"/>任何一个开头，如果是返回当前开头的字符串，否则返回<c>null</c>。
        /// 注意：判定列表为参数列表顺序，如果比较长的字符串要在前面。
        /// </summary>
        /// <param name="processor">代码块分析器。</param>
        /// <param name="ignoreCase">忽略大小写。</param>
        /// <param name="starts">开始字符串列表。</param>
        /// <returns>返回判断结果，如果不存在返回<c>null</c>。</returns>
        public static string GetStartsWith(this BlockProcessor processor, bool ignoreCase, params string[] starts)
        {
            foreach (var start in starts)
            {
                if (processor.StartsWith(start, ignoreCase))
                    return start;
            }
            return null;
        }

        /// <summary>
        /// 跳过偏移量。
        /// </summary>
        /// <param name="processor">代码块分析器。</param>
        /// <param name="offset">偏移量。</param>
        public static void SkipChars(this BlockProcessor processor, int offset)
        {
            for (var i = 0; i < offset; i++)
            {
                processor.Line.SkipChar();
            }
        }
    }
}

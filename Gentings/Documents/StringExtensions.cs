using System.Text;

namespace Gentings.Documents
{
    /// <summary>
    /// 字符串扩展类。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 读取直到遇到<paramref name="end"/>字符。
        /// </summary>
        /// <param name="source">源代码。</param>
        /// <param name="index">当前字符索引。</param>
        /// <param name="end">字符。</param>
        /// <returns>返回当前读取到的字符串。</returns>
        public static string ReadWhile(this string source, ref int index, char end)
        {
            var builder = new StringBuilder();
            while (index < source.Length)
            {
                var current = source[index];
                index++;
                if (end == current)
                    return builder.ToString();
                builder.Append(current);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 读取直到遇到<paramref name="ends"/>的字符。
        /// </summary>
        /// <param name="source">源代码。</param>
        /// <param name="index">当前字符索引。</param>
        /// <param name="ends">分隔符。</param>
        /// <returns>返回当前读取到的字符串。</returns>
        public static (string Current, char End) ReadWhile(this string source, ref int index, params char[] ends)
        {
            char current;
            var builder = new StringBuilder();
            while (index < source.Length)
            {
                current = source[index];
                index++;
                if (ends.Any(x => x == current))
                    return (builder.ToString(), current);
                builder.Append(current);
            }

            return (builder.ToString(), '\0');
        }

        /// <summary>
        /// 跳过空白字符。
        /// </summary>
        /// <param name="source">源代码。</param>
        /// <param name="index">当前字符索引。</param>
        public static void EscapeWhiteSpace(this string source, ref int index)
        {
            while (index < source.Length)
            {
                var current = source[index];
                if (!char.IsWhiteSpace(current))
                    break;
                index++;
            }
        }

        /// <summary>
        /// 截图字符串。
        /// </summary>
        /// <param name="source">原字符串。</param>
        /// <param name="start">开始字符串，结果不包含此字符串。</param>
        /// <param name="end">结束字符串，结果不包含此字符串。</param>
        /// <param name="comparison">对比模式。</param>
        /// <returns>返回截取得到的字符串。</returns>
        public static string? Substring(this string source, string start, string? end = null, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var index = source.IndexOf(start, comparison);
            if (index == -1)
            {
                return null;
            }

            source = source[(index + start.Length)..];
            if (end != null)
            {
                index = source.IndexOf(end, comparison);
                if (index == -1)
                {
                    return null;
                }

                source = source[..index];
            }

            return source.Trim();
        }
    }
}
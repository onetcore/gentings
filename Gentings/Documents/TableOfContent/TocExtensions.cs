namespace Gentings.Documents.TableOfContent
{
    /// <summary>
    /// Toc扩展类。
    /// </summary>
    internal static class TocExtensions
    {
        /// <summary>
        /// 获取缩进长度。
        /// </summary>
        /// <param name="line">当前字符串。</param>
        /// <returns>返回缩进长度。</returns>
        public static int GetIndent(this string line)
        {
            var indent = 0;
            while (char.IsWhiteSpace(line[indent])) indent++;
            return indent;
        }

        /// <summary>
        /// 读取下一行。
        /// </summary>
        /// <param name="reader">当前字符串读取实例。</param>
        /// <param name="line">当前行数。</param>
        /// <returns>返回读取的实例。</returns>
        public static TocString ReadNext(this StringReader reader, ref int line)
        {
            return new TocString(ref line, reader.ReadLine());
        }

        /// <summary>
        /// 读取下一行字符串，过滤注释和空行。
        /// </summary>
        /// <param name="reader">当前字符串读取实例。</param>
        /// <param name="line">当前行数。</param>
        /// <returns>返回读取的实例。</returns>
        public static TocString ReadNextString(this StringReader reader, ref int line)
        {
            var slice = reader.ReadNext(ref line);
            if (slice.IsEnd || !slice.IsEmptyOrComment) return slice;
            return reader.ReadNextString(ref line);
        }
    }
}

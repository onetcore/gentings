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

        /// <summary>
        /// 移除“/index.md”或者文件“.md”后缀名。
        /// </summary>
        /// <param name="href">当前链接地址。</param>
        /// <returns>返回移除后的地址。</returns>
        public static string TrimEndLowerCaseMD(this string href)
        {
            href = href.ToLower();
            if (href.EndsWith('/'))
                href = href.TrimEnd('/');
            if (href.EndsWith(".md"))
                href = href[0..^3];
            if (href.EndsWith("/index"))
                href = href[0..^6];
            return href;
        }
    }
}

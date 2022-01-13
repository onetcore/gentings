using Gentings.Properties;
using Gentings.Storages;
using System.Collections;
using System.Text;

namespace Gentings.Documents.TableOfContent
{
    /// <summary>
    /// 简单的目录Yaml结构解析，为了兼容docfx。
    /// </summary>
    public class Toc : IEnumerable<TocItem>
    {
        /// <summary>
        /// 文件夹名称。
        /// </summary>
        public string DirectoryName { get; private set; }

        private readonly List<TocItem> _items = new List<TocItem>();

        /// <summary>
        /// 获取正确的URL地址。
        /// </summary>
        /// <param name="href">Markdown链接地址。</param>
        /// <returns>返回挣钱的URL地址。</returns>
        public string SafeUrl(string href)
        {
            if (string.IsNullOrEmpty(href)) return null;
            href = href.Trim();
            if (href.StartsWith("http:", StringComparison.OrdinalIgnoreCase) ||
                href.StartsWith("https:", StringComparison.OrdinalIgnoreCase) ||
                href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                return href;
            href = Path.GetFullPath(Path.Join(DirectoryName, href));
            href = href.Substring(2).Replace('\\', '/');
            href = href.ToLower();
            if (href.EndsWith(".md"))
                href = href.Substring(0, href.Length - 3);
            if (href.EndsWith("/index"))
                href = href.Substring(0, href.Length - 6);
            return href;
        }

        private void Init(string source)
        {
            var line = 1;
            TocItem item = null;
            using var reader = new StringReader(source);
            var slice = reader.ReadNextString(ref line);
            while (!slice.IsEnd)
            {
                if (slice.IsStruct)
                {
                    item = new TocItem(this);
                    item.Indent = slice.Indent;
                    item.Index = slice.Index;
                    item.Line = slice.Line;
                    item.Init(slice);
                    _items.Add(item);
                    slice = reader.ReadNext(ref line);
                    continue;
                }
                if (slice.Name == "items")
                {
                    slice = ReadChildren(item, reader, ref line);
                    continue;
                }
                else
                {
                    item.Init(slice);
                }
                slice = reader.ReadNextString(ref line);
            }
        }

        private TocString ReadChildren(TocItem item, StringReader reader, ref int line)
        {
            var slice = reader.ReadNextString(ref line);
            if (slice.Indent <= item.Indent) throw new Exception(Resources.TocItemsInvalid);
            TocItem current = null;
            while (!slice.IsEnd)
            {
                if (slice.Indent <= item.Indent)
                    return slice;
                else if (slice.IsStruct)
                {
                    current = new TocItem(this);
                    current.Indent = slice.Indent;
                    current.Index = slice.Index;
                    current.Line = slice.Line;
                    current.Init(slice);
                    current.Parent = item;
                    item.Items.Add(current);
                }
                else if (slice.Name == "items")
                {
                    slice = ReadChildren(current, reader, ref line);
                    continue;
                }
                else if (slice.Indent == current.Index)
                {
                    current.Init(slice);
                }
                slice = reader.ReadNextString(ref line);
            }
            return slice;
        }

        /// <summary>
        /// 迭代器。
        /// </summary>
        /// <returns>返回迭代器实例。</returns>
        public IEnumerator<TocItem> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// 呈现Toc字符串。
        /// </summary>
        /// <returns>呈现Toc字符串。</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var item in _items)
            {
                builder.Append(item.ToString());
            }
            return builder.ToString();
        }

        /// <summary>
        /// 加载文件并实例化。
        /// </summary>
        /// <param name="path">当前toc文件物理路径。</param>
        /// <param name="culture">语言。</param>
        /// <returns>返回<see cref="Toc"/>实例对象。</returns>
        public void Load(string path, string culture)
        {
            DirectoryName = TocPath.GetUrlRoot(culture, path);
            var source = FileHelper.ReadText(path);
            Init(source);
        }

        /// <summary>
        /// 加载文件并实例化。
        /// </summary>
        /// <param name="path">当前toc文件物理路径。</param>
        /// <param name="culture">语言。</param>
        /// <returns>返回<see cref="Toc"/>实例对象。</returns>
        public async Task LoadAsync(string path, string culture)
        {
            DirectoryName = TocPath.GetUrlRoot(culture, path);
            var source = await FileHelper.ReadTextAsync(path);
            Init(source);
        }

        /// <summary>
        /// 通过链接获取<see cref="TocItem"/>实例。
        /// </summary>
        /// <param name="href">链接地址。</param>
        /// <returns>返回<see cref="TocItem"/>实例。</returns>
        public TocItem GetByHref(string href)
        {
            href = href.TrimEnd('/', '\\');
            foreach (var item in _items)
            {
                var search = Search(item, x => x.Href?.Equals(href, StringComparison.OrdinalIgnoreCase) == true);
                if (search != null)
                    return search;
            }
            return null;
        }

        private TocItem Search(TocItem item, Predicate<TocItem> predicate)
        {
            if (predicate(item)) return item;
            foreach (var child in item.Items)
            {
                var result = Search(child, predicate);
                if (result != null) return result;
            }
            return null;
        }
    }
}

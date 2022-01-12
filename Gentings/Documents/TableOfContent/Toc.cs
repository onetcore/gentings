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
        private readonly List<TocItem> _items = new List<TocItem>();

        private Toc(string source)
        {
            var line = 1;
            TocItem item = null;
            using var reader = new StringReader(source);
            var slice = reader.ReadNextString(ref line);
            while (!slice.IsEnd)
            {
                if (slice.IsStruct)
                {
                    item = new TocItem();
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
                    current = new TocItem();
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
        /// <returns>返回<see cref="Toc"/>实例对象。</returns>
        public static Toc Load(string path)
        {
            var source = FileHelper.ReadText(path);
            return new Toc(source);
        }

        /// <summary>
        /// 加载文件并实例化。
        /// </summary>
        /// <param name="path">当前toc文件物理路径。</param>
        /// <returns>返回<see cref="Toc"/>实例对象。</returns>
        public static async Task<Toc> LoadAsync(string path)
        {
            var source = await FileHelper.ReadTextAsync(path);
            return new Toc(source);
        }

        /// <summary>
        /// 加载YAML并实例化。
        /// </summary>
        /// <param name="yaml">YAML字符串。</param>
        /// <returns>返回<see cref="Toc"/>实例对象。</returns>
        public static Toc LoadYaml(string yaml)
        {
            return new Toc(yaml);
        }

        /// <summary>
        /// 通过链接获取<see cref="TocItem"/>实例。
        /// </summary>
        /// <param name="href">链接地址。</param>
        /// <returns>返回<see cref="TocItem"/>实例。</returns>
        public TocItem GetByHref(string href)
        {
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

using System.Text;

namespace Gentings.Documents.TableOfContent
{
    /// <summary>
    /// 目录项。
    /// </summary>
    public class TocItem
    {
        internal TocItem(Toc toc)
        {
            Toc = toc;
        }

        /// <summary>
        /// 缩进。
        /// </summary>
        public int Indent { get; internal set; }

        /// <summary>
        /// 当前结构属性缩进字符数。
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// 所在文件中的行数。
        /// </summary>
        public int Line { get; internal set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 链接地址。
        /// </summary>
        public string Href { get; private set; }

        /// <summary>
        /// 唯一Id。
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// 菜单的唯一Id。
        /// </summary>
        public string Id => $"L{Line}";

        /// <summary>
        /// 父级实例。
        /// </summary>
        public TocItem Parent { get; internal set; }

        /// <summary>
        /// 子项数组。
        /// </summary>
        public List<TocItem> Items { get; } = new List<TocItem>();

        /// <summary>
        /// 当前Toc实例对象。
        /// </summary>
        public Toc Toc { get; }

        private readonly Dictionary<string, string> _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 获取其他属性值。
        /// </summary>
        /// <param name="key">其他属性健。</param>
        /// <returns>返回当前配置的值，如果不存在则返回<c>null</c>。</returns>
        public string this[string key]
        {
            get
            {
                if (_items.TryGetValue(key, out var value))
                    return value;
                return null;
            }
        }

        internal void Init(TocString toc)
        {
            switch (toc.Name)
            {
                case "name":
                    Name = toc.Value;
                    break;
                case "href":
                    Href = GetSafeUrl(toc.Value);
                    break;
                case "uid":
                    Uid = toc.Value;
                    break;
                default:
                    _items[toc.Name] = toc.Value;
                    break;
            }
        }

        private string GetSafeUrl(string href)
        {
            href = Path.GetFullPath(Path.Join(Toc.DirectoryName ?? "/docs", href));
            href = href.Substring(2).Replace('\\', '/');
            href = href.ToLower();
            if (href.EndsWith(".md"))
                href = href.Substring(0, href.Length - 3);
            if (href.EndsWith("/index"))
                href = href.Substring(0, href.Length - 6);
            return href;
        }

        /// <summary>
        /// 输出字符串。
        /// </summary>
        /// <returns>当前输出字符串。</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(new string(' ', Indent))
                .Append('-')
                .Append(new string(' ', Index - Indent - 1))
                .Append("name: ")
                .AppendLine(Name);
            if (Href != null)
                builder.Append(new string(' ', Index)).Append("href: ").AppendLine(Href);
            if (Uid != null)
                builder.Append(new string(' ', Index)).Append("href: ").AppendLine(Uid);
            foreach (var item in _items)
                builder.Append(new string(' ', Index)).Append(item.Key).Append(": ").AppendLine(item.Value);
            if (Items.Count > 0)
            {
                builder.Append(new string(' ', Index + 2)).AppendLine("items:");
                foreach (var item in Items)
                {
                    builder.Append(item);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 判断<paramref name="item"/>是否为当前项的父项。
        /// </summary>
        /// <param name="item">当前项目实例。</param>
        /// <returns>返回判断结果。</returns>
        public bool IsCurrent(TocItem item)
        {
            var current = this;
            while (current != null)
            {
                if (item == current)
                    return true;
                current = current.Parent;
            }
            return false;
        }
    }
}

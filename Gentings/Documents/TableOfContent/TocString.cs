using Gentings.Properties;

namespace Gentings.Documents.TableOfContent
{
    /// <summary>
    /// Toc键值对字符串。
    /// </summary>
    public class TocString
    {
        private readonly string? _source;
        internal TocString(ref int line, string? source)
        {
            IsEnd = source == null;
            if (IsEnd) return;
            Line = ++line;
            _source = source;
            if (string.IsNullOrWhiteSpace(source))
            {
                IsEmptyOrComment = true;
                return;
            }
            Indent = source.GetIndent();
            Index = Indent;
            switch (source[Indent])
            {
                case '#':
                    IsEmptyOrComment = true;
                    return;
                case '-':
                    IsStruct = true;
                    source = source[(Indent + 1)..];
                    Index = Indent + 1 + source.GetIndent();
                    break;
            }
            var index = source.IndexOf(':');
            if (index != -1)
            {
                Name = source[..index].Trim().ToLowerInvariant();
                Value = source[(index + 1)..].Trim();
            }
            else
            {
                throw new Exception(string.Format(Resources.TocItemInvalid, Line, _source));
            }
        }

        /// <summary>
        /// 是否结束实例。
        /// </summary>
        public bool IsEnd { get; }

        /// <summary>
        /// 是否为空或者注释。
        /// </summary>
        public bool IsEmptyOrComment { get; }

        /// <summary>
        /// 是否为结构开始。
        /// </summary>
        public bool IsStruct { get; }

        /// <summary>
        /// 是否为数组项。
        /// </summary>
        public bool IsItems { get; }

        /// <summary>
        /// 当前结构缩进字符数。
        /// </summary>
        public int Indent { get; }

        /// <summary>
        /// 当前结构属性缩进字符数。
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 所在文件中的行数。
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// 值。
        /// </summary>
        public string? Value { get; }

        /// <summary>
        /// 源码字符串。
        /// </summary>
        /// <returns>源码字符串。</returns>
        public override string? ToString()
        {
            return _source;
        }
    }
}

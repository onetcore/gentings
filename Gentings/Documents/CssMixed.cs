using System.Collections;

namespace Gentings.Documents
{
    /// <summary>
    /// 样式混淆简化类。
    /// </summary>
    public class CssSyntaxMixed
    {
        private readonly Style _style;
        /// <summary>
        /// 初始化类<see cref="CssSyntaxMixed"/>。
        /// </summary>
        /// <param name="source">样式源代码。</param>
        public CssSyntaxMixed(string source)
        {
            _style = new Style(source);
        }

        /// <summary>
        /// 样式字符串。
        /// </summary>
        /// <returns>返回样式字符串。</returns>
        public override string ToString()
        {
            return _style.ToString();
        }

        /// <summary>
        /// 样式实例。
        /// </summary>
        private class Style : IEnumerable<Syntax>
        {
            public Style(string source)
            {
                var index = 0;
                while (index < source.Length)
                {
                    var syntax = new Syntax(source, ref index);
                    _styles.Add(syntax);
                }
            }

            private readonly List<Syntax> _styles = new List<Syntax>();

            /// <summary>
            /// 获取迭代器实例。
            /// </summary>
            /// <returns>迭代器实例。</returns>
            public IEnumerator<Syntax> GetEnumerator() => _styles.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// 样式字符串。
            /// </summary>
            /// <returns>返回样式字符串。</returns>
            public override string ToString()
            {
                return _styles.Join(" ");
            }
        }

        /// <summary>
        /// CSS语法。
        /// </summary>
        private class Syntax : IEnumerable<NameValue>
        {
            public Syntax(string source, ref int index)
            {
                var selectors = source.ReadWhile(ref index, '{');
                source.EscapeWhiteSpace(ref index);
                Selector = selectors.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Join();
                while (index < source.Length)
                {
                    if (source[index] == '}')
                    {
                        index++;
                        break;
                    }
                    _values.Add(new NameValue(source, ref index));
                }
            }

            /// <summary>
            /// 选择器。
            /// </summary>
            public string Selector { get; }

            private readonly List<NameValue> _values = new List<NameValue>();

            /// <summary>
            /// 获取迭代器实例。
            /// </summary>
            /// <returns>迭代器实例。</returns>
            public IEnumerator<NameValue> GetEnumerator() => _values.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// 样式字符串。
            /// </summary>
            /// <returns>返回样式字符串。</returns>
            public override string ToString()
            {
                return $"{Selector}{{{_values.Join(string.Empty)}}}";
            }
        }

        /// <summary>
        /// 名称和值对。
        /// </summary>
        private class NameValue
        {
            public NameValue(string source, ref int index)
            {
                Name = source.ReadWhile(ref index, ':').Trim();
                source.EscapeWhiteSpace(ref index);
                Value = source.ReadWhile(ref index, ';', '}').Current;
                source.EscapeWhiteSpace(ref index);
            }

            /// <summary>
            /// 名称。
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// 值。
            /// </summary>
            public string Value { get; }

            /// <summary>
            /// 样式字符串。
            /// </summary>
            /// <returns>返回样式字符串。</returns>
            public override string ToString()
            {
                return $"{Name}:{Value};";
            }
        }
    }
}
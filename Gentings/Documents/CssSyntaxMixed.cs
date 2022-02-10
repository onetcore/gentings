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
            RemoveComments(ref source);
            _style = new Style(source);
        }

        private void RemoveComments(ref string source)
        {
            // 移除注释
            var index = source.IndexOf("/*");
            while (index != -1)
            {
                var temp = source[index..];
                source = source[..index];
                index = temp.IndexOf("*/");
                if (index == -1) break;
                source += temp[(index + 2)..];
                index = source.IndexOf("/*");
            }
            source = source.Replace("\r\n", string.Empty).Replace("\n", string.Empty);
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
        /// 样式字符串。
        /// </summary>
        /// <param name="key">唯一键，样式前缀。</param>
        /// <returns>返回样式字符串。</returns>
        public string ToString(string key)
        {
            return _style.ToString(key);
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

            private readonly List<Syntax> _styles = new();

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

            /// <summary>
            /// 样式字符串。
            /// </summary>
            /// <param name="key">唯一键，样式前缀。</param>
            /// <returns>返回样式字符串。</returns>
            public string ToString(string key)
            {
                return _styles.Select(x => x.ToString(key)).Join(" ");
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
                Selectors = selectors.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray();
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
            public string[] Selectors { get; }

            private readonly List<NameValue> _values = new();

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
                return $"{Selectors.Join()}{{{_values.Join(string.Empty)}}}";
            }

            /// <summary>
            /// 样式字符串。
            /// </summary>
            /// <param name="key">唯一键，样式前缀。</param>
            /// <returns>返回样式字符串。</returns>
            public string ToString(string key)
            {
                var keys = new List<string>();
                foreach (var selector in Selectors)
                {
                    if (selector.IndexOf('&') != -1)
                        keys.Add(key.Replace("&", key));
                    else
                        keys.Add($"{key} {selector}");
                }
                if (keys.Count == 0)
                    keys.Add(key);
                return $"{keys.Join()}{{{_values.Join(string.Empty)}}}";
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
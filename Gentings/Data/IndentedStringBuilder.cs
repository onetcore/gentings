﻿using System.Text;

namespace Gentings.Data
{
    /// <summary>
    /// 缩进字符串组合类。
    /// </summary>
    public class IndentedStringBuilder
    {
        private byte _indent;
        private const byte IndentSize = 4;
        private bool _indentPending = true;
        private readonly StringBuilder _builder = new();

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="o">当前实例。</param>
        /// <param name="seperator">分隔符。</param>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder JoinAppend<T>(IEnumerable<T> o, string seperator = ",")
        {
            DoIndent();

            _builder.Append(string.Join(seperator, o));

            return this;
        }

        /// <summary>
        /// 添加实例，如果<paramref name="o"/>为空则不进行任何操作。
        /// </summary>
        /// <param name="o">当前实例。</param>
        /// <param name="format">格式化字符串。</param>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder AppendEx(object? o, string format)
        {
            if (string.IsNullOrWhiteSpace(o?.ToString()))
            {
                return this;
            }

            DoIndent();

            _builder.AppendFormat(format, o);

            return this;
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="o">当前实例。</param>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder Append(object? o)
        {
            DoIndent();

            _builder.Append(o);

            return this;
        }

        /// <summary>
        /// 添加空行。
        /// </summary>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder AppendLine()
        {
            AppendLine(string.Empty);

            return this;
        }

        /// <summary>
        /// 缩进添加行实例。
        /// </summary>
        /// <param name="o">当前实例。</param>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder AppendLine(object o)
        {
            var value = o.ToString();

            if (value != string.Empty)
            {
                DoIndent();
            }

            _builder.AppendLine(value);

            _indentPending = true;

            return this;
        }

        /// <summary>
        /// 增加缩进。
        /// </summary>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder IncrementIndent()
        {
            _indent++;
            return this;
        }

        /// <summary>
        /// 减少缩进。
        /// </summary>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder DecrementIndent()
        {
            if (_indent > 0)
            {
                _indent--;
            }

            return this;
        }

        /// <summary>
        /// 增加缩进块。
        /// </summary>
        /// <returns>返回当前块实例，使用using将自动释放。</returns>
        public virtual IDisposable Indent()
        {
            return new Indenter(this);
        }

        /// <summary>
        /// 获取当前字符串实例。
        /// </summary>
        /// <returns>返回当前字符串内容实例。</returns>
        public override string ToString()
        {
            return _builder.ToString();
        }

        private void DoIndent()
        {
            if (_indentPending && _indent > 0)
            {
                _builder.Append(new string(' ', _indent * IndentSize));
            }

            _indentPending = false;
        }

        private sealed class Indenter : IDisposable
        {
            private readonly IndentedStringBuilder _stringBuilder;

            public Indenter(IndentedStringBuilder stringBuilder)
            {
                _stringBuilder = stringBuilder;

                _stringBuilder.IncrementIndent();
            }

            public void Dispose()
            {
                _stringBuilder.DecrementIndent();
            }
        }

        /// <summary>
        /// 附加“IS NULL”或者“IS NOT NULL”。
        /// </summary>
        public void AppendIsNullOrNotNull()
        {
            var index = LastIndex('=');
            if (index == -1)
                return;
            var notIndex = index - 1;
            if (notIndex >= 0 && _builder[notIndex] == '!')
            {
                _builder.Remove(notIndex, _builder.Length - notIndex);
                _builder.Append("IS NOT NULL");
            }
            else
            {
                _builder.Remove(index, _builder.Length - index);
                _builder.Append("IS NULL");
            }
        }

        private int LastIndex(char end)
        {
            for (var i = _builder.Length - 1; i > 0; i--)
            {
                var current = _builder[i];
                if (char.IsWhiteSpace(current))
                    continue;
                if (current == end)
                    return i;
                return -1;
            }

            return -1;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Gentings.Tasks
{
    /// <summary>
    /// 参数实例对象。
    /// </summary>
    public class Argument
    {
        private readonly IDictionary<string, object> _arguments =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 初始化类<see cref="Argument"/>。
        /// </summary>
        public Argument()
        {
        }

        internal Argument(string arguments)
        {
            if (string.IsNullOrWhiteSpace(arguments))
            {
                return;
            }

            var data = Cores.FromJsonString<Dictionary<string, object>>(arguments);
            foreach (var o in data)
            {
                _arguments[o.Key] = o.Value;
            }
        }

        /// <summary>
        /// 索引查找和设置参数实例对象。
        /// </summary>
        /// <param name="name">参数名称。</param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                if (_arguments.TryGetValue(name, out var value))
                {
                    return value;
                }

                return null;
            }
            set
            {
                if (name == nameof(IsStack) || name == nameof(Interval))
                {
                    throw new Exception($"不能在服务内部设置 {nameof(IsStack)} 和 {nameof(Interval)} 属性！");
                }

                _arguments[name] = value;
            }
        }

        /// <summary>
        /// 返回表示当前对象的字符串。
        /// </summary>
        /// <returns>
        /// 表示当前对象的字符串。
        /// </returns>
        public override string ToString()
        {
            return _arguments.ToJsonString();
        }

        /// <summary>
        /// 参数个数。
        /// </summary>
        public int Count => _arguments.Count;

        /// <summary>
        /// 获取整形参数。
        /// </summary>
        /// <param name="name">参数名称。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>返回当前参数。</returns>
        public int GetInt32(string name, int defaultValue = 0)
        {
            var value = this[name];
            if (value == null) return defaultValue;
            if (value is JsonElement element)
                return element.GetInt32();
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// 获取布尔型参数。
        /// </summary>
        /// <param name="name">参数名称。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>返回当前参数。</returns>
        public bool GetBoolean(string name, bool defaultValue = false)
        {
            var value = this[name];
            if (value == null) return defaultValue;
            if (value is JsonElement element)
                return element.GetBoolean();
            return Convert.ToBoolean(value);
        }

        /// <summary>
        /// 获取日期时间参数。
        /// </summary>
        /// <param name="name">参数名称。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>返回当前参数。</returns>
        public DateTime? GetDateTime(string name, DateTime? defaultValue = null)
        {
            var value = this[name];
            if (value == null) return defaultValue;
            if (value is JsonElement element)
                return element.GetDateTime();
            return Convert.ToDateTime(value);
        }

        /// <summary>
        /// 当前服务Id。
        /// </summary>
        public TaskContext TaskContext { get; internal set; }

        /// <summary>
        /// 自定义后台服务运行模式。
        /// </summary>
        public string Interval
        {
            get => this[nameof(Interval)]?.ToString();
            internal set => _arguments[nameof(Interval)] = value;
        }

        /// <summary>
        /// 错误消息。
        /// </summary>
        public string Error
        {
            get => this[nameof(Error)]?.ToString();
            set => this[nameof(Error)] = value;
        }

        /// <summary>
        /// 错误发生时间。
        /// </summary>
        public DateTime? ErrorDate
        {
            get => GetDateTime(nameof(ErrorDate));
            set => this[nameof(ErrorDate)] = value;
        }

        /// <summary>
        /// 是否保存堆栈信息。
        /// </summary>
        public bool IsStack
        {
            get => GetBoolean(nameof(IsStack));
            internal set => _arguments[nameof(IsStack)] = value;
        }
    }
}
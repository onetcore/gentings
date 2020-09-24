﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Gentings.Extensions
{
    /// <summary>
    /// 扩展基类。
    /// </summary>
    public abstract class ExtendBase
    {
        private IDictionary<string, string> _extendProperties =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 扩展方法，注意如果使用这个扩展属性承载选择的其他项目，需要先选择当前表格的<see cref="ExtendProperties"/>列，否则可能会被覆盖。
        /// </summary>
        [JsonIgnore]
        public string ExtendProperties
        {
            get => _extendProperties.ToJsonString();
            set => _extendProperties = Cores.FromJsonString<Dictionary<string, string>>(value);
        }

        /// <summary>
        /// 索引访问和设置扩展属性。
        /// </summary>
        /// <param name="name">索引值。</param>
        /// <returns>返回当前扩展属性值。</returns>
        [NotMapped]
        [JsonIgnore]
        public string this[string name]
        {
            get
            {
                if (!name.StartsWith("ex:"))
                {
                    name = "ex:" + name;
                }

                _extendProperties.TryGetValue(name, out var value);
                return value;
            }
            set
            {
                if (!name.StartsWith("ex:"))
                {
                    name = "ex:" + name;
                }

                _extendProperties[name] = value;
            }
        }

        /// <summary>
        /// 扩展属性列表。
        /// </summary>
        [JsonIgnore]
        public IEnumerable<string> ExtendKeys => _extendProperties.Keys;
    }
}
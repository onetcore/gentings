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
            get => _extendProperties.ToJsonString()!;
            set => _extendProperties = Cores.FromJsonString<Dictionary<string, string>>(value) ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 索引访问和设置扩展属性。
        /// </summary>
        /// <param name="name">索引值。</param>
        /// <returns>返回当前扩展属性值。</returns>
        [NotMapped]
        [JsonIgnore]
        public string? this[string name]
        {
            get
            {
                _extendProperties.TryGetValue(name, out var value);
                return value;
            }
            set
            {
                if (value == null)
                    _extendProperties.Remove(name);
                else
                    _extendProperties[name] = value;
            }
        }

        /// <summary>
        /// 获取布尔值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <returns>返回当前名称的布尔值。</returns>
        protected bool? GetBoolean(string name)
        {
            if (_extendProperties.TryGetValue(name, out var value) &&
                bool.TryParse(value.Trim(), out var result))
                return result;
            return null;
        }

        /// <summary>
        /// 设置布尔值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <param name="value">参数值。</param>
        protected void SetBoolean(string name, bool? value)
        {
            if (value == null)
                _extendProperties.Remove(name);
            else
                this[name] = value.ToString();
        }

        /// <summary>
        /// 获取枚举值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <returns>返回当前名称的枚举值。</returns>
        protected TEnum? GetEnum<TEnum>(string name)
            where TEnum : struct
        {
            if (_extendProperties.TryGetValue(name, out var value) &&
                Enum.TryParse<TEnum>(value.Trim(), true, out var result))
                return result;
            return null;
        }

        /// <summary>
        /// 设置枚举值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <param name="value">参数值。</param>
        protected void SetEnum(string name, Enum? value)
        {
            if (value == null)
                _extendProperties.Remove(name);
            else
                this[name] = value.ToString("D");
        }

        /// <summary>
        /// 获取数值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <returns>返回当前名称的数值。</returns>
        protected int? GetInt32(string name)
        {
            if (_extendProperties.TryGetValue(name, out var value) &&
                int.TryParse(value.Trim(), out var result))
                return result;
            return null;
        }

        /// <summary>
        /// 设置数值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <param name="value">参数值。</param>
        protected void SetInt32(string name, int? value)
        {
            if (value == null)
                _extendProperties.Remove(name);
            else
                this[name] = value.ToString();
        }

        /// <summary>
        /// 获取数值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <returns>返回当前名称的数值。</returns>
        protected long? GetInt64(string name)
        {
            if (_extendProperties.TryGetValue(name, out var value) &&
                long.TryParse(value.Trim(), out var result))
                return result;
            return null;
        }

        /// <summary>
        /// 设置数值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <param name="value">参数值。</param>
        protected void SetInt64(string name, long? value)
        {
            if (value == null)
                _extendProperties.Remove(name);
            else
                this[name] = value.ToString();
        }

        /// <summary>
        /// 获取日期值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <returns>返回当前名称的日期值。</returns>
        protected DateTime? GetDateTime(string name)
        {
            if (_extendProperties.TryGetValue(name, out var value) &&
                DateTime.TryParse(value.Trim(), out var result))
                return result;
            return null;
        }

        /// <summary>
        /// 设置日期值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <param name="value">参数值。</param>
        protected void SetDateTime(string name, DateTime? value)
        {
            if (value == null)
                _extendProperties.Remove(name);
            else
                this[name] = value.ToString();
        }

        /// <summary>
        /// 获取日期值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <returns>返回当前名称的日期值。</returns>
        protected DateTimeOffset? GetDateTimeOffset(string name)
        {
            if (_extendProperties.TryGetValue(name, out var value) &&
                DateTimeOffset.TryParse(value.Trim(), out var result))
                return result;
            return null;
        }

        /// <summary>
        /// 设置日期值。
        /// </summary>
        /// <param name="name">当前名称。</param>
        /// <param name="value">参数值。</param>
        protected void SetDateTimeOffset(string name, DateTimeOffset? value)
        {
            if (value == null)
                _extendProperties.Remove(name);
            else
                this[name] = value.ToString();
        }

        /// <summary>
        /// 扩展属性列表。
        /// </summary>
        [JsonIgnore]
        public IEnumerable<string> ExtendKeys => _extendProperties.Keys;
    }
}
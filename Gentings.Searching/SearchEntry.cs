using System.Data.Common;

namespace Gentings.Searching
{
    /// <summary>
    /// 搜索实体。
    /// </summary>
    public class SearchEntry
    {
        /// <summary>
        /// 实体唯一Id。
        /// </summary>
        public int Id { get; }

        private readonly IDictionary<string, string> _entries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        internal SearchEntry(DbDataReader reader)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                if (name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                    Id = reader.GetInt32(i);
                else if (!reader.IsDBNull(i))
                    _entries.Add(name, reader.GetValue(i).ToString()!.Trim());
            }
        }

        /// <summary>
        /// 获取或设置实体实例对象。
        /// </summary>
        /// <param name="key">实体名称。</param>
        /// <returns>返回实体实例对象。</returns>
        public string? this[string key]
        {
            get
            {
                _entries.TryGetValue(key, out var value);
                return value;
            }
            set
            {
                if (value == null)
                    _entries.Remove(key);
                else
                    _entries[key] = value;
            }
        }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions.Groups;

namespace Gentings.Extensions.Settings
{
    /// <summary>
    /// 名称字典字符串。
    /// </summary>
    [Table("core_Named_Strings")]
    public class NamedString : GroupBase<NamedString>
    {
        /// <summary>
        /// 值。
        /// </summary>
        [Size(256)]
        public string? Value { get; set; }

        private string? _path;
        /// <summary>
        /// 路径，以“.”分割父级名称。
        /// </summary>
        public string Path
        {
            get
            {
                if (_path == null)
                {
                    var list = new List<string>();
                    var current = this;
                    while (current?.Id > 0)
                    {
                        list.Add(current.Name!);
                        current = current.Parent;
                    }

                    list.Reverse();
                    _path = string.Join(".", list).ToLower();
                }

                return _path;
            }
        }

        /// <summary>
        /// 当前名称的值。
        /// </summary>
        /// <returns>当前名称的值。</returns>
        public override string? ToString()
        {
            return Value;
        }
    }
}
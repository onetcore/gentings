using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions.Groups;

namespace Gentings.Extensions.OpenServices.Industries
{
    /// <summary>
    /// 行业分类。
    /// </summary>
    [Table("open_Industries")]
    public class Industry : GroupBase<Industry>
    {
        /// <summary>
        /// 门类。
        /// </summary>
        public char Kind { get; set; }

        /// <summary>
        /// 大类。
        /// </summary>
        public short Group { get; set; }

        /// <summary>
        /// 中类。
        /// </summary>
        public short Category { get; set; }

        /// <summary>
        /// 小类。
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [Size(512)]
        public string Summary { get; set; }
    }
}
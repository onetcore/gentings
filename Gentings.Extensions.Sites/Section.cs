using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 节点类。
    /// </summary>
    [Table("site_Sections")]
    public class Section : SourceEntityBase
    {
        /// <summary>
        /// 页面Id。
        /// </summary>
        [NotUpdated]
        public int PageId { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        [Size(64)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 排序。
        /// </summary>
        [NotUpdated]
        public int Order { get; set; }

        /// <summary>
        /// 是否显式呈现在页面中。
        /// </summary>
        public bool IsPaged { get; set; }

        /// <summary>
        /// 节点HTML中唯一标记：s{Id:D3}。
        /// </summary>
        public virtual string UniqueId => $"s{Id:D3}";

        /// <summary>
        /// 节点模板类型Id。
        /// </summary>
        [NotMapped]
        public string SectionType { get => this[nameof(SectionType)]; set => this[nameof(SectionType)] = value; }

        /// <summary>
        /// 标签名称。
        /// </summary>
        [NotMapped]
        public string TagName { get => this[nameof(TagName)]; set => this[nameof(TagName)] = value; }

        /// <summary>
        /// 是否宽屏。
        /// </summary>
        [NotMapped]
        public bool? IsFluid { get => GetBoolean(nameof(IsFluid)); set => SetBoolean(nameof(IsFluid), value); }
    }
}

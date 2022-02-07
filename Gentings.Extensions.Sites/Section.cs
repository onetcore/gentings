using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 节点类。
    /// </summary>
    [Table("site_Sections")]
    public class Section : PageSectionBase
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
        public string? DisplayName { get; set; }

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
        public string RenderName { get => this[nameof(RenderName)]; set => this[nameof(RenderName)] = value; }

        /// <summary>
        /// 节点模板类型显示名称。
        /// </summary>
        [NotMapped]
        public string RenderDisplayName { get => this[nameof(RenderDisplayName)]; set => this[nameof(RenderDisplayName)] = value; }

        /// <summary>
        /// 是否宽屏。
        /// </summary>
        [NotMapped]
        public bool? IsFluid { get => GetBoolean(nameof(IsFluid)); set => SetBoolean(nameof(IsFluid), value); }

        /// <summary>
        /// 样式代码。
        /// </summary>
        [NotMapped]
        public string? Style { get => this[nameof(Style)]; set => this[nameof(Style)] = value; }

        /// <summary>
        /// 脚本代码。
        /// </summary>
        [NotMapped]
        public string? Script { get => this[nameof(Script)]; set => this[nameof(Script)] = value; }

        /// <summary>
        /// 将存储类型实例转换为子类型。
        /// </summary>
        /// <typeparam name="TSection">节点类型。</typeparam>
        /// <returns>返回节点类型实例。</returns>
        public TSection As<TSection>() where TSection : Section, new()
        {
            var section = new TSection();
            section.Id = Id;
            section.Name = Name;
            section.Disabled = Disabled;
            section.DisplayMode = DisplayMode;
            section.CreatedDate = CreatedDate;
            section.UpdatedDate = UpdatedDate;
            section.PageId = PageId;
            section.DisplayName = DisplayName;
            section.Order = Order;
            section.IsPaged = IsPaged;
            foreach (var key in ExtendKeys)
                section[key] = this[key];
            return section;
        }
    }
}

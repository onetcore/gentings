using Gentings.Extensions.Sites.Menus;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Sites.Sections.Carousels
{
    /// <summary>
    /// Carousel实例。
    /// </summary>
    [Table("site_Sections_Carousels")]
    public class Carousel : ExtendBase, IIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 滚动项目Id。
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// 排序。
        /// </summary>
        [NotUpdated]
        public int Order { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        [Size(64)]
        public string? Title { get; set; }

        /// <summary>
        /// 是否禁用。
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 显示模式。
        /// </summary>
        public DisplayMode DisplayMode { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTimeOffset? UpdatedDate { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [NotMapped]
        public string? Description { get => this[nameof(Description)]; set => this[nameof(Description)] = value; }

        /// <summary>
        /// 图片地址。
        /// </summary>
        [NotMapped]
        public string? ImageUrl { get => this[nameof(ImageUrl)]; set => this[nameof(ImageUrl)] = value; }

        /// <summary>
        /// 背景颜色。
        /// </summary>
        [NotMapped]
        public string? BgColor { get => this[nameof(BgColor)]; set => this[nameof(BgColor)] = value; }

        /// <summary>
        /// 链接地址。
        /// </summary>
        [NotMapped]
        public string LinkUrl { get => this[nameof(LinkUrl)]; set => this[nameof(LinkUrl)] = value; }

        /// <summary>
        /// 打开方式。
        /// </summary>
        [NotMapped]
        public OpenTarget Target { get => GetEnum<OpenTarget>(nameof(Target)) ?? OpenTarget.Self; set => SetEnum(nameof(Target), value); }

        /// <summary>
        /// a和link标签的rel属性。
        /// </summary>
        [NotMapped]
        public LinkRel? Rel { get => GetEnum<LinkRel>(nameof(Rel)); set => SetEnum(nameof(Rel), value); }

        /// <summary>
        /// 内容HTML代码。
        /// </summary>
        [NotMapped]
        public string CaptionHTML { get => this[nameof(CaptionHTML)]; set => this[nameof(CaptionHTML)] = value; }
    }
}
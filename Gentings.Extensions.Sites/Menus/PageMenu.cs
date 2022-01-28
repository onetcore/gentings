using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions.Groups;

namespace Gentings.Extensions.Sites.Menus
{
    /// <summary>
    /// 页面菜单。
    /// </summary>
    [Table("site_Menus")]
    public class PageMenu : GroupBase<PageMenu>
    {
        /// <summary>
        /// 分类Id。
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 显示模式。
        /// </summary>
        public DisplayMode DisplayMode { get; set; }

        /// <summary>
        /// 链接地址。
        /// </summary>
        [Size(256)]
        public string? LinkUrl { get; set; }

        /// <summary>
        /// 打开方式。
        /// </summary>
        public OpenTarget Target { get; set; }

        /// <summary>
        /// 框架名称。
        /// </summary>
        [Size(32)]
        public string? FrameName { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [Size(64)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// 排序。
        /// </summary>
        [NotUpdated]
        public int Order { get; set; }

        /// <summary>
        /// a和link标签的rel属性。
        /// </summary>
        public LinkRel? Rel { get; set; }
    }
}
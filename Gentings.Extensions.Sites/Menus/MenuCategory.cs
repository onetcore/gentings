using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions.Categories;

namespace Gentings.Extensions.Sites.Menus
{
    /// <summary>
    /// 菜单分类。
    /// </summary>
    [Table("site_Menus_Categories")]
    public class MenuCategory : CategoryBase
    {
        /// <summary>
        /// 显示名称。
        /// </summary>
        [Size(64)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [Size(256)]
        public string? Description { get; set; }
    }
}
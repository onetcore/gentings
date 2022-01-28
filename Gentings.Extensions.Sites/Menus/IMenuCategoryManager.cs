using Gentings.Extensions.Categories;

namespace Gentings.Extensions.Sites.Menus
{
    /// <summary>
    /// 菜单分类管理接口。
    /// </summary>
    public interface IMenuCategoryManager : ICachableCategoryManager<MenuCategory>, ISingletonService
    {

    }
}
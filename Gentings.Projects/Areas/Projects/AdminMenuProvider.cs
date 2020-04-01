using System;
using System.Linq;
using Gentings.AspNetCore.RazorPages.AdminMenus;
using Gentings.Projects.APIs;

namespace Gentings.Projects.Areas.Projects
{
    public class AdminMenuProvider : MenuProvider
    {
        private readonly IApiManager _apiManager;

        public AdminMenuProvider(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("proj", item =>
            {
                item.Texted("项目管理", "layers").Page("/Index", area: ProjectSettings.ExtensionName);
                var modules = _apiManager.GetAssemblies().Select(x => x.AssemblyName).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                foreach (var assembly in modules)
                {
                    item.AddMenu(assembly, it => it.Texted(assembly).Page("/Assembly", area: ProjectSettings.ExtensionName, routeValues: new { assembly }));
                }
                //.AddMenu("docs", it => it.Texted("注释文档").Page("/Index", area: "Projects"))
                //.AddMenu("actions", it => it.Texted("API方法").Page("/Actions", area: "Projects"))
                //.AddMenu("controllers", it => it.Texted("控制器").Page("/Controllers", area: "Projects"));
            });
        }
    }
}
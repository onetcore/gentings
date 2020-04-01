using System.Collections.Generic;
using System.Linq;
using Gentings.Projects.APIs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gentings.Projects.Areas.Projects.Pages
{
    public class AssemblyModel : PageModel
    {
        private readonly IApiManager _apiManager;

        public AssemblyModel(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }

        public IActionResult OnGet()
        {
            if (RouteData.Values.TryGetValue("assembly", out var value))
            {
                var assemblyName = value.ToString();
                Assembly = _apiManager.GetAssemblies().FirstOrDefault(x => x.AssemblyName == assemblyName);
                if (Assembly == null)
                    return NotFound();
                var index = assemblyName.LastIndexOf('.');
                TagName = index > 0 ? assemblyName[index + 1] : assemblyName[0];
                Apis = _apiManager.GetApiDescriptors().Where(x => x.Assembly.AssemblyName == assemblyName).ToList();
                return Page();
            }

            return NotFound();
        }

        /// <summary>
        /// 程序集首字母。
        /// </summary>
        public char TagName { get; private set; }

        /// <summary>
        /// 程序集实例。
        /// </summary>
        public AssemblyInfo Assembly { get; private set; }

        /// <summary>
        /// 当前程序集的API列表。
        /// </summary>
        public IEnumerable<ApiDescriptor> Apis { get; private set; }
    }
}

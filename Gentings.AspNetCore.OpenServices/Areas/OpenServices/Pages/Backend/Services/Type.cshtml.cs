using Gentings.Extensions.OpenServices.ApiDocuments;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.OpenServices.Areas.OpenServices.Pages.Backend.Services
{
    /// <summary>
    /// 类型模型。
    /// </summary>
    public class TypeModel : ModelBase
    {
        /// <summary>
        /// 获取当前类型实例。
        /// </summary>
        /// <returns>返回类型相关页面。</returns>
        public IActionResult OnGet()
        {
            if (RouteData.Values.TryGetValue("type", out var value))
            {
                var typeName = value.ToString();
                TypeDescriptor = AssemblyDocument.GetTypeDescriptor(typeName);
                if (TypeDescriptor == null)
                    return NotFound();
                AssemblyName = TypeDescriptor.Assembly.AssemblyName;
                Type = Type.GetType($"{typeName}, {AssemblyName}");
                if (Type == null)
                    return NotFound();
                var index = typeName.LastIndexOf('.');
                TagName = index > 0 ? typeName[index + 1] : typeName[0];
                return Page();
            }

            return NotFound();
        }

        /// <summary>
        /// 程序集。
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// 类型名称。
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// 类型名称首字母。
        /// </summary>
        public char TagName { get; private set; }

        /// <summary>
        /// 类型注释实例。
        /// </summary>
        public TypeDescriptor TypeDescriptor { get; private set; }
    }
}

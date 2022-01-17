using Gentings.Documents.XmlDocuments;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Docs.Areas.Docs.Pages
{
    /// <summary>
    /// 类型模型。
    /// </summary>
    public class TypeModel : ModelBase
    {
        /// <summary>
        /// 获取当前类型实例。
        /// </summary>
        /// <param name="type">类型名称。</param>
        /// <returns>返回类型相关页面。</returns>
        public IActionResult OnGet(string type)
        {
            TypeDescriptor = AssemblyDocument.GetTypeDescriptor(type);
            if (TypeDescriptor == null)
                return NotFound();
            AssemblyName = TypeDescriptor.Assembly.AssemblyName;
            Type = Type.GetType($"{type}, {AssemblyName}");
            if (Type == null)
                return NotFound();
            var index = type.LastIndexOf('.');
            TagName = index > 0 ? type[index + 1] : type[0];
            return Page();
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

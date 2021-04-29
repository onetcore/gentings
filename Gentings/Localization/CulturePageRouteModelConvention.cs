using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Gentings.Localization
{
    /// <summary>
    /// 资源路由模型转换类型。
    /// </summary>
    internal class CulturePageRouteModelConvention : IPageRouteModelConvention
    {
        /// <summary>
        /// 附加转换模型<see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.PageRouteModel" />实例。
        /// </summary>
        /// <param name="model">页面路由模型<see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.PageRouteModel" />实例。</param>
        public void Apply(PageRouteModel model)
        {
            var selectorModels = new List<SelectorModel>();
            foreach (var selector in model.Selectors.ToList())
            {
                var template = selector.AttributeRouteModel.Template;
                selectorModels.Add(new SelectorModel()
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = "/{culture}/" + template
                    }
                });
            }
            foreach (var m in selectorModels)
            {
                model.Selectors.Add(m);
            }
        }
    }
}
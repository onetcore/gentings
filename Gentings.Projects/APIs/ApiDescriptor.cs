using System.Collections.Generic;
using Gentings.Projects.Documents;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace Gentings.Projects.APIs
{
    /// <summary>
    /// API实体。
    /// </summary>
    public class ApiDescriptor
    {
        /// <summary>
        /// 注释信息。
        /// </summary>
        public MethodDescriptor Summary { get; set; }
        /// <summary>
        /// 程序集名称。
        /// </summary>
        public AssemblyInfo Assembly { get; set; }
        /// <summary>
        /// Controller名称。
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// Action名称。
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 显示名称。
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 路由模板。
        /// </summary>
        public string RouteTemplate { get; set; }
        /// <summary>
        /// HTTP方法。
        /// </summary>
        public HttpMethod HttpMethod { get; set; }
        /// <summary>
        /// 路由参数列表。
        /// </summary>
        public IDictionary<string, string> RouteValues { get; set; }
        /// <summary>
        /// 参数描述实例。
        /// </summary>
        public IList<Microsoft.AspNetCore.Mvc.Abstractions.ParameterDescriptor> Parameters { get; set; }

        /// <summary>
        /// 获取参数描述信息。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <returns>返回参数描述信息实例。</returns>
        public ParameterDescriptor GetParameterDescriptor(string parameterName)
        {
            if (Summary == null)
                return null;
            if (Summary.Parameters.TryGetValue(parameterName, out var descriptor))
                return descriptor;
            return null;
        }
    }
}
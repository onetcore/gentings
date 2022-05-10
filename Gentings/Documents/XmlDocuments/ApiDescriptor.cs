using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Gentings.Documents.XmlDocuments
{
    /// <summary>
    /// API实体。
    /// </summary>
    public class ApiDescriptor : IEqualityComparer<ApiDescriptor>, IComparable<ApiDescriptor>
    {
        /// <summary>
        /// 注释信息。
        /// </summary>
        public MethodDescriptor? Summary { get; set; }

        /// <summary>
        /// 程序集名称。
        /// </summary>
        public AssemblyInfo? Assembly { get; set; }

        /// <summary>
        /// Controller名称。
        /// </summary>
        public string? ControllerName { get; set; }

        /// <summary>
        /// Action名称。
        /// </summary>
        public string? ActionName { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// 分组名称。
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// 路由模板。
        /// </summary>
        public string? RouteTemplate { get; set; }

        /// <summary>
        /// HTTP方法。
        /// </summary>
        public string? HttpMethod { get; set; }

        /// <summary>
        /// 是否需要登录验证。
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// 路由参数列表。
        /// </summary>
        public IDictionary<string, string>? RouteValues { get; set; }

        /// <summary>
        /// 参数描述实例。
        /// </summary>
        public IList<ApiParameterDescription>? Parameters { get; set; }

        /// <summary>
        /// 获取参数描述信息。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <returns>返回参数描述信息实例。</returns>
        public ParameterDescriptor? GetParameterDescriptor(string parameterName)
        {
            if (Summary == null)
                return null;
            if (Summary.Parameters.TryGetValue(parameterName, out var descriptor))
                return descriptor;
            return null;
        }

        /// <summary>
        /// 返回类型实例。
        /// </summary>
        public IList<ApiResponseType>? ResponseTypes { get; set; }

        /// <summary>
        /// 实现对比接口。
        /// </summary>
        /// <param name="x">API实体对象。</param>
        /// <param name="y">API实体对象。</param>
        /// <returns>返回对比结果。</returns>
        public bool Equals(ApiDescriptor? x, ApiDescriptor? y)
        {
            return x?.RouteTemplate?.Equals(y?.RouteTemplate) == true;
        }

        /// <summary>
        /// 哈希值。
        /// </summary>
        /// <param name="obj">API实体对象。</param>
        /// <returns>返回当前程序集信息的哈希值。</returns>
        public int GetHashCode(ApiDescriptor obj)
        {
            return obj.RouteTemplate?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? 0;
        }

        /// <summary>
        /// 实现对比方法。
        /// </summary>
        /// <param name="other">API实体对象。</param>
        /// <returns>返回对比结果。</returns>
        public int CompareTo(ApiDescriptor? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(RouteTemplate, other.RouteTemplate, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// JwtToken键。
        /// </summary>
        public const string JwtToken = "jwt-token";
    }
}
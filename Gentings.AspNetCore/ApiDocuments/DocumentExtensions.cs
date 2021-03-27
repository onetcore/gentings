using System;
using System.Reflection;

namespace Gentings.AspNetCore.ApiDocuments
{
    /// <summary>
    /// 扩展方法类型。
    /// </summary>
    public static class DocumentExtensions
    {
        /// <summary>
        /// 获取方法注释。
        /// </summary>
        /// <param name="info">当前方法实例。</param>
        /// <returns>返回方法注释实例。</returns>
        public static MethodDescriptor GetSummary(this MemberInfo info)
        {
            var typeDescriptor = AssemblyDocument.GetTypeDescriptor(info.DeclaringType);
            return typeDescriptor?.GetMethodDescriptor(info);
        }

        /// <summary>
        /// 获取HTTP请求方法颜色。
        /// </summary>
        /// <param name="method">HTTP请求方法。</param>
        /// <returns>返回HTTP请求方法颜色。</returns>
        public static string GetColor(this string method)
        {
            if (method == "POST")
                return "#28a745";
            if (method == "GET")
                return "#007bff";
            if (method == "DELETE")
                return "#dc3545";
            if (method == "PUT")
                return "#d39e00";
            if (method == "HEAD")
                return "#0062cc";
            if (method == "PATCH")
                return "#17a2b8";
            if (method == "OPTIONS")
                return "#adb5bd";
            return "#007bff";
        }

        /// <summary>
        /// 是否可以链接。
        /// </summary>
        /// <param name="type">类型实例。</param>
        /// <returns>返回判断结果。</returns>
        public static bool IsLinkable(this Type type)
        {
            return AssemblyDocument.GetTypeDescriptor(type) != null;
        }
    }
}
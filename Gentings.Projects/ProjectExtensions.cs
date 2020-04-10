using System;
using System.Reflection;
using System.Text;
using Gentings.Projects.Documents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace Gentings.Projects
{
    /// <summary>
    /// 项目扩展方法类。
    /// </summary>
    public static class ProjectExtensions
    {
        /// <summary>
        /// 获取HTTP请求方法。
        /// </summary>
        /// <param name="info">当前方法实例。</param>
        /// <returns>返回HTTP请求方法。</returns>
        public static HttpMethod GetHttpMethod(this MemberInfo info)
        {
            if (info.IsDefined(typeof(HttpPostAttribute)))
                return HttpMethod.Post;
            if (info.IsDefined(typeof(HttpPutAttribute)))
                return HttpMethod.Put;
            if (info.IsDefined(typeof(HttpDeleteAttribute)))
                return HttpMethod.Delete;
            if (info.IsDefined(typeof(HttpHeadAttribute)))
                return HttpMethod.Head;
            if (info.IsDefined(typeof(HttpPatchAttribute)))
                return HttpMethod.Patch;
            if (info.IsDefined(typeof(HttpOptionsAttribute)))
                return HttpMethod.Options;
            return HttpMethod.Get;
        }

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
        public static string GetColor(this HttpMethod method)
        {
            return method switch
            {
                HttpMethod.Post => "#28a745",
                HttpMethod.Put => "#d39e00",
                HttpMethod.Delete => "#dc3545",
                HttpMethod.Head => "#0062cc",
                HttpMethod.Patch => "#17a2b8",
                HttpMethod.Options => "#adb5bd",
                _ => "#007bff"
            };
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

        /// <summary>
        /// 获取真实类型，如果可空将获取真实类型。
        /// </summary>
        /// <param name="type">类型实例。</param>
        /// <returns>返回真实类型。</returns>
        public static string GetRealType(this Type type)
        {
            var isNullable = type.IsNullableType();
            if (isNullable)
                return type.UnwrapNullableType().FullName + "?";
            return type.FullName;
        }

        /// <summary>
        /// 获取TypeScript类型。
        /// </summary>
        /// <param name="type">当前类型实例。</param>
        /// <returns>返回TypeScript类型。</returns>
        private static string GetTypeScriptType(this Type type)
        {
            var isNullable = type.IsNullableType();
            if (isNullable)
                type = type.UnwrapNullableType();

            string GetTypeName()
            {
                if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
                    return "Date";
                if (type.IsEnum || type.IsValueType)
                    return "number";
                return "string";
            }

            var typeName = GetTypeName();
            if (isNullable)
                return "? " + typeName;
            return " " + typeName;
        }

        /// <summary>
        /// 获取类型定义。
        /// </summary>
        /// <param name="type">当前类型。</param>
        /// <returns>返回接口定义。</returns>
        public static string GetClass(this Type type)
        {
            var typeDescriptor = AssemblyDocument.GetTypeDescriptor(type);
            var builder = new StringBuilder();
            builder.AppendLine("/// <summary>")
                .Append("/// ").AppendLine(typeDescriptor?.Summary)
                .AppendLine("/// </summary>");
            builder.Append("public class ").Append(type.Name).AppendLine("{");

            void Append(PropertyInfo info)
            {
                var summary = typeDescriptor?.GetPropertyDescriptor(info)?.Summary;
                if (summary != null)
                {
                    builder.AppendLine("    /// <summary>")
                        .Append("    /// ").AppendLine(summary)
                        .AppendLine("    /// </summary>");
                }

                builder.Append("    public ").Append(info.PropertyType.Name)
                    .Append(" ").Append(info.Name)
                    .AppendLine(" {get; set;}");
            }

            foreach (var property in type.GetProperties())
            {
                if (!property.CanRead || !property.CanWrite)
                {
                    continue;
                }
                Append(property);
            }

            builder.AppendLine("}");
            return builder.ToString();
        }

        /// <summary>
        /// 获取TypeScript接口定义。
        /// </summary>
        /// <param name="type">当前类型。</param>
        /// <returns>返回接口定义。</returns>
        public static string GetTypeScriptInterface(this Type type)
        {
            var typeDescriptor = AssemblyDocument.GetTypeDescriptor(type);
            var builder = new StringBuilder();
            builder.AppendLine("/**")
                .Append("* ").AppendLine(typeDescriptor?.Summary)
                .AppendLine("*/");
            builder.Append("interface ").Append(type.Name).AppendLine("{");

            void Append(PropertyInfo info)
            {
                var summary = typeDescriptor?.GetPropertyDescriptor(info)?.Summary;
                if (summary != null)
                {
                    builder.AppendLine("   /**")
                        .Append("    * ").AppendLine(summary)
                        .AppendLine("    */");
                }

                builder.Append("    ").Append(char.ToLower(info.Name[0]) + info.Name[1..])
                    .Append(info.PropertyType.GetTypeScriptType())
                    .AppendLine(";");
            }

            foreach (var property in type.GetProperties())
            {
                if (!property.CanRead || !property.CanWrite)
                {
                    continue;
                }
                Append(property);
            }

            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
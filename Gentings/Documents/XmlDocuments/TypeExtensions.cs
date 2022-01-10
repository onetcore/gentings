using System.Reflection;
using System.Text;

namespace Gentings.Documents.XmlDocuments
{
    /// <summary>
    /// 扩展方法。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取真实类型，如果可空将获取真实类型。
        /// </summary>
        /// <param name="type">类型实例。</param>
        /// <returns>返回真实类型。</returns>
        public static Type GetRealType(this Type type)
        {
            var isNullable = type.IsNullableType();
            if (isNullable)
                return type.UnwrapNullableType();
            return type;
        }

        /// <summary>
        /// 获取真实类型名称，如果可空将获取真实类型名称。
        /// </summary>
        /// <param name="type">类型实例。</param>
        /// <returns>返回真实类型名称。</returns>
        public static string GetRealTypeName(this Type type) => type.GetRealType().Name;

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

                builder.Append("    ").Append(info.Name.ToLowerCamelCase())
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

        /// <summary>
        /// 将名称转换为Camel格式名称。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>返回驼峰格式字符串。</returns>
        public static string ToLowerCamelCase(this string name) => char.ToLower(name[0]) + name[1..];
    }
}
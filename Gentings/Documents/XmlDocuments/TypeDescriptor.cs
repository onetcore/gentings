﻿using System.Reflection;

namespace Gentings.Documents.XmlDocuments
{
    /// <summary>
    /// 类型描述。
    /// </summary>
    public class TypeDescriptor
    {
        internal TypeDescriptor(string typeName, string? summary)
        {
            Summary = summary;
            FullName = typeName;
            var index = typeName.LastIndexOf('.');
            Name = typeName[(index + 1)..];
            Namespace = typeName[..index];
        }

        /// <summary>
        /// 程序集实例。
        /// </summary>
        public AssemblyDocument? Assembly { get; internal set; }

        /// <summary>
        /// 是否为枚举。
        /// </summary>
        public bool IsEnum { get; internal set; }

        /// <summary>
        /// 命名空间。
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 全名。
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string? Summary { get; internal set; }

        /// <summary>
        /// 属性列表。
        /// </summary>
        private readonly IDictionary<string, PropertyDescriptor> _properties = new Dictionary<string, PropertyDescriptor>(StringComparer.OrdinalIgnoreCase);

        internal void Add(PropertyDescriptor property)
        {
            _properties[property.FullName] = property;
        }

        /// <summary>
        /// 方法列表。
        /// </summary>
        private readonly IDictionary<string, MethodDescriptor> _methods = new Dictionary<string, MethodDescriptor>(StringComparer.OrdinalIgnoreCase);

        internal void Add(MethodDescriptor method)
        {
            _methods[method.FullName] = method;
        }

        /// <summary>
        /// 通过成员信息获取属性注释实例。
        /// </summary>
        /// <param name="name">成员名称。</param>
        /// <returns>返回属性注释实例。</returns>
        public PropertyDescriptor? GetPropertyDescriptor(string name)
        {
            name = $"{FullName}.{name}";
            _properties.TryGetValue(name, out var descriptor);
            return descriptor;
        }

        /// <summary>
        /// 通过成员信息获取方法注释实例。
        /// </summary>
        /// <param name="info">成员信息。</param>
        /// <returns>返回方法注释实例。</returns>
        public MethodDescriptor? GetMethodDescriptor(MemberInfo info)
        {
            var method = info as MethodInfo;
            if (method == null)
                return null;
            var fullName = $"{method.DeclaringType!.FullName}.{method.Name}";
            var parameters = new List<string>();
            foreach (var parameter in method.GetParameters())
            {
                parameters.Add(parameter.ParameterType.FullName!);
            }

            if (parameters.Count > 0)
            {
                fullName += $"({parameters.Join()})";
            }

            _methods.TryGetValue(fullName, out var descriptor);
            return descriptor;
        }

        /// <summary>
        /// 属性列表。
        /// </summary>
        public IEnumerable<PropertyDescriptor> Properties => _properties.Values;
    }
}
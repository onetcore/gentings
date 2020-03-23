using System;
using System.Collections.Generic;

namespace Gentings.Projects.Documents
{
    /// <summary>
    /// 方法实例。
    /// </summary>
    public class MethodDescriptor
    {
        internal MethodDescriptor(TypeDescriptor type, string name, string summary, string fullName)
        {
            Type = type;
            Name = name;
            Summary = summary;
            FullName = fullName;
        }

        /// <summary>
        /// 全名。
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// 类型实例。
        /// </summary>
        public TypeDescriptor Type { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 是否为构造函数。
        /// </summary>
        public bool IsConstructor => Name == "#ctor";

        /// <summary>
        /// 描述。
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// 返回描述。
        /// </summary>
        public ReturnDescriptor Returns { get; internal set; }

        /// <summary>
        /// 参数。
        /// </summary>
        public IDictionary<string, ParameterDescriptor> Parameters { get; } = new Dictionary<string, ParameterDescriptor>(StringComparer.OrdinalIgnoreCase);

        internal void Add(ParameterDescriptor parameter)
        {
            Parameters[parameter.Name] = parameter;
        }
    }
}
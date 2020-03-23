namespace Gentings.Projects.Documents
{
    /// <summary>
    /// 属性。
    /// </summary>
    public class PropertyDescriptor
    {
        internal PropertyDescriptor(TypeDescriptor type, string name, string summary, string fullName)
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
        /// 描述。
        /// </summary>
        public string Summary { get; }
    }
}
namespace Gentings.Projects.Documents
{
    /// <summary>
    /// 参数实例。
    /// </summary>
    public class ParameterDescriptor : ReturnDescriptor
    {
        internal ParameterDescriptor(string typeName, string name, MethodDescriptor method, string summary)
            : base(typeName, method, summary)
        {
            Name = name;
        }

        /// <summary>
        /// 参数名称。
        /// </summary>
        public string Name { get; }
    }
}
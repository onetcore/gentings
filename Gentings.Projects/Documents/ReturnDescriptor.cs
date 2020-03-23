namespace Gentings.Projects.Documents
{
    /// <summary>
    /// 返回描述。
    /// </summary>
    public class ReturnDescriptor
    {
        internal ReturnDescriptor(string typeName, MethodDescriptor method, string summary)
        {
            TypeName = typeName;
            Method = method;
            Summary = summary;
        }

        /// <summary>
        /// 当前方法。
        /// </summary>
        public MethodDescriptor Method { get; }

        /// <summary>
        /// 类型名称。
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Summary { get; }
    }
}
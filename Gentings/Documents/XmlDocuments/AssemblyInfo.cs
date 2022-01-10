using System.Reflection;

namespace Gentings.Documents.XmlDocuments
{
    /// <summary>
    /// 程序集信息。
    /// </summary>
    public class AssemblyInfo : IEqualityComparer<AssemblyInfo>, IComparable<AssemblyInfo>
    {
        /// <summary>
        /// 初始化程序集信息实例。
        /// </summary>
        /// <param name="assembly">程序集实例对象。</param>
        public AssemblyInfo(Assembly assembly)
        {
            var name = assembly.GetName();
            AssemblyName = name.Name!;
            Version = name.Version!;
        }

        /// <summary>
        /// 程序集名称。
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// 版本。
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// 实现对比接口。
        /// </summary>
        /// <param name="x">程序集信息实例。</param>
        /// <param name="y">程序集信息实例。</param>
        /// <returns>返回对比结果。</returns>
        public bool Equals(AssemblyInfo x, AssemblyInfo y)
        {
            return x?.AssemblyName.Equals(y?.AssemblyName) == true;
        }

        /// <summary>
        /// 哈希值。
        /// </summary>
        /// <param name="obj">程序集信息实例。</param>
        /// <returns>返回当前程序集信息的哈希值。</returns>
        public int GetHashCode(AssemblyInfo obj)
        {
            return obj.AssemblyName.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 实现对比方法。
        /// </summary>
        /// <param name="other">程序集信息实例。</param>
        /// <returns>返回对比结果。</returns>
        public int CompareTo(AssemblyInfo other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(AssemblyName, other.AssemblyName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
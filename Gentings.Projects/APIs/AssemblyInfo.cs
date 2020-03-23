using System;
using System.Collections.Generic;
using System.Reflection;

namespace Gentings.Projects.APIs
{
    /// <summary>
    /// 程序集信息。
    /// </summary>
    public class AssemblyInfo : IEqualityComparer<AssemblyInfo>, IComparable<AssemblyInfo>
    {
        internal AssemblyInfo(Assembly assembly)
        {
            var name = assembly.GetName();
            AssemblyName = name.Name;
            Version = name.Version;
        }

        /// <summary>
        /// 程序集名称。
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// 版本。
        /// </summary>
        public Version Version { get; }

        public bool Equals(AssemblyInfo x, AssemblyInfo y)
        {
            return x.AssemblyName.Equals(y.AssemblyName);
        }

        public int GetHashCode(AssemblyInfo obj)
        {
            return obj.AssemblyName.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        public int CompareTo(AssemblyInfo other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(AssemblyName, other.AssemblyName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
using System.Diagnostics;
using System.Reflection;

namespace Gentings.Extensions.Internal
{
    [DebuggerStepThrough]
    internal static class PropertyInfoExtensions
    {
        public static PropertyInfo? FindGetterProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.DeclaringType!
                           .GetPropertiesInHierarchy(propertyInfo.Name)
                           .FirstOrDefault(p => p.GetMethod != null);
        }

        public static PropertyInfo? FindSetterProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.DeclaringType!
                           .GetPropertiesInHierarchy(propertyInfo.Name)
                           .FirstOrDefault(p => p.SetMethod != null);
        }
    }
}
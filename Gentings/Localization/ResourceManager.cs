using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Gentings.Localization
{
    /// <summary>
    /// 资源读取类型。
    /// </summary>
    public class ResourceManager
    {
        private static readonly ConcurrentDictionary<Type, System.Resources.ResourceManager> _localizers = new ConcurrentDictionary<Type, System.Resources.ResourceManager>();

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="type">资源所在程序集的类型。</param>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString(Type type, string key)
        {
            var resourceManager = _localizers.GetOrAdd(type ?? typeof(ResourceManager), t =>
              {
                  var assembly = type == null ? Assembly.GetEntryAssembly() : t.Assembly;
                  var baseName = assembly.GetManifestResourceNames()
                      .SingleOrDefault(x => x.EndsWith(".Resources.resources"));
                  if (baseName == null)
                  {
                      return null;
                  }

                  baseName = baseName[0..^10];
                  return new System.Resources.ResourceManager(baseName, assembly);
              });
            return resourceManager?.GetString(key) ?? key;
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例（网站程序集）。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString(string key) => GetString(null, key);

        /// <summary>
        /// 获取当前键的本地化字符串实例（网站程序集）。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString(string key, params object[] args)
        {
            var resource = GetString(key);
            if (resource == null)
            {
                return key;
            }

            return string.Format(resource, args);
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <typeparam name="TResource">包含当前资源包的所在程序集的类型。</typeparam>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString<TResource>(string key)
        {
            return GetString(typeof(TResource), key);
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <typeparam name="TResource">包含当前资源包的所在程序集的类型。</typeparam>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString<TResource>(string key, params object[] args)
        {
            var resource = GetString(typeof(TResource), key);
            if (resource == null)
            {
                return key;
            }

            return string.Format(resource, args);
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="type">资源所在程序集的类型。</param>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString(Type type, string key, params object[] args)
        {
            var resource = GetString(type, key);
            if (resource == null)
            {
                return key;
            }

            return string.Format(resource, args);
        }

        /// <summary>
        /// 获取当前表达式类型属性得资源字符串。
        /// </summary>
        /// <typeparam name="TResource">当前属性所在得类型实例。</typeparam>
        /// <param name="expression">表达式。</param>
        /// <returns>返回当前属性本地化字符串。</returns>
        public static string GetString<TResource>(Expression<Func<TResource, object>> expression)
        {
            var member = expression.GetPropertyAccess();
            if (member == null)
            {
                return null;
            }

            return GetString(member.DeclaringType, member.Name);
        }

        /// <summary>
        /// 获取当前表达式类型属性得资源字符串。
        /// </summary>
        /// <typeparam name="TResource">当前属性所在得类型实例。</typeparam>
        /// <param name="expression">表达式。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前属性本地化字符串。</returns>
        public static string GetString<TResource>(Expression<Func<TResource, object>> expression, params object[] args)
        {
            var resource = GetString(expression);
            if (resource == null)
            {
                return null;
            }

            return string.Format(resource, args);
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString(Enum key)
        {
            var type = key.GetType();
            var name = $"{type.Name}_{key}";
            var resource = GetString(type, name);
            if (resource == name)
                resource = key.ToDescriptionString();
            return resource;
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString(Enum key, params object[] args)
        {
            var resource = GetString(key);
            return string.Format(resource, args);
        }
    }
}
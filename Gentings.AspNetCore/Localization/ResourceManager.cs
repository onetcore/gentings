using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace Gentings.AspNetCore.Localization
{
    /// <summary>
    /// UI多语言资源管理接口。
    /// </summary>
    public class ResourceManager : IResourceManager
    {
        private readonly Regex _regex = new Regex(@"\W");
        private readonly Regex _single = new Regex("_+");
        private string GetSafeKey(string key) => _single.Replace(_regex.Replace(key, "_"), "_").Trim('_');//移除空格，将空格转换为下划线

        private readonly ConcurrentDictionary<string, TypedResource> _resources = new ConcurrentDictionary<string, TypedResource>(StringComparer.OrdinalIgnoreCase);
        private string? GetPhysicalPath(Type type, string culture, out string assemblyName, out string typeName)
        {
            assemblyName = type.Assembly.GetName().Name!;
            typeName = type.FullName!.Substring(assemblyName.Length + 1);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", culture, assemblyName, $"{typeName}.xml");
            if (File.Exists(path)) return path;
            culture = culture.Split('-')[0];
            path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", culture, assemblyName, $"{typeName}.xml");
            if (File.Exists(path)) return path;
            return null;
        }

        private string? GetPhysicalPath(string resourceName, string culture)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", culture, $"{resourceName}.xml");
            if (File.Exists(path)) return path;
            culture = culture.Split('-')[0];
            path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", culture, $"{resourceName}.xml");
            if (File.Exists(path)) return path;
            return null;
        }

        /// <summary>
        /// 获取资源实例。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <param name="key">资源名称。</param>
        /// <param name="culture">区域语言。</param>
        /// <returns>返回当前资源实例。</returns>
        public virtual string GetResource(Type type, string key, string? culture = null)
        {
            var safeKey = GetSafeKey(key);
            culture ??= Thread.CurrentThread.CurrentUICulture.Name;//当前UI语言
            var path = GetPhysicalPath(type, culture, out var assemblyName, out var typeName);
            if (path == null)
            {
#if DEBUG
                WriteTypedResource(assemblyName, typeName, safeKey, key);
#endif
                return key;
            }
            var resource = _resources.GetOrAdd(culture, _ => new TypedResource(type, path));
            var value = resource.GetResource(type, safeKey);
#if DEBUG
            if (value == null)
                WriteTypedResource(assemblyName, typeName, safeKey, key);
#endif
            return value ?? key;
        }

#if DEBUG
        private static void WriteTypedResource(string assemblyName, string typeName, string key, string text)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", "def", assemblyName, $"{typeName}.xml");
            WriteResource(path, key, text);
        }

        private static void WriteNamedResource(string resourceName, string key, string text)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", "def", $"{resourceName}.xml");
            WriteResource(path, key, text);
        }

        private static void WriteResource(string path, string key, string text)
        {
            if (!File.Exists(path))
            {
                var dir = Path.GetDirectoryName(path)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(path, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<root>\r\n</root>\r\n");
            }
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(path);
            var root = xmlDoc.SelectSingleNode("root")!;
            if (root.SelectSingleNode(key) != null) return;
            var element = xmlDoc.CreateElement(key);
            element.InnerXml = text.Trim();
            root.AppendChild(element);
            xmlDoc.Save(path);
        }
#endif
        private readonly ConcurrentDictionary<string, NamedResource> _namedResources = new ConcurrentDictionary<string, NamedResource>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 获取资源实例。
        /// </summary>
        /// <param name="resourceName">资源文件名，在语言包根目录下。</param>
        /// <param name="key">资源名称。</param>
        /// <param name="culture">区域语言。</param>
        /// <returns>返回当前资源实例。</returns>
        public virtual string GetResource(string resourceName, string key, string? culture = null)
        {
            var safeKey = GetSafeKey(key);
            culture ??= Thread.CurrentThread.CurrentUICulture.Name;//当前UI语言
            var path = GetPhysicalPath(resourceName, culture);
            if (path == null)
            {
#if DEBUG
                WriteNamedResource(resourceName, safeKey, key);
#endif
                return key;
            }
            var resource = _namedResources.GetOrAdd(culture, _ => new NamedResource(resourceName, path));
            var value = resource.GetResource(resourceName, safeKey);
#if DEBUG
            if (value == null)
                WriteNamedResource(resourceName, safeKey, key);
#endif
            return value ?? key;
        }
    }
}

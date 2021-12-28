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
        private readonly ConcurrentDictionary<string, TypedResource> _resources = new ConcurrentDictionary<string, TypedResource>(StringComparer.OrdinalIgnoreCase);
        private string GetPath(Type type, string culture)
        {
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName.Substring(assemblyName.Length + 1);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", culture, assemblyName, $"{typeName}.xml");
            if (File.Exists(path)) return path;
            culture = culture.Split('-')[0];
            path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", culture, assemblyName, $"{typeName}.xml");
            if (File.Exists(path)) return path;
            return null;
        }

        /// <summary>
        /// 获取资源实例。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <param name="key">资源名称。</param>
        /// <returns>返回当前资源实例。</returns>
        public string GetResource(Type type, string key)
        {
            var safeKey = _single.Replace(_regex.Replace(key, "_"), "_").Trim('_');//移除空格，将空格转换为下划线
            var culture = Thread.CurrentThread.CurrentUICulture.Name;//当前UI语言
            var path = GetPath(type, culture);
            if (path == null)
            {
#if DEBUG
                WriteResource(type, safeKey, key);
#endif
                return key;
            }
            var resource = _resources.GetOrAdd(culture, _ => new TypedResource(type, path));
            var value = resource.GetResource(type, safeKey);
#if DEBUG
            if (value == null)
                WriteResource(type, safeKey, key);
#endif
            return value ?? key;
        }
#if DEBUG
        private static void WriteResource(Type type, string key, string text)
        {
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName.Substring(assemblyName.Length + 1);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Resources", "def", assemblyName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, $"{typeName}.xml");
            if (!File.Exists(path))
                File.WriteAllText(path, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<root>\r\n</root>\r\n");
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(path);
            var root = xmlDoc.SelectSingleNode("root");
            if (root.SelectSingleNode(key) != null) return;
            var element = xmlDoc.CreateElement(key);
            element.InnerXml = text.Trim();
            root.AppendChild(element);
            xmlDoc.Save(path);
        }
#endif
    }
}

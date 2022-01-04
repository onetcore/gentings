using System.Collections.Concurrent;
using System.Xml;

namespace Gentings.AspNetCore.Localization
{
    /// <summary>
    /// 唯一名称资源。
    /// </summary>
    public class Resource
    {
        private readonly ConcurrentDictionary<string, string> _resources = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        internal Resource(string path)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            var node = xmlDoc.SelectSingleNode("root");
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Comment)
                    continue;
                _resources.AddOrUpdate(childNode.Name, childNode.InnerXml.Trim(), (k, v) => childNode.InnerXml.Trim());
            }
        }

        /// <summary>
        /// 获取当前资源名称的资源值。
        /// </summary>
        /// <param name="resourceName">当前资源名称唯一键。</param>
        /// <returns>返回资源值。</returns>
        public string? this[string resourceName]
        {
            get
            {
                _resources.TryGetValue(resourceName, out var value);
                return value;
            }
        }
    }
}

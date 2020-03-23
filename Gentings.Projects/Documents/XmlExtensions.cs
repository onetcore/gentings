using System.Xml;

namespace Gentings.Projects.Documents
{
    /// <summary>
    /// XML辅助类。
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// 获取子节点文本字符串。
        /// </summary>
        /// <param name="node">节点实例。</param>
        /// <param name="xpath">路径。</param>
        /// <returns>返回子节点文本字符串。</returns>
        public static string GetInnerXml(this XmlNode node, string xpath)
        {
            node = node.SelectSingleNode(xpath);
            return node?.InnerXml.Trim();
        }
    }
}
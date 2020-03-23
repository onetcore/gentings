using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Gentings.Projects.Documents
{
    /// <summary>
    /// XML注释文档实例。
    /// </summary>
    public class AssemblyDocument
    {
        static AssemblyDocument()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var files = directory.GetFiles("*.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var document = new AssemblyDocument(file.FullName);
                if (document.AssemblyName == null)
                    continue;
                if (!_assemblyDocuments.ContainsKey(document.AssemblyName))
                {
                    document.InitMembers();
                    _assemblyDocuments.Add(document.AssemblyName, document);
                }
            }
        }

        private readonly XmlNode _xmlDoc;
        private static readonly IDictionary<string, AssemblyDocument> _assemblyDocuments = new ConcurrentDictionary<string, AssemblyDocument>();
        /// <summary>
        /// 初始化类<see cref="AssemblyDocument"/>。
        /// </summary>
        /// <param name="path">注释文件路径。</param>
        private AssemblyDocument(string path)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            var docNode = xmlDoc.SelectSingleNode("doc");
            if (docNode == null)
                return;
            foreach (XmlNode node in docNode)
            {
                if (node.NodeType == XmlNodeType.Comment)
                    continue;
                switch (node.Name)
                {
                    case "assembly":
                        AssemblyName = node.GetInnerXml("name");
                        break;
                    case "members":
                        _xmlDoc = node;
                        break;
                }
            }
        }

        /// <summary>
        /// 程序集名称。
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// 类型成员。
        /// </summary>
        private readonly IDictionary<string, TypeDescriptor> _members = new ConcurrentDictionary<string, TypeDescriptor>(StringComparer.OrdinalIgnoreCase);

        private void InitMembers()
        {
            foreach (XmlNode xmlNode in _xmlDoc)
            {
                if (xmlNode.NodeType == XmlNodeType.Comment || xmlNode.Name != "member")
                    continue;
                var name = xmlNode.Attributes["name"]?.Value.Trim();
                if (name == null) continue;
                var prefix = name[0];
                var typeName = name.Substring(2);
                var fullName = typeName;
                var summary = xmlNode.GetInnerXml("summary");
                switch (prefix)
                {
                    case 'T':
                        {
                            var type = new TypeDescriptor(fullName, summary);
                            type.Assembly = this;
                            _members[type.FullName] = type;
                        }
                        break;
                    case 'P':
                    case 'F':
                        {
                            var index = typeName.LastIndexOf('.');
                            name = typeName.Substring(index + 1);
                            typeName = typeName.Substring(0, index);
                            if (!_members.TryGetValue(typeName, out var ptype))
                                throw new NullReferenceException($"类型{typeName}不能为空！");
                            ptype.IsEnum = prefix == 'F';
                            ptype.Add(new PropertyDescriptor(ptype, name, summary, fullName));
                        }
                        break;
                    case 'M':
                        {
                            var index = typeName.IndexOf('(');
                            if (index != -1)
                                typeName = typeName.Substring(0, index);
                            index = typeName.LastIndexOf('.');
                            name = typeName.Substring(index + 1);
                            typeName = typeName.Substring(0, index);
                            if (!_members.TryGetValue(typeName, out var mtype))
                                throw new NullReferenceException($"类型{typeName}不能为空！");
                            var method = new MethodDescriptor(mtype, name, summary, fullName);
                            mtype.Add(method);
                            var @params = xmlNode.SelectNodes("param");
                            foreach (XmlNode param in @params)
                            {
                                if (param.NodeType == XmlNodeType.Comment)
                                    continue;
                                method.Add(new ParameterDescriptor(null, param.Attributes["name"]?.Value.Trim(), method, param.InnerXml.Trim()));
                            }

                            var returns = xmlNode.SelectSingleNode("returns");
                            if (returns != null)
                                method.Returns = new ReturnDescriptor(null, method, returns.InnerXml.Trim());
                        }
                        break;
                }

            }
        }

        /// <summary>
        /// 尝试获取当前注释实例。
        /// </summary>
        /// <param name="assemblyName">程序集。</param>
        /// <param name="document">返回注释实例对象。</param>
        /// <returns>返回获取结果。</returns>
        public static bool TryGetDocument(string assemblyName, out AssemblyDocument document) =>
            _assemblyDocuments.TryGetValue(assemblyName, out document);

        /// <summary>
        /// 获取当前注释实例。
        /// </summary>
        /// <param name="assemblyName">程序集。</param>
        /// <returns>返回获取结果。</returns>
        public static AssemblyDocument GetDocument(string assemblyName)
        {
            _assemblyDocuments.TryGetValue(assemblyName, out var document);
            return document;
        }

        /// <summary>
        /// 包含的程序集列表。
        /// </summary>
        public static IEnumerable<string> Assemblies => _assemblyDocuments.Keys;

        /// <summary>
        /// 获取类型注释描述。
        /// </summary>
        /// <param name="type">当前类型。</param>
        /// <returns>返回注释描述实例。</returns>
        public static TypeDescriptor GetTypeDescriptor(Type type)
        {
            if (TryGetDocument(type.Assembly.GetName().Name, out var document) &&
                document._members.TryGetValue(type.FullName, out var value))
                return value;
            return null;
        }

        /// <summary>
        /// 获取类型注释描述。
        /// </summary>
        /// <param name="typeFullName">当前类型。</param>
        /// <returns>返回注释描述实例。</returns>
        public static TypeDescriptor GetTypeDescriptor(string typeFullName)
        {
            return _assemblyDocuments.Values.SelectMany(x => x._members.Values)
                .FirstOrDefault(x => x.FullName == typeFullName);
        }
    }
}
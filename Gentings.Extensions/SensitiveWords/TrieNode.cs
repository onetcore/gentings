using System.Collections.Generic;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 字典树。
    /// </summary>
    public class TrieNode
    {
        /// <summary>
        /// 设置是否已经到达尾部。
        /// </summary>
        public bool End { get; set; }

        /// <summary>
        /// 处理后的字符串存储列表。
        /// </summary>
        public List<string> Results { get; }= new List<string>();

        /// <summary>
        /// 节点列表。
        /// </summary>
        public Dictionary<char, TrieNode> Nodes { get; }= new Dictionary<char, TrieNode>();
        private uint _min = uint.MaxValue;
        private uint _max = uint.MinValue;
        /// <summary>
        /// 通过字符查询当前节点后的字典树。
        /// </summary>
        /// <param name="c">当前字符。</param>
        /// <param name="node">返回字典树。</param>
        /// <returns>返回获取结果。</returns>
        public bool TryGetValue(char c, out TrieNode node)
        {
            if (_min <= (uint)c && _max >= (uint)c)
            {
                return Nodes.TryGetValue(c, out node);
            }
            node = null;
            return false;
        }
        
        /// <summary>
        /// 获取已经存在的字典树或者新添加一个字典树。
        /// </summary>
        /// <param name="c">当前字符。</param>
        /// <returns>返回当前字符对应的字典树。</returns>
        public TrieNode Add(char c)
        {
            if (Nodes.TryGetValue(c, out var node))
            {
                return node;
            }

            if (_min > c) { _min = c; }
            if (_max < c) { _max = c; }

            node = new TrieNode();
            Nodes[c] = node;
            return node;
        }

        /// <summary>
        /// 设置结果字符串。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        public void SetResults(string text)
        {
            if (End == false)
            {
                End = true;
            }
            if (Results.Contains(text) == false)
            {
                Results.Add(text);
            }
        }

        /// <summary>
        /// 拼接字典树实例。
        /// </summary>
        /// <param name="node">字典树实例。</param>
        /// <param name="links">关联的字典树列表。</param>
        public void Merge(TrieNode node, Dictionary<TrieNode, TrieNode> links)
        {
            if (node.End)
            {
                if (End == false)
                {
                    End = true;
                }
                foreach (var item in node.Results)
                {
                    if (Results.Contains(item) == false)
                    {
                        Results.Add(item);
                    }
                }
            }

            foreach (var item in node.Nodes)
            {

                if (Nodes.ContainsKey(item.Key) == false)
                {
                    if (_min > item.Key) { _min = item.Key; }
                    if (_max < item.Key) { _max = item.Key; }
                    Nodes[item.Key] = item.Value;
                }
            }

            if (links.TryGetValue(node, out var node2))
            {
                Merge(node2, links);
            }
        }

        /// <summary>
        /// 将当前节点列表转换为字典树列表。
        /// </summary>
        /// <returns>字典树列表。</returns>
        public TrieNode[] ToArray()
        {
            var first = new TrieNode[char.MaxValue + 1];
            foreach (var item in Nodes)
            {
                first[item.Key] = item.Value;
            }
            return first;
        }
    }
}

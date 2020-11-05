using System.Collections.Generic;
using System.Text;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 文本搜索
    /// </summary>
    public class StringSearch
    {
        private TrieNode[] _first = new TrieNode[char.MaxValue + 1];
        /// <summary>
        /// 设置关键字
        /// </summary>
        /// <param name="keywords">关键字列表</param>
        public virtual void SetKeywords(IEnumerable<string> keywords)
        {
            var first = new TrieNode[char.MaxValue + 1];
            var root = new TrieNode();

            foreach (var p in keywords)
            {
                if (string.IsNullOrEmpty(p)) continue;

                var nd = _first[p[0]];
                if (nd == null)
                {
                    nd = root.Add(p[0]);
                    first[p[0]] = nd;
                }
                for (var i = 1; i < p.Length; i++)
                {
                    nd = nd.Add(p[i]);
                }
                nd.SetResults(p);
            }
            _first = first;// root.ToArray();

            var links = new Dictionary<TrieNode, TrieNode>();
            foreach (var item in root.Nodes)
            {
                TryLinks(item.Value, null, links);
            }

            foreach (var item in links)
            {
                item.Key.Merge(item.Value, links);
            }

            //_root = root;
        }

        private void TryLinks(TrieNode node, TrieNode node2, Dictionary<TrieNode, TrieNode> links)
        {
            foreach (var item in node.Nodes)
            {
                TrieNode tn = null;
                if (node2 == null)
                {
                    tn = _first[item.Key];
                    if (tn != null)
                    {
                        links[item.Value] = tn;
                    }
                }
                else if (node2.TryGetValue(item.Key, out tn))
                {
                    links[item.Value] = tn;
                }
                TryLinks(item.Value, tn, links);
            }
        }

        /// <summary>
        /// 在文本中查找第一个关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回第一个关键词。</returns>
        public string FindFirst(string text)
        {
            TrieNode ptr = null;
            foreach (var t in text)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = _first[t];
                }
                else
                {
                    if (ptr.TryGetValue(t, out tn) == false)
                    {
                        tn = _first[t];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        return tn.Results[0];
                    }
                }
                ptr = tn;
            }
            return null;
        }

        /// <summary>
        /// 在文本中查找所有的关键词。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回所有关键词列表。</returns>
        public List<string> FindAll(string text)
        {
            TrieNode ptr = null;
            var list = new List<string>();

            foreach (var t in text)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = _first[t];
                }
                else
                {
                    if (ptr.TryGetValue(t, out tn) == false)
                    {
                        tn = _first[t];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        foreach (var item in tn.Results)
                        {
                            list.Add(item);
                        }
                    }
                }
                ptr = tn;
            }
            return list;
        }

        /// <summary>
        /// 判断文本是否包含关键字。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <returns>返回判断结果。</returns>
        public bool Contains(string text)
        {
            TrieNode ptr = null;
            foreach (var t in text)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = _first[t];
                }
                else
                {
                    if (ptr.TryGetValue(t, out tn) == false)
                    {
                        tn = _first[t];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        return true;
                    }
                }
                ptr = tn;
            }
            return false;
        }

        /// <summary>
        /// 在文本中替换所有的关键字。
        /// </summary>
        /// <param name="text">文本字符串。</param>
        /// <param name="replaceChar">替换字符。</param>
        /// <returns>返回替换结果。</returns>
        public string Replace(string text, char replaceChar = '*')
        {
            var result = new StringBuilder(text);

            TrieNode ptr = null;
            for (var i = 0; i < text.Length; i++)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = _first[text[i]];
                }
                else
                {
                    if (ptr.TryGetValue(text[i], out tn) == false)
                    {
                        tn = _first[text[i]];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        var maxLength = tn.Results[0].Length;
                        var start = i + 1 - maxLength;
                        for (var j = start; j <= i; j++)
                        {
                            result[j] = replaceChar;
                        }
                    }
                }
                ptr = tn;
            }
            return result.ToString();
        }
    }
}

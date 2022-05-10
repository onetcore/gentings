using System.Collections;

namespace Gentings.Text
{
    /// <summary>
    /// 文字。
    /// </summary>
    public class Character : IEnumerable<Letter>
    {
        private readonly IList<Letter> _letters = new List<Letter>();
        internal Character(string character)
        {
            var parts = character.Split(':');
            Code = (char)Convert.ToInt32(parts[0]);
            foreach (var letters in parts[1].Split(','))
            {
                _letters.Add(new Letter(letters));
            }
        }

        /// <summary>
        /// Unicode字符串。
        /// </summary>
        public char Code { get; }

        /// <summary>
        /// 是否为多音字。
        /// </summary>
        public bool IsMultiple => _letters.Count > 1;

        /// <summary>
        /// 循环迭代器。
        /// </summary>
        /// <returns>返回循环的迭代实例。</returns>
        public IEnumerator<Letter> GetEnumerator()
        {
            return _letters.GetEnumerator();
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return Code.ToString();
        }

        /// <summary>
        /// [音标]获取文字拼音字母。
        /// </summary>
        /// <param name="character">文字。</param>
        /// <param name="multiple">是否输出多音字拼音，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音。
        /// <returns>文字拼音字母。</returns>
        public string? GetPhonetic(bool multiple = false)
        {
            return multiple ? string.Join(Alphabet.MultipleSeparater, _letters.Select(x => x.DisplayName)) : _letters.FirstOrDefault()?.DisplayName;
        }

        /// <summary>
        /// 获取文字拼音字母。
        /// </summary>
        /// <param name="character">文字。</param>
        /// <param name="multiple">是否输出多音字拼音，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音。
        /// <returns>文字拼音字母。</returns>
        public string? GetLetters(bool multiple = false)
        {
            return multiple ? string.Join(Alphabet.MultipleSeparater, _letters.Select(x => x.Name)) : _letters.FirstOrDefault()?.Name;
        }

        /// <summary>
        /// 获取文字拼音首字母。
        /// </summary>
        /// <param name="character">文字。</param>
        /// <param name="multiple">是否输出多音字拼音，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音。
        /// <returns>文字拼音首字母。</returns>
        public string? GetFirstLetters(bool multiple = false)
        {
            return multiple ? string.Join(Alphabet.MultipleSeparater, _letters.Select(x => x.First)) : _letters.FirstOrDefault()?.First.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
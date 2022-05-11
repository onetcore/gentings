using System.Collections.Concurrent;
using System.Text;

namespace Gentings.Text
{
    /// <summary>
    /// 字母表。
    /// </summary>
    public static class Alphabet
    {
        /// <summary>
        /// 多音字分隔符。
        /// </summary>
        public const char MultipleSeparater = '|';
        private static readonly ConcurrentDictionary<char, Character> _alphabet = new();

        static Alphabet()
        {
            var parts = Unicodes.Defines.Split('|');
            foreach (var part in parts)
            {
                var character = new Character(part);
                _alphabet[character.Code] = character;
            }
        }

        /// <summary>
        /// 获取字符集数量。
        /// </summary>
        public static int Count => _alphabet.Count;

        /// <summary>
        /// 尝试获取字符实例对象。
        /// </summary>
        /// <param name="code">Unicode对应的十进制编码。</param>
        /// <param name="character">字符实例对象。</param>
        /// <returns>返回获取结果。</returns>
        public static bool TryGetCharacter(char code, out Character? character)
        {
            return _alphabet.TryGetValue(code, out character);
        }

        /// <summary>
        /// [音标]获取文字拼音字母，多个文字中间使用“ ”间隔。
        /// </summary>
        /// <param name="characters">文字字符串。</param>
        /// <param name="multiple">是否输出多音字拼音，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音。
        /// <returns>文字拼音字母，多个文字中间使用“ ”间隔。</returns>
        public static string? GetPhonetic(string? characters, bool multiple = false)
        {
            if (string.IsNullOrWhiteSpace(characters))
                return null;
            characters = characters.Trim();
            var builder = new StringBuilder();
            foreach (var character in characters)
            {
                //英文字母或者空格
                if (character >= 'a' && character <= 'z' || character >= 'A' && character <= 'Z' || character == ' ')
                {
                    builder.Append(character);
                    continue;
                }

                //中文拼音
                if (TryGetCharacter(character, out var letters))
                {
                    builder.Append(' ').Append(letters!.GetPhonetic(multiple)).Append(' ');
                }

                //其他不管
            }
            builder.Replace("  ", " ");
            return builder.ToString().Trim();
        }

        /// <summary>
        /// [音标]获取文字拼音字母。
        /// </summary>
        /// <param name="character">文字。</param>
        /// <param name="multiple">是否输出多音字拼音，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音。
        /// <returns>文字拼音字母。</returns>
        public static string? GetPhonetic(char character, bool multiple = false)
        {
            //英文字母或者空格
            if (character >= 'a' && character <= 'z' || character >= 'A' && character <= 'Z' || character == ' ')
                return character.ToString();

            //中文拼音
            if (TryGetCharacter(character, out var letters))
                return letters!.GetPhonetic(multiple);

            //其他不管
            return null;
        }

        /// <summary>
        /// 获取文字拼音字母，多个文字中间使用“ ”间隔。
        /// </summary>
        /// <param name="characters">文字字符串。</param>
        /// <param name="multiple">是否输出多音字拼音，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音。
        /// <returns>文字拼音字母，多个文字中间使用“ ”间隔。</returns>
        public static string? GetLetters(string? characters, bool multiple = false)
        {
            if (string.IsNullOrWhiteSpace(characters))
                return null;
            characters = characters.Trim();
            var builder = new StringBuilder();
            foreach (var character in characters)
            {
                //英文字母或者空格
                if (character >= 'a' && character <= 'z' || character >= 'A' && character <= 'Z' || character == ' ')
                {
                    builder.Append(character);
                    continue;
                }

                //中文拼音
                if (TryGetCharacter(character, out var letters))
                {
                    builder.Append(' ').Append(letters!.GetLetters(multiple)).Append(' ');
                }

                //其他不管
            }

            builder.Replace("  ", " ");
            return builder.ToString().Trim();
        }

        /// <summary>
        /// 获取文字拼音字母。
        /// </summary>
        /// <param name="character">文字。</param>
        /// <param name="multiple">是否输出多音字拼音，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音。
        /// <returns>文字拼音字母。</returns>
        public static string? GetLetters(char character, bool multiple = false)
        {
            //英文字母或者空格
            if (character >= 'a' && character <= 'z' || character >= 'A' && character <= 'Z' || character == ' ')
                return character.ToString();

            //中文拼音
            if (TryGetCharacter(character, out var letters))
                return letters!.GetLetters(multiple);

            //其他不管
            return null;
        }

        /// <summary>
        /// 获取文字拼音首字母集合。
        /// </summary>
        /// <param name="characters">文字字符串。</param>
        /// <param name="multiple">是否输出多音字拼音首字母，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音首字母。
        /// <returns>获取文字拼音首字母集合。</returns>
        public static string? GetFirstLetters(string? characters, bool multiple = false)
        {
            if (string.IsNullOrWhiteSpace(characters))
                return null;
            characters = characters.Trim();
            var builder = new StringBuilder();
            var next = true;
            foreach (var character in characters)
            {
                //空格下一个英文字母为首字母
                if (character == ' ')
                {
                    next = true;
                    continue;
                }

                //英文字母或者空格
                if (next && (character >= 'a' && character <= 'z' || character >= 'A' && character <= 'Z'))
                {
                    builder.Append(char.ToUpper(character));
                    next = false;
                    continue;
                }

                //中文拼音
                if (TryGetCharacter(character, out var letters))
                {
                    builder.Append(letters!.GetFirstLetters(multiple));
                }

                //其他不管
            }
            return builder.ToString().Trim();
        }

        /// <summary>
        /// 获取文字拼音首字母。
        /// </summary>
        /// <param name="character">文字。</param>
        /// <param name="multiple">是否输出多音字拼音，如果输出将使用<seealso cref="MultipleSeparater"/></param>进行分隔，否则只输出第一个拼音首字母。
        /// <returns>文字拼音首字母。</returns>
        public static string? GetFirstLetters(char character, bool multiple = false)
        {
            //英文字母或者空格
            if (character >= 'a' && character <= 'z' || character >= 'A' && character <= 'Z')
                return character.ToString().ToUpper();

            //中文拼音
            if (TryGetCharacter(character, out var letters))
                return letters!.GetFirstLetters(multiple);

            //其他不管
            return null;
        }

        /// <summary>
        /// 是否包含多音字。
        /// </summary>
        /// <param name="letters">当前拼音字母集合。</param>
        /// <returns>返回判断结果。</returns>
        public static bool IsAlphabetMultiple(this string? letters)
        {
            return letters?.IndexOf(MultipleSeparater) > 0;
        }
    }
}
namespace Gentings.Text
{
    /// <summary>
    /// 拼音字母。
    /// </summary>
    public class Letter
    {
        internal Letter(string letters)
        {
            var parts = letters.Split('.');
            DisplayName = parts[0];
            var name = parts[1];
            Name = name[0..^1];
            First = char.ToUpper(name[0]);
            Tone = Convert.ToInt16(name[^1].ToString());
        }

        /// <summary>
        /// 含音标的拼音字母。
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// 英文字母。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 首字母。
        /// </summary>
        public char First { get; }

        /// <summary>
        /// 声调。
        /// </summary>
        public short Tone { get; }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
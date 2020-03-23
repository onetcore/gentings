namespace Gentings.ConsoleApp
{
    /// <summary>
    /// 可读取接口。
    /// </summary>
    public interface IByteReader
    {
        /// <summary>
        /// 实例化数据结构。
        /// </summary>
        /// <param name="reader">当前字节读取器。</param>
        void Init(ByteReader reader);
    }
}
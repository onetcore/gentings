namespace Gentings.ConsoleApp
{
    /// <summary>
    /// 字节封包接口。
    /// </summary>
    public interface IBytePackage
    {
        /// <summary>
        /// 实例化数据结构。
        /// </summary>
        /// <param name="reader">当前字节读取器。</param>
        void Init(ByteReader reader);

        /// <summary>
        /// 写入数据包中。
        /// </summary>
        /// <param name="writer">字节写入器实例。</param>
        void Write(ByteWriter writer);
    }
}
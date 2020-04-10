namespace Gentings.Sockets
{
    /// <summary>
    /// 可写入接口。
    /// </summary>
    public interface IByteWriter
    {
        /// <summary>
        /// 写入数据包中。
        /// </summary>
        /// <param name="writer">字节写入器实例。</param>
        void Write(ByteWriter writer);
    }
}
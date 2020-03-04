using System;
using System.Net.Sockets;

namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// CMPP扩展类。
    /// </summary>
    public static class CMPPExtenions
    {
        /// <summary>
        /// 将日期转换为时间撮。
        /// </summary>
        /// <param name="timestamp">日期时间。</param>
        /// <returns>返回时间戳。</returns>
        public static string ToTimestamp(this DateTime timestamp) => timestamp.ToString("MMddHHmmss");

        /// <summary>
        /// 读取网络流数据字节数组实例。
        /// </summary>
        /// <param name="stream">当前网络流实例。</param>
        /// <param name="length">读取最大长度。</param>
        /// <param name="bufferSize">每次读取的缓存大小。</param>
        /// <returns>返回当前读取的字节数组实例。</returns>
        public static byte[] ReadBytes(this NetworkStream stream, int length, int bufferSize)
        {
            var bytes = new byte[length];
            var size = 0;

            do
            {
                var buffer = new byte[bufferSize];
                int current = stream.Read(buffer, 0, buffer.Length);
                if (current > 0)
                {
                    Buffer.BlockCopy(buffer, 0, bytes, size, current);
                    size += current;
                }
            } while (stream.DataAvailable);

            var result = new byte[size];
            Buffer.BlockCopy(bytes, 0, result, 0, result.Length);
            return result;
        }

        /// <summary>
        /// 将实例写入到网络流中。
        /// </summary>
        /// <param name="stream">当前网络流实例对象。</param>
        /// <param name="buffer">写入数据实例。</param>
        public static void WriteBytes(this NetworkStream stream, byte[] buffer)
        {
            if (stream.CanWrite)
                stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 将实例写入到网络流中。
        /// </summary>
        /// <param name="stream">当前网络流实例对象。</param>
        /// <param name="buffer">写入数据实例。</param>
        public static void WritePackage(this NetworkStream stream, IPackage package)
        {
            if (stream.CanWrite)
            {
                var buffer = package.ToBytes();
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}

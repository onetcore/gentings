using Gentings.Messages.CMPP.Packaging;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

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
        public static async Task<byte[]> ReadBytesAsync(this NetworkStream stream)
        {
            if (!stream.CanRead)
            {
                return null;
            }

            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);
            while (stream.DataAvailable)
            {
                var buffer = new byte[PackageHeader.Size];
                int current = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (current > 0)
                {
                    writer.Write(buffer);
                }
            }
            return ms.ToArray();
        }

        /// <summary>
        /// 读取网络流数据消息实例。
        /// </summary>
        /// <param name="stream">当前网络流实例。</param>
        /// <returns>返回当前读取消息实例。</returns>
        public static async Task<TMessage> ReadMessageAsync<TMessage>(this NetworkStream stream)
            where TMessage : class, IMessage
        {
            var buffer = await stream.ReadBytesAsync();
            if (buffer == null)
            {
                return default;
            }

            return Activator.CreateInstance(typeof(TMessage), new[] { buffer }) as TMessage;
        }

        /// <summary>
        /// 将实例写入到网络流中。
        /// </summary>
        /// <param name="stream">当前网络流实例对象。</param>
        /// <param name="buffer">写入数据实例。</param>
        public static async Task WriteBytesAsync(this NetworkStream stream, byte[] buffer)
        {
            if (stream.CanWrite)
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 将实例写入到网络流中。
        /// </summary>
        /// <param name="stream">当前网络流实例对象。</param>
        /// <param name="buffer">写入数据实例。</param>
        public static async Task WritePackageAsync(this NetworkStream stream, IPackage package)
        {
            if (stream.CanWrite)
            {
                var buffer = package.ToBytes();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}

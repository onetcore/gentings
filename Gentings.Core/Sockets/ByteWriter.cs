using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gentings.Sockets
{
    /// <summary>
    /// 字节写入辅助类。
    /// </summary>
    public class ByteWriter : IDisposable, IAsyncDisposable
    {
        private readonly Encoding _encoding;
        private readonly BinaryWriter _writer;

        /// <summary>
        /// 初始化类<see cref="ByteWriter"/>。
        /// </summary>
        /// <param name="output">流实例。</param>
        public ByteWriter(Stream output)
            : this(output, Encoding.Default)
        {
        }

        /// <summary>
        /// 初始化类<see cref="ByteWriter"/>。
        /// </summary>
        /// <param name="output">流实例。</param>
        /// <param name="encoding">编码。</param>
        /// <param name="leaveOpen">当释放<see cref="ByteWriter"/>后，流实例是否保持打开状态。</param>
        public ByteWriter(Stream output, Encoding encoding, bool leaveOpen = false)
        {
            _encoding = encoding;
            _writer = new BinaryWriter(output, encoding, leaveOpen);
        }

        /// <summary>
        /// 写入一个字节数值。
        /// </summary>
        /// <param name="value">当前整型值。</param>
        public void Write(byte value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// 写入<paramref name="size"/>大小的整型数值。
        /// </summary>
        /// <param name="value">当前整型值。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(ushort value, int size = 2)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            _writer.Write(bytes, 0, size);
        }

        /// <summary>
        /// 写入<paramref name="size"/>大小的整型数值。
        /// </summary>
        /// <param name="value">当前整型值。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(short value, int size = 2)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write(bytes, 0, size);
        }

        /// <summary>
        /// 写入<paramref name="size"/>大小的整型数值。
        /// </summary>
        /// <param name="value">当前整型值。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(uint value, int size = 4)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write(bytes, 0, size);
        }

        /// <summary>
        /// 写入<paramref name="size"/>大小的整型数值。
        /// </summary>
        /// <param name="value">当前整型值。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(int value, int size = 4)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write(bytes, 0, size);
        }

        /// <summary>
        /// 写入<paramref name="size"/>大小的长整型数值。
        /// </summary>
        /// <param name="value">当前长整型值。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(ulong value, int size = 8)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write(bytes, 0, size);
        }

        /// <summary>
        /// 写入<paramref name="size"/>大小的长整型数值。
        /// </summary>
        /// <param name="value">当前长整型值。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(long value, int size = 8)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write(bytes, 0, size);
        }

        /// <summary>
        /// 写入字符串。
        /// </summary>
        /// <param name="value">当前字符串。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(string value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// 写入<paramref name="size"/>大小的字符串。
        /// </summary>
        /// <param name="value">当前字符串。</param>
        /// <param name="size">大小。</param>
        /// <param name="encoding">字符集。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(string value, int size, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(value))
                Zero(size);
            else
            {
                var bytes = (encoding ?? _encoding).GetBytes(value);
                Write(bytes, 0, size);
            }
        }

        /// <summary>
        /// 写入空字节实例。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Zero(int size)
        {
            Write(new byte[size]);
        }

        /// <summary>
        /// 将流长度写到包前面位置中，需要在最开始写包时写入一个长度值。
        /// </summary>
        /// <param name="bytes">长度字节实例。</param>
        public void Sized(byte[] bytes)
        {
            _writer.BaseStream.Position = 0;
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write(bytes);
            _writer.BaseStream.Position = _writer.BaseStream.Length - 1;
        }

        /// <summary>
        /// 写入字节数组。
        /// </summary>
        /// <param name="value">当前字符串。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(ReadOnlySpan<byte> value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// 写入字节数组。
        /// </summary>
        /// <param name="value">当前字符串。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(byte[] value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// 写入<paramref name="size"/>大小的字节数组。
        /// </summary>
        /// <param name="value">当前字符串。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(byte[] value, int size) => Write(value, 0, size);

        /// <summary>
        /// 写入<paramref name="size"/>大小的字节数组。
        /// </summary>
        /// <param name="value">当前字符串。</param>
        /// <param name="offset">偏移量。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回当前实例。</returns>
        public void Write(byte[] value, int offset, int size)
        {
            if (value.Length >= size + offset)
                _writer.Write(value, offset, size);
            else
            {
                _writer.Write(value, offset, value.Length - offset);
                Zero(size - value.Length + offset);
            }
        }

        /// <summary>
        /// 关闭流。
        /// </summary>
        public void Close()
        {
            _writer.Close();
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            _writer.Dispose();
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public ValueTask DisposeAsync()
        {
            return _writer.DisposeAsync();
        }

        /// <summary>
        /// 大小。
        /// </summary>
        public long Length => _writer.BaseStream.Length;

        /// <summary>
        /// 当前流实例对象。
        /// </summary>
        public Stream Stream => _writer.BaseStream;
    }
}
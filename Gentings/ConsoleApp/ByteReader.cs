using System;
using System.Buffers;
using System.Text;

namespace Gentings.ConsoleApp
{
    /// <summary>
    /// 字节读取器。
    /// </summary>
    public class ByteReader
    {
        private SequencePosition _position;
        private readonly ReadOnlySequence<byte> _sequence;
        /// <summary>
        /// 初始化类<see cref="ByteReader"/>。
        /// </summary>
        /// <param name="sequence">只读字节内存片段。</param>
        public ByteReader(ReadOnlySequence<byte> sequence)
        {
            _sequence = sequence;
            _position = Start = _sequence.Start;
            End = sequence.End;
        }

        /// <summary>
        /// 初始化类<see cref="ByteReader"/>，并设置<paramref name="reader"/>序列位置前进<paramref name="length"/>字节。
        /// </summary>
        /// <param name="reader">字节读取实例。</param>
        /// <param name="length">从<paramref name="reader"/>中读取字节数量。</param>
        public ByteReader(ByteReader reader, int length)
        {
            _sequence = reader._sequence.Slice(reader._position, length);
            _position = Start = _sequence.Start;
            End = _sequence.End;
            reader._position = reader._sequence.GetPosition(length, reader._position);
        }

        /// <summary>
        /// 初始化类<see cref="ByteReader"/>，并设置<paramref name="reader"/>序列位置前进<paramref name="length"/>字节。
        /// </summary>
        /// <param name="reader">字节读取实例。</param>
        /// <param name="length">从<paramref name="reader"/>中读取字节数量。</param>
        public ByteReader(ByteReader reader, long length)
        {
            _sequence = reader._sequence.Slice(reader._position, length);
            _position = _sequence.Start;
            reader._position = reader._sequence.GetPosition(length, reader._position);
        }

        /// <summary>
        /// 读取一个整形实例。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <returns>返回读取值。</returns>
        public short ReadInt16(int size = 2)
        {
            return BitConverter.ToInt16(Read(size));
        }

        /// <summary>
        /// 读取一个无符号整形实例。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <returns>返回读取值。</returns>
        public ushort ReadUInt16(int size = 2)
        {
            return BitConverter.ToUInt16(Read(size));
        }

        /// <summary>
        /// 读取一个整形实例。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <returns>返回读取值。</returns>
        public int ReadInt32(int size = 4)
        {
            return BitConverter.ToInt32(Read(size));
        }

        /// <summary>
        /// 读取一个无符号整形实例。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <returns>返回读取值。</returns>
        public uint ReadUInt32(int size = 4)
        {
            return BitConverter.ToUInt32(Read(size));
        }

        /// <summary>
        /// 读取一个长整形实例。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <returns>返回读取值。</returns>
        public long ReadInt64(int size = 8)
        {
            return BitConverter.ToInt32(Read(size));
        }

        /// <summary>
        /// 读取一个无符号长整形实例。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <returns>返回读取值。</returns>
        public ulong ReadUInt64(int size = 8)
        {
            return BitConverter.ToUInt32(Read(size));
        }

        /// <summary>
        /// 读取字符串。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <param name="encoding">编码。</param>
        /// <returns>返回当前字符串实例。</returns>
        public string ReadString(int size, Encoding encoding = default)
        {
            var bytes = Read(size);
            encoding = encoding ?? Encoding.Default;
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 读取一个字节。
        /// </summary>
        /// <returns>返回读取结果。</returns>
        public byte ReadByte()
        {
            return Read(1)[0];
        }

        /// <summary>
        /// 读取字节。
        /// </summary>
        /// <param name="size">当前字节数。</param>
        /// <returns>返回读取结果。</returns>
        public byte[] ReadBytes(int size) => Read(size).ToArray();

        /// <summary>
        /// 读取当前只读片段。
        /// </summary>
        /// <param name="size">当前字节数。</param>
        /// <returns>返回读取结果。</returns>
        public ReadOnlySpan<byte> Read(int size)
        {
            var result = _sequence.Slice(_position, size).FirstSpan;
            _position = _sequence.GetPosition(size, _position);
            return result;
        }

        /// <summary>
        /// 起始位置。
        /// </summary>
        public SequencePosition Start { get; }

        /// <summary>
        /// 结束位置。
        /// </summary>
        public SequencePosition End { get; }

        /// <summary>
        /// 判断是否结尾。
        /// </summary>
        public bool IsEnd => _position.Equals(End);
    }
}
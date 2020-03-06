using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Gentings.Messages.CMPP.Packaging
{

    /// <summary>
    /// 接收连接包实例。
    /// </summary>
    public class ConnectMessage : IMessage
    {
        /// <summary>
        /// 包大小。
        /// </summary>
        public const int Size = 4 + 16 + 1;

        /// <summary>
        /// 16 Octet String ISMG认证码,用于鉴别ISMG。 
        ///   其值通过单向MD5 hash计算得出,表示如下: 
        ///   AuthenticatorISMG =MD5(Status+AuthenticatorSource+shared secret),Shared secret 由中国移动与源地址实体事先商定,AuthenticatorSource为源地址实体发送给ISMG的对应消息CMPP_Connect中的值。 
        ///    认证出错时,此项为空。
        /// </summary>
        public byte[] AuthenticatorISMG { get; }

        /// <summary>
        /// 4 Unsigned Integer 状态。
        ///  0：正确
        ///  1：消息结构错
        ///  2：非法源地址
        ///  3：认证错
        ///  4：版本太高
        ///  5~：其他错误
        /// </summary>
        public ConnectStatus Status { get; }

        /// <summary>
        /// 1 Unsigned Integer 服务器支持的最高版本号,对于3.0的版本,高4bit为3,低4位为0。
        /// </summary>
        public uint Version { get; }

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; }

        /// <summary>
        /// 判断是否验证正确。
        /// </summary>
        /// <param name="password">密码。</param>
        /// <param name="authenticatorSource">验证加密字节数组。</param>
        /// <returns>返回判断结果。</returns>
        public bool IsValid(string password, byte[] authenticatorSource)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write((uint)Status);
                writer.Write(authenticatorSource);
                writer.Write(Encoding.ASCII.GetBytes(password));
                var buffer = ms.ToArray();
                var current = new MD5CryptoServiceProvider().ComputeHash(buffer, 0, buffer.Length);
                return Array.Equals(current, AuthenticatorISMG);
            }
        }

        /// <summary>
        /// 解析连接包实例。
        /// </summary>
        /// <param name="bytes">连接返回包字节数组。</param>
        public ConnectMessage(byte[] bytes)
        {
            //header 12 
            int i = 0;
            var buffer = new byte[PackageHeader.Size];
            Buffer.BlockCopy(bytes, 0, buffer, 0, buffer.Length);
            Header = new PackageHeader(buffer);

            //status 4 
            i += PackageHeader.Size;
            buffer = new byte[4];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            Status = (ConnectStatus)BitConverter.ToUInt32(buffer, 0);

            //AuthenticatorISMG 16 
            i += 4;
            AuthenticatorISMG = new byte[16];
            Buffer.BlockCopy(bytes, i, AuthenticatorISMG, 0, AuthenticatorISMG.Length);

            //version 
            i += 16;
            Version = bytes[i];
        }

        /// <summary>
        /// 包头是否合法。
        /// </summary>
        /// <param name="header">发送包头实例。</param>
        /// <returns>判断包头是否合法。</returns>
        public bool IsHeader(PackageHeader header)
        {
            return header.SequenceId == Header.SequenceId && Header.CommandId == CMPPCommand.CMPP_CONNECT_RESP;
        }
    }
}

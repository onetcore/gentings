using System;
using System.Security.Cryptography;
using System.Text;

namespace GCApp.Packaging
{
    /// <summary>
    /// 发送连接到服务商消息包。
    /// </summary>
    public class ConnectPackage : IPackage
    {
        /// <summary>
        /// 连接包大小。
        /// </summary>
        public const int Size = 6 + 16 + 1 + 4;
        /// <summary>
        /// 包总大小。
        /// </summary>
        public const int Length = Size + PackageHeader.Size;

        private readonly string _spid; // 6 Octet String 源地址,此处为SP_Id,即SP的企业代码。 
        private readonly string _password;
        private readonly uint _version; // 1 Unsigned Integer 双方协商的版本号(高位4bit表示主版本号,低位4bit表示次版本号),对于3.0的版本,高4bit为3,低4位为0 
        private readonly uint _timestamp; // 4 Unsigned Integer 时间戳的明文,由客户端产生,格式为MMDDHHMMSS,即月日时分秒,10位数字的整型,右对齐 。

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; }

        /// <summary>
        /// 验证MD5加密字节。
        /// MD5(Source_Addr+9 字节的0 +shared secret+timestamp) 
        /// Shared secret 由中国移动与源地址实体事先商定,
        /// timestamp格式为:MMDDHHMMSS,即月日时分秒,10位。
        /// </summary>
        public byte[] AuthenticatorSource { get; }

        /// <summary>
        /// 初始化连接包实例。
        /// </summary>
        /// <param name="spid">服务提供商Id。</param>
        /// <param name="password">密钥。</param>
        /// <param name="version">版本。</param>
        public ConnectPackage(string spid, string password, uint version)
        {
            _spid = spid;
            _password = password;
            _version = version;

            var timestamp = DateTime.Now.ToTimestamp();
            _timestamp = uint.Parse(timestamp);

            var buffer = new byte[6 + 9 + _password.Length + 10];
            Encoding.ASCII.GetBytes(_spid).CopyTo(buffer, 0);
            Encoding.ASCII.GetBytes(_password).CopyTo(buffer, 6 + 9);
            Encoding.ASCII.GetBytes(timestamp).CopyTo(buffer, 6 + 9 + _password.Length);
            AuthenticatorSource = new MD5CryptoServiceProvider().ComputeHash(buffer, 0, buffer.Length);
            Header = new PackageHeader(Size, CMPPCommand.CMPP_CONNECT, 1);
        }

        /// <summary>
        /// 将包转换为字节数组。
        /// </summary>
        /// <returns>字节数组。</returns>
        public byte[] ToBytes()
        {
            var bytes = new byte[Length];
            var i = 0;

            //header 12 
            var buffer = Header.ToBytes();
            Buffer.BlockCopy(buffer, 0, bytes, 0, buffer.Length);

            //Source_Addr 6 
            i += PackageHeader.Size;
            buffer = Encoding.ASCII.GetBytes(_spid);
            Buffer.BlockCopy(buffer, 0, bytes, i, 6);

            //AuthenticatorSource 16 
            i += 6;
            buffer = AuthenticatorSource;
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //16

            //version 1 
            i += 16;
            bytes[i++] = (byte)_version; //版本

            //Timestamp 
            buffer = BitConverter.GetBytes(_timestamp);
            Array.Reverse(buffer);
            buffer.CopyTo(bytes, i);
            return bytes;
        }
    }
}

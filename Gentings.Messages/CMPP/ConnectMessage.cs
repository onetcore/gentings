using System;

namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// 接收连接包实例。
    /// </summary>
    public class ConnectMessage 
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
        /// </summary>
        public uint Status { get; }

        /// <summary>
        /// 1 Unsigned Integer 服务器支持的最高版本号,对于3.0的版本,高4bit为3,低4位为0。
        /// </summary>
        public uint Version { get; }

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; }

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
            Status = BitConverter.ToUInt32(buffer, 0);

            //AuthenticatorISMG 16 
            i += 4;
            AuthenticatorISMG = new byte[16];
            Buffer.BlockCopy(bytes, i, AuthenticatorISMG, 0, AuthenticatorISMG.Length);

            //version 
            i += 16;
            Version = bytes[i];
        }
    }
}

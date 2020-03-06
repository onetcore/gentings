using System;
using System.Text;

namespace Gentings.Messages.CMPP.Packaging
{
    /// <summary>
    /// 短信下发包实例。
    /// </summary>
    public class DeliverPackage : IPackage
    {
        /// <summary>
        /// 8 Unsigned Integer 信息标识。 
        ///   生成算法如下: 
        ///   采用64位(8字节)的整数: 
        ///   (1)????????? 时间(格式为MMDDHHMMSS,即月日时分秒):bit64~bit39,其中 
        ///   bit64~bit61:月份的二进制表示; 
        ///   bit60~bit56:日的二进制表示; 
        ///   bit55~bit51:小时的二进制表示; 
        ///   bit50~bit45:分的二进制表示; 
        ///   bit44~bit39:秒的二进制表示; 
        ///   (2)????????? 短信网关代码:bit38~bit17,把短信网关的代码转换为整数填写到该字段中; 
        ///   (3)????????? 序列号:bit16~bit1,顺序增加,步长为1,循环使用。 
        ///   各部分如不能填满,左补零,右对齐。 
        /// </summary>
        public ulong MsgId { get; }

        /// <summary>
        /// 21 Octet String 目的号码。 
        ///  SP的服务代码,一般4--6位,或者是前缀为服务代码的长号码;该号码是手机用户短消息的被叫号码。
        /// </summary>
        public string DestId { get; }

        /// <summary>
        /// 10 Octet String 业务标识,是数字、字母和符号的组合。
        /// </summary>
        public string ServiceId { get; }

        /// <summary>
        /// 1 Unsigned Integer GSM协议类型。详细解释请参考GSM03.40中的9.2.3.9。 
        /// </summary>
        public uint Pid { get; }

        /// <summary>
        /// 1 Unsigned Integer GSM协议类型。详细解释请参考GSM03.40中的9.2.3.23,仅使用1位,右对齐。 
        /// </summary>
        public uint Udhi { get; }

        /// <summary>
        /// 1 Unsigned Integer 信息格式: 
        ///   0:ASCII串; 
        ///   3:短信写卡操作; 
        ///   4:二进制信息; 
        ///   8:UCS2编码; 
        ///   15:含GB汉字。
        /// </summary>
        public MsgFormat MsgFmt { get; }

        /// <summary>
        /// 32 Octet String 源终端MSISDN号码(状态报告时填为CMPP_SUBMIT消息的目的终端号码)。 
        /// </summary>
        public string SrcTerminalId { get; }

        /// <summary>
        ///  1 Unsigned Integer 源终端号码类型,0:真实号码;1:伪码。
        /// </summary>
        public uint SrcTerminalType { get; }

        /// <summary>
        /// 1 Unsigned Integer 是否为状态报告: 
        ///   0:非状态报告; 
        ///   1:状态报告。 
        /// </summary>
        public uint RegisteredDelivery { get; }

        /// <summary>
        /// 1 Unsigned Integer 消息长度,取值大于或等于0。
        /// </summary>
        public uint MsgLength { get; private set; }

        /// <summary>
        /// Octet String 消息内容。
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// 20 Octet String 点播业务使用的LinkID,非点播类业务的MT流程不使用该字段。
        /// </summary>
        public string LinkId { get; }

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; private set; }

        public const int FixedBodyLength = 8 // Msg_Id Unsigned Integer 信息标识。 
                                             //   生成算法如下: 
                                             //   采用64位(8字节)的整数: 
                                             //   (1)????????? 时间(格式为MMDDHHMMSS,即月日时分秒):bit64~bit39,其中 
                                             //   bit64~bit61:月份的二进制表示; 
                                             //   bit60~bit56:日的二进制表示; 
                                             //   bit55~bit51:小时的二进制表示; 
                                             //   bit50~bit45:分的二进制表示; 
                                             //   bit44~bit39:秒的二进制表示; 
                                             //   (2)????????? 短信网关代码:bit38~bit17,把短信网关的代码转换为整数填写到该字段中; 
                                             //   (3)????????? 序列号:bit16~bit1,顺序增加,步长为1,循环使用。 
                                             //   各部分如不能填满,左补零,右对齐。 
         + 21 // Dest_Id Octet String 目的号码。 
              //   SP的服务代码,一般4--6位,或者是前缀为服务代码的长号码;该号码是手机用户短消息的被叫号码。 
         + 10 // Service_Id Octet String 业务标识,是数字、字母和符号的组合。 
         + 1 // TP_pid Unsigned Integer GSM协议类型。详细解释请参考GSM03.40中的9.2.3.9。 
         + 1 // TP_udhi Unsigned Integer GSM协议类型。详细解释请参考GSM03.40中的9.2.3.23,仅使用1位,右对齐。 
         + 1 // Msg_Fmt Unsigned Integer 信息格式: 
             //   0:ASCII串; 
             //   3:短信写卡操作; 
             //   4:二进制信息; 
             //   8:UCS2编码; 
             //   15:含GB汉字。 
         + 32 // Src_terminal_Id Octet String 源终端MSISDN号码(状态报告时填为CMPP_SUBMIT消息的目的终端号码)。 
         + 1 // Src_terminal_type Unsigned Integer 源终端号码类型,0:真实号码;1:伪码。 
         + 1 // Registered_Delivery Unsigned Integer 是否为状态报告: 
             //   0:非状态报告; 
             //   1:状态报告。 
         + 1 // Msg_Length Unsigned Integer 消息长度,取值大于或等于0。 
             //Msg_length // Msg_Content Octet String 消息内容。 
         + 20; // LinkID Octet String 点播业务使用的LinkID,非点播类业务的MT流程不使用该字段。
        /// <summary>
        /// 包大小。
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 解析提交包实例。
        /// </summary>
        /// <param name="bytes">提交返回包字节数组。</param>
        public DeliverPackage(byte[] bytes)
        {
            int i = 0;
            byte[] buffer = new byte[PackageHeader.Size];
            Buffer.BlockCopy(bytes, 0, buffer, 0, PackageHeader.Size);
            Header = new PackageHeader(buffer);

            //Msg_Id 8 
            i += PackageHeader.Size;
            buffer = new byte[8];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            MsgId = BitConverter.ToUInt64(buffer, 0);

            //Dest_Id 21 
            i += 8;
            buffer = new byte[21];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            DestId = Encoding.ASCII.GetString(buffer).Trim();

            //Service_Id 20 
            i += 21;
            buffer = new byte[10];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            ServiceId = Encoding.ASCII.GetString(buffer).Trim();

            //TP_pid 1 
            i += 10;
            Pid = bytes[i++];
            Udhi = bytes[i++];
            MsgFmt = (MsgFormat)bytes[i++];

            //Src_terminal_Id 32 
            buffer = new byte[32];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            SrcTerminalId = Encoding.ASCII.GetString(buffer).Trim();


            //Src_terminal_type 1 
            i += 32;
            SrcTerminalType = bytes[i++];
            RegisteredDelivery = bytes[i++];
            MsgLength = bytes[i++];

            //Msg_Content 
            buffer = new byte[MsgLength];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            switch (MsgFmt)
            {
                case MsgFormat.UCS2:
                    Content = Encoding.BigEndianUnicode.GetString(buffer).Trim();
                    break;
                case MsgFormat.BG2312: //gb2312 
                    Content = Encoding.GetEncoding("gb2312").GetString(buffer).Trim();
                    break;
                case MsgFormat.ASCII: //ascii 
                case MsgFormat.Card: //短信写卡操作 
                case MsgFormat.Binary: //二进制信息 
                default:
                    Content = Encoding.ASCII.GetString(buffer).ToString();
                    break;
            }


            //Linkid 20 
            i += (int)MsgLength;
            buffer = new byte[20];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            LinkId = Encoding.ASCII.GetString(buffer).Trim();

        }

        /// <summary>
        /// 将包转换为字节数组。
        /// </summary>
        /// <returns>字节数组。</returns>
        public byte[] ToBytes()
        { //Msg_Length Msg_Content 
            byte[] buf;
            switch (MsgFmt)
            {
                case MsgFormat.UCS2:
                    buf = Encoding.BigEndianUnicode.GetBytes(Content);
                    break;
                case MsgFormat.BG2312: //gb2312 
                    buf = Encoding.GetEncoding("gb2312").GetBytes(Content);
                    break;
                case MsgFormat.ASCII: //ascii 
                case MsgFormat.Card: //短信写卡操作 
                case MsgFormat.Binary: //二进制信息 
                default:
                    buf = Encoding.ASCII.GetBytes(Content);
                    break;
            }

            MsgLength = (uint)buf.Length;
            Size = FixedBodyLength + (int)MsgLength;
            Header = new PackageHeader((uint)Size, CMPPCommand.CMPP_DELIVER, 0);

            byte[] bytes = new byte[PackageHeader.Size + Size];

            int i = 0;
            //header 12 

            //Msg_Id 8 
            i += PackageHeader.Size;
            byte[] buffer = BitConverter.GetBytes(MsgId);
            Array.Reverse(buffer);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length);

            //Dest_Id 21 
            i += 8;
            buffer = new byte[21];
            buffer = Encoding.ASCII.GetBytes(DestId);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length);

            //Service_Id 10 
            i += 21;
            buffer = new byte[10];
            buffer = Encoding.ASCII.GetBytes(ServiceId);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length);

            //TP_pid 1 
            i += 10;
            bytes[i++] = (byte)Pid;
            bytes[i++] = (byte)Udhi;
            bytes[i++] = (byte)MsgFmt;

            //Src_terminal_Id 32 
            buffer = Encoding.ASCII.GetBytes(SrcTerminalId);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length);

            //Src_terminal_type 1 
            i += 32;
            bytes[i++] = (byte)SrcTerminalType;
            bytes[i++] = (byte)RegisteredDelivery;
            bytes[i++] = (byte)MsgLength;

            //Msg_Content 
            Buffer.BlockCopy(buf, 0, bytes, i, buf.Length);

            //LinkID 
            i += (int)MsgLength;

            return bytes;
        }
    }
}

using System;
using System.ComponentModel;
using System.Text;

namespace Gentings.Messages.CMPP.Packaging
{
    /// <summary>
    /// 提交短信包。
    /// </summary>
    public class SubmitPackage : IPackage
    {
        /// <summary>
        /// 消息包大小。
        /// </summary>
        public int Size { get; private set; }
        //without _Dest_terminal_Id Msg_Content 
        public const int FixedBodyLength = 8// 8 Unsigned Integer 信息标识。
         + 1// 1 Unsigned Integer 相同Msg_Id的信息总条数,从1开始。
         + 1// 1 Unsigned Integer 相同Msg_Id的信息序号,从1开始。 
         + 1// 1 Unsigned Integer 是否要求返回状态确认报告: 
            //   0:不需要; 
            //   1:需要。  
         + 1// 1 Unsigned Integer 信息级别。
         + 10 // 10 Octet String 业务标识,是数字、字母和符号的组合。
         + 1// 1 Unsigned Integer 计费用户类型字段: 
            //   0:对目的终端MSISDN计费; 
            //   1:对源终端MSISDN计费; 
            //   2:对SP计费; 
            //   3:表示本字段无效,对谁计费参见Fee_terminal_Id字段。 
         + 32// 32 Octet String 被计费用户的号码,当Fee_UserType为3时该值有效,当Fee_UserType为0、1、2时该值无意义。 
         + 1// 1 Unsigned Integer 被计费用户的号码类型,0:真实号码;1:伪码。 
         + 1// 1 Unsigned Integer GSM协议类型。详细是解释请参考GSM03.40中的9.2.3.9。 
         + 1 // 1 Unsigned Integer GSM协议类型。详细是解释请参考GSM03.40中的9.2.3.23,仅使用1位,右对齐。
         + 1 // 1 Unsigned Integer 信息格式: 
             //   0:ASCII串; 
             //   3:短信写卡操作; 
             //   4:二进制信息; 
             //   8:UCS2编码; 
             //   15:含GB汉字。。。。。。 
         + 6// 6 Octet String 信息内容来源(SP_Id)。
         + 2// 2 Octet String 资费类别: 
            //   01:对"计费用户号码"免费; 
            //   02:对"计费用户号码"按条计信息费; 
            //   03:对"计费用户号码"按包月收取信息费。 
         + 6// 6 Octet String 资费代码(以分为单位)。 
         + 17// 17 Octet String 存活有效期,格式遵循SMPP3.3协议。 
         + 17// 17 Octet String 定时发送时间,格式遵循SMPP3.3协议。 
         + 21// 21 Octet String 源号码。SP的服务代码或前缀为服务代码的长号码, 网关将该号码完整的填到SMPP协议Submit_SM消息相应的source_addr字段,该号码最终在用户手机上显示为短消息的主叫号码。 
         + 1  // 1 Unsigned Integer 接收信息的用户数量(小于100个用户)。 
              //+ 32*DestUsr_tl 
         + 1// 1 Unsigned Integer 接收短信的用户的号码类型,0:真实号码;1:伪码。 
         + 1// 1 Unsigned Integer 信息长度(Msg_Fmt值为0时:<160个字节;其它<=140个字节),取值大于或等于0。
            //+ Msg_length 
         + 20;//20 Octet String 点播业务使用的LinkID,非点播类业务的MT流程不使用该字段。

        private readonly byte[] _msg;
        /// <summary>
        /// 初始化类<see cref="SubmitPackage"/>。
        /// </summary>
        /// <param name="msgfmt">消息格式。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="destTerminalId">群发号码列表。</param>
        public SubmitPackage(MsgFormat msgfmt, string msg, params string[] destTerminalId)
        {
            switch (msgfmt)
            {
                case MsgFormat.UCS2:
                    _msg = Encoding.BigEndianUnicode.GetBytes(Msg);
                    break;
                case MsgFormat.BG2312: //gb2312 
                    _msg = Encoding.GetEncoding("gb2312").GetBytes(Msg);
                    break;
                case MsgFormat.ASCII: //ascii 
                case MsgFormat.Card: //短信写卡操作 
                case MsgFormat.Binary: //二进制信息 
                default:
                    _msg = Encoding.ASCII.GetBytes(Msg);
                    break;
            }

            Msg = msg;
            MsgFmt = msgfmt;
            MsgLength = (uint)_msg.Length;
            DestTerminalId = destTerminalId;
            DestTerminalIdLength = (byte)destTerminalId.Length;
            Size = (int)(FixedBodyLength + 32 * DestTerminalIdLength + MsgLength);
            Header = new PackageHeader((uint)Size, CMPPCommand.CMPP_SUBMIT, 0);
        }

        /// <summary>
        /// 将包转换为字节数组。
        /// </summary>
        /// <returns>字节数组。</returns>
        public byte[] ToBytes()
        {
            var bytes = new byte[PackageHeader.Size + Size];
            var i = 0;
            //header 
            var buffer = Header.ToBytes();
            Buffer.BlockCopy(buffer, 0, bytes, 0, buffer.Length);
            i += PackageHeader.Size;

            //Msg_Id //8 [12,19] 
            buffer = BitConverter.GetBytes(MsgId);
            Array.Reverse(buffer);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //10 //[24,33]

            //_Pk_total 
            i += 8;
            bytes[i++] = (byte)PkTotal; //[20,20] 
            bytes[i++] = (byte)PkNumber; //[21,21] 
            bytes[i++] = (byte)RegisteredDelivery; //[22,22] 
            bytes[i++] = (byte)MsgLevel; //[23,23]

            //Service_Id 
            buffer = Encoding.ASCII.GetBytes(ServiceId);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //10 //[24,33]

            //Fee_UserType 
            i += 10;
            bytes[i++] = (byte)FeeUserType; //[34,34]

            //Fee_terminal_Id 
            buffer = Encoding.ASCII.GetBytes(FeeTerminalId);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //32 //[35,66]

            //Fee_terminal_type 
            i += 32;
            bytes[i++] = (byte)FeeTerminalType; //[67,67] 
            bytes[i++] = (byte)Pid; //[68,68] 
            bytes[i++] = (byte)Udhi; //[69,69] 
            bytes[i++] = (byte)MsgFmt; //[70,70]

            //Msg_src 
            buffer = Encoding.ASCII.GetBytes(MsgSrc);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //6 //[71,76]

            //FeeType 
            i += 6;
            buffer = Encoding.ASCII.GetBytes(FeeType);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //2 //[77,78]

            //FeeCode 
            i += 2;
            buffer = Encoding.ASCII.GetBytes(FeeCode);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //6 //[79,84]

            //ValId_Time 
            i += 6;
            buffer = Encoding.ASCII.GetBytes(ValIdTime); 
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //17 //[85,101]

            //At_Time 
            i += 17;
            buffer = Encoding.ASCII.GetBytes(AtTime); 
            Buffer.BlockCopy(buffer , 0, bytes, i, buffer.Length); //17 //[102,118]

            //Src_Id 
            i += 17;
            buffer = Encoding.ASCII.GetBytes(SrcId);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //21 //[119,139]

            //DestUsr_tl 
            i += 21;
            bytes[i++] = DestTerminalIdLength; //[140,140]

            //Dest_terminal_Id 
            foreach (string s in DestTerminalId)
            {
                buffer = Encoding.ASCII.GetBytes(s);
                Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length);
                i += 32;
            }

            //Dest_terminal_type 
            bytes[i++] = (byte)DestTerminalType;
            //Msg_Length 
            bytes[i++] = (byte)MsgLength;

            //Msg_Content 短信内容
            Buffer.BlockCopy(_msg, 0, bytes, i, _msg.Length);

            //LinkID 
            i += (int)MsgLength;
            buffer = Encoding.ASCII.GetBytes(LinkId);
            Buffer.BlockCopy(buffer, 0, bytes, i, buffer.Length); //20 
            return bytes;
        }

        /// <summary>
        /// 消息Id。
        /// </summary>
        public ulong MsgId { get; set; }

        /// <summary>
        /// 1 Unsigned Integer 相同MsgId的信息总条数,从1开始。
        /// </summary>
        public uint PkTotal { get; set; }

        /// <summary>
        /// 1 Unsigned Integer 相同MsgId的信息序号,从1开始。
        /// </summary>
        public uint PkNumber { get; set; }

        /// <summary>
        /// 1 Unsigned Integer 是否要求返回状态确认报告: 
        ///   0:不需要; 
        ///   1:需要。
        /// </summary>
        public RegisteredDelivery RegisteredDelivery { get; set; }

        /// <summary>
        /// // 1 Unsigned Integer 信息级别。 
        /// </summary>
        public uint MsgLevel { get; set; }

        /// <summary>
        /// 10 Octet String 业务标识,是数字、字母和符号的组合。
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 1 Unsigned Integer 计费用户类型字段: 
        ///   0:对目的终端MSISDN计费; 
        ///   1:对源终端MSISDN计费; 
        ///   2:对SP计费; 
        ///   3:表示本字段无效,对谁计费参见FeeTerminalId字段。
        /// </summary>
        public FeeUserType FeeUserType { get; set; }

        /// <summary>
        /// 32 Octet String 被计费用户的号码,当FeeUserType为3时该值有效,当FeeUserType为0、1、2时该值无意义。
        /// </summary>
        public string FeeTerminalId { get; set; }

        /// <summary>
        /// 1 Unsigned Integer 被计费用户的号码类型,0:真实号码;1:伪码。
        /// </summary>
        public TerminalType FeeTerminalType { get; set; }

        /// <summary>
        /// 1 Unsigned Integer GSM协议类型。详细是解释请参考GSM03.40中的9.2.3.9。
        /// </summary>
        [DisplayName("TP_pId")]
        public uint Pid { get; set; }

        /// <summary>
        /// 1 Unsigned Integer GSM协议类型。详细是解释请参考GSM03.40中的9.2.3.23,仅使用1位,右对齐。
        /// </summary>
        [DisplayName("TP_udhi")]
        public uint Udhi { get; set; }

        /// <summary>
        /// 1 Unsigned Integer 信息格式: 
        ///   0:ASCII串; 
        ///   3:短信写卡操作; 
        ///   4:二进制信息; 
        ///   8:UCS2编码; 
        ///   15:含GB汉字。。。。。。 
        /// </summary>
        public MsgFormat MsgFmt { get; }

        /// <summary>
        /// 6 Octet String 信息内容来源(SP_Id)。
        /// </summary>
        public string MsgSrc { get; set; }

        /// <summary>
        /// 2 Octet String 资费类别: 
        ///   01:对"计费用户号码"免费; 
        ///   02:对"计费用户号码"按条计信息费; 
        ///   03:对"计费用户号码"按包月收取信息费。 
        /// </summary>
        public string FeeType { get; set; }

        /// <summary>
        /// 6 Octet String 资费代码(以分为单位)。 
        /// </summary>
        public string FeeCode { get; set; }

        /// <summary>
        /// 17 Octet String 存活有效期,格式遵循SMPP3.3协议。
        /// </summary>
        [DisplayName("ValId_Time")]
        public string ValIdTime { get; set; }

        /// <summary>
        /// 17 Octet String 定时发送时间,格式遵循SMPP3.3协议。 
        /// </summary>
        public string AtTime { get; set; }

        /// <summary>
        /// 21 Octet String 源号码。SP的服务代码或前缀为服务代码的长号码, 网关将该号码完整的填到SMPP协议Submit_SM消息相应的source_addr字段,该号码最终在用户手机上显示为短消息的主叫号码。 
        /// </summary>
        public string SrcId { get; set; }

        /// <summary>
        /// 1 Unsigned Integer 接收信息的用户数量(小于100个用户)。 
        /// </summary>
        [DisplayName("DestUsr_tl")]
        public byte DestTerminalIdLength { get; }

        /// <summary>
        /// 32*DestUsr_tl Octet String 接收短信的MSISDN号码。
        /// </summary>
        public string[] DestTerminalId { get; }

        /// <summary>
        /// 1 Unsigned Integer 接收短信的用户的号码类型,0:真实号码;1:伪码。 
        /// </summary>
        public TerminalType DestTerminalType { get; set; }

        /// <summary>
        /// 1 Unsigned Integer 信息长度(MsgFmt值为0时:<160个字节;其它<=140个字节),取值大于或等于0。
        /// </summary>
        public uint MsgLength { get; }

        /// <summary>
        /// Octet String 信息内容。
        /// </summary>
        public string Msg { get; }

        /// <summary>
        /// 20 Octet String 点播业务使用的LinkID,非点播类业务的MT流程不使用该字段。
        /// </summary>
        public string LinkId { get; set; }

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; }
    }
}

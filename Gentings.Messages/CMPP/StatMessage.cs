using System;
using System.Text;

namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// 状态报告。
    /// </summary>
    public class StatMessage  
    {
        /// <summary>
        /// 包大小。
        /// </summary>
        public const int Size = 8 + 7 + 10 + 10 + 32 + 4;

        /// <summary>
        /// 解析提交包实例。
        /// </summary>
        /// <param name="bytes">提交返回包字节数组。</param>
        public StatMessage(byte[] bytes)
        {
            if (bytes.Length == Size)
            {
                int i = 0;
                //_Msg_Id 8 
                byte[] buffer = new byte[8];
                Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
                Array.Reverse(buffer);
                MsgId = BitConverter.ToUInt32(buffer, 0);

                //_Stat 7 
                i += 8;
                buffer = new byte[7];
                Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
                Stat = Encoding.ASCII.GetString(buffer);

                //_Submit_time 10 
                i += 7;
                buffer = new byte[10];
                Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
                SubmitTime = Encoding.ASCII.GetString(buffer);

                //_Done_time 10 
                i += 10;
                buffer = new byte[10];
                Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
                SubmitTime = Encoding.ASCII.GetString(buffer);

                //Dest_terminal_Id 32 
                i += 10;
                buffer = new byte[32];
                Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
                DestTerminalId = Encoding.ASCII.GetString(buffer);

                //SMSC_sequence 4 
                i += 32;
                buffer = new byte[4];
                Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
                Array.Reverse(buffer);
                SMSCSequence = BitConverter.ToUInt32(buffer, 0);
            }
        }

        /// <summary>
        /// 8 Unsigned Integer 信息标识。SP提交短信(CMPP_SUBMIT)操作时,与SP相连的ISMG产生的MsgId。 
        /// </summary>
        public uint MsgId { get; set; }

        /// <summary>
        /// 7 Octet String 发送短信的应答结果,含义详见表一。SP根据该字段确定CMPP_SUBMIT消息的处理状态。 
        /// </summary>
        public string Stat { get; set; }

        /// <summary>
        /// 10 Octet String YYMMDDHHMM(YY为年的后两位00-99,MM:01-12,DD:01-31,HH:00-23,MM:00-59)。
        /// </summary>
        public string SubmitTime { get; set; }

        /// <summary>
        /// 10 Octet String YYMMDDHHMM。
        /// </summary>
        public string DoneTime { get; set; }

        /// <summary>
        /// 32 Octet String 目的终端MSISDN号码(SP发送CMPP_SUBMIT消息的目标终端)。
        /// </summary>
        public string DestTerminalId { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 取自SMSC发送状态报告的消息体中的消息标识。 
        /// </summary>
        public uint SMSCSequence { get; set; }
    }

}

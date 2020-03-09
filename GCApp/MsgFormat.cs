namespace GCApp
{
    /// <summary>
    /// 消息内容格式。
    /// </summary>
    public enum MsgFormat : byte
    {
        /// <summary>
        ///   0:ASCII串; 
        ///   </summary>
        ASCII = 0,
        ///   <summary>
        ///   3:短信写卡操作; 
        ///   </summary>
        Card = 3,
        ///   <summary>
        ///   4:二进制信息; 
        ///   </summary>
        Binary = 4,
        ///   <summary>
        ///   8:UCS2编码; 
        ///   </summary>
        UCS2 = 8,
        ///   <summary>
        ///   15:含GB汉字。。。。。。 
        ///   </summary>
        BG2312 = 15,
    }
}

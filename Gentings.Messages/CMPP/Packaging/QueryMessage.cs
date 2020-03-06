namespace Gentings.Messages.CMPP.Packaging
{
    /// <summary>
    /// 查询返回的消息。
    /// </summary>
    public class QueryMessage
    {
        /// <summary>
        /// 8 Octet String 时间(精确至日)。
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 1 Unsigned Integer查询类别
        /// </summary>
        public QueryType QueryType { get; set; }

        /// <summary>
        /// 10 Octet String 查询码。
        /// </summary>
        public string QueryCode { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 从SP接收信息总数。
        /// </summary>
        public uint MT_TLMsg { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 从SP接收用户总数。
        /// </summary>
        public uint MT_Tlusr { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 成功转发数量。
        /// </summary>
        public uint MT_Scs { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 待转发数量。
        /// </summary>
        public uint MT_WT { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 转发失败数量。
        /// </summary>
        public uint MT_FL { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 向SP成功送达数量。
        /// </summary>
        public uint MO_Scs { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 向SP待送达数量。
        /// </summary>
        public uint MO_WT { get; set; }

        /// <summary>
        /// 4 Unsigned Integer 向SP送达失败数量。
        /// </summary>
        public uint MO_FL { get; set; }
    }
}

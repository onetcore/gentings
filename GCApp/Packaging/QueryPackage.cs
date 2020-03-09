using GCApp;
using System.IO;

namespace GCApp.Packaging
{
    /// <summary>
    /// 查询包。
    /// </summary>
    public class QueryPackage : IPackage
    {
        /// <summary>
        /// 初始化类<see cref="QueryPackage"/>。
        /// </summary>
        public QueryPackage()
        {
            Header = new PackageHeader(Size, CMPPCommand.CMPP_QUERY, 1);
        }

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; }

        /// <summary>
        /// 包体大小。
        /// </summary>
        public const int Size = 8 + 1 + 10 + 8;

        /// <summary>
        /// 将包转换为字节数组。
        /// </summary>
        /// <returns>字节数组。</returns>
        public byte[] ToBytes()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.WriteString(Time, 8);
                bw.WriteByte(QueryType);
                bw.WriteString(QueryCode, 10);
                bw.WriteString(Reserve, 8);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 8 时间YYYYMMDD(精确至日)。
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 1 查询类别：
        /// </summary>
        public QueryType QueryType { get; set; }

        /// <summary>
        /// 10 查询码。 当Query_Type为0时，此项无效；当Query_Type为1时，此项填写业务类型Service_Id。
        /// </summary>
        public string QueryCode { get; set; }

        /// <summary>
        /// 8 保留字段。
        /// </summary>
        public string Reserve { get; set; }
    }
}

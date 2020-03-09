using Gentings;

namespace GCApp.ServiceProviders
{
    /// <summary>
    /// CMPP服务提供者。
    /// </summary>
    /// <![CDATA[
    /// ISMG_Id:网关代码：0XYZ01~0XYZ99，其中XYZ为省会区号，位数不足时左补零，如北京编号为1的网关代码为001001，江西编号为1的网关代码为079101，依此类推;
    /// SP_Id:SP的企业代码：网络中SP地址和身份的标识、地址翻译、计费、结算等均以企业代码为依据。企业代码以数字表示，共6位，从“9XY000”至“9XY999”，其中“XY”为各移动公司代码;
    /// SP_Code:SPSP的服务代码：服务代码是在使用短信方式的上行类业务中，提供给用户使用的服务提供商代码。服务代码以数字表示，业务服务代码长度为4位，即“1000”－“9999”；本地业务服务代码长度统一为5位，即“01000”－“09999”；信产部对新的SP的服务代码分配提出了新的要求，要求以“1061”－“1069”作为前缀，目前中国移动进行了如下分配：
    ///         1062：用于省内SP服务代码
    ///         1066：用于SP服务代码
    ///         其它号段保留。
    /// Service_Id:SP的业务类型，数字、字母和符号的组合，由SP自定，如图片传情可定为TPCQ，股票查询可定义为11
    /// ]]>
    public interface ICMPPServiceProvider : ISingletonServices
    {
        /// <summary>
        /// 提供者名称，唯一标识。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 服务提供商的IP地址。
        /// </summary>
        string Host { get; }

        /// <summary>
        /// 端口。
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 服务提供商Id。
        /// </summary>
        string Spid { get; }

        /// <summary>
        /// SP的服务代码（接入码）：服务代码是在使用短信方式的上行类业务中，提供给用户使用的服务提供商代码。服务代码以数字表示，全国业务服务代码长度为4位，即“1000”－“9999”；本地业务服务代码长度统一为5位，即“01000”－“09999”；信产部对新的SP的服务代码分配提出了新的要求，要求以“1061”－“1069”作为前缀，目前中国移动进行了如下分配：
        /// 1062：用于省内SP服务代码
        /// 1066：用于全国SP服务代码
        /// 其它号段保留。
        /// </summary>
        string SpCode { get; }

        /// <summary>
        /// 密钥。
        /// </summary>
        string Password { get; }

        /// <summary>
        /// 是否禁用。
        /// </summary>
        bool Disabled => false;

        /// <summary>
        /// 连接数。
        /// </summary>
        int Clients => 1;

        /// <summary>
        /// 每次处理短信数量。
        /// </summary>
        int Size => 100;
    }
}

using Gentings.Messages.CMPP.Packages;
using System.Threading.Tasks;

namespace Gentings.Messages.CMPP.Services
{
    /// <summary>
    /// 服务处理接口。
    /// </summary>
    public interface IServiceHandler : IScopedServices
    {
        /// <summary>
        /// 返回的命令。
        /// </summary>
        CMPPCommand Command { get; }

        /// <summary>
        /// 执行当前命令的方法。
        /// </summary>
        /// <param name="header">标题头。</param>
        /// <param name="bytes">返回的字节数据。</param>
        /// <returns>返回当前执行的任务。</returns>
        Task ExecuteAsync(PackageHeader header, byte[] bytes);
    }
}

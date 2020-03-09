using Gentings;
using Gentings.ConsoleApp;
using System.Threading.Tasks;

namespace GCApp.Services
{
    /// <summary>
    /// 服务处理接口。
    /// </summary>
    public interface IServiceHandler : IServices
    {
        /// <summary>
        /// 返回的命令。
        /// </summary>
        CMPPCommand Command { get; }

        /// <summary>
        /// 执行当前命令的方法。
        /// </summary>
        /// <param name="context">服务上下文。</param>
        /// <param name="header">标题头。</param>
        /// <param name="bytes">返回的字节数据。</param>
        /// <returns>返回当前执行的任务。</returns>
        Task ExecuteAsync(ServiceContext context, PackageHeader header, byte[] bytes);
    }

    /// <summary>
    /// 连接处理类。
    /// </summary>
    public class ConnectHandler : IServiceHandler
    {
        /// <summary>
        /// 返回的命令。
        /// </summary>
        public CMPPCommand Command => CMPPCommand.CMPP_CONNECT;

        /// <summary>
        /// 执行当前命令的方法。
        /// </summary>
        /// <param name="context">服务上下文。</param>
        /// <param name="header">标题头。</param>
        /// <param name="bytes">返回的字节数据。</param>
        /// <returns>返回当前执行的任务。</returns>
        public async Task ExecuteAsync(ServiceContext context, PackageHeader header, byte[] bytes)
        {
            Consoles.Warning("client:接收到连接数据！");
        }
    }
}

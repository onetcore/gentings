using System.Threading.Tasks;

namespace Gentings.Commands
{
    /// <summary>
    /// 命令处理器工厂。
    /// </summary>
    public interface ICommandHandlerFactory : ISingletonService
    {
        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="commandName">命令名称。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回执行任务。</returns>
        Task ExecuteAsync(string commandName, string args);
    }
}
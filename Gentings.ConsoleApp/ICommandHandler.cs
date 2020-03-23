using System.Threading.Tasks;

namespace Gentings.ConsoleApp
{
    /// <summary>
    /// 命令处理器，命令以“.”开头。
    /// </summary>
    public interface ICommandHandler : IServices
    {
        /// <summary>
        /// 命令。
        /// </summary>
        string Command { get; }

        /// <summary>
        /// 帮助。
        /// </summary>
        string Help { get; }

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="argument">参数。</param>
        /// <returns>返回执行任务。</returns>
        Task ExecuteAsync(CommandArgs argument);
    }

    /// <summary>
    /// 显示调试信息命令。
    /// </summary>
    public class DebugCommandHandler : ICommandHandler
    {
        /// <summary>
        /// 命令。
        /// </summary>
        public string Command => "debug";

        /// <summary>
        /// 帮助。
        /// </summary>
        public string Help => "是否显示调试信息，如果关闭使用命令.debug off";

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="argument">参数。</param>
        /// <returns>返回执行任务。</returns>
        public Task ExecuteAsync(CommandArgs argument)
        {
            Consoles.IsDebug = !argument.IsSubCommand("off");
            if (Consoles.IsDebug)
            {
                Consoles.Info("开启调试信息显示！");
            }
            else
            {
                Consoles.Warning("关闭调试信息显示！");
            }

            return Task.CompletedTask;
        }
    }
}
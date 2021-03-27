using System.Threading.Tasks;
using Gentings.Properties;

namespace Gentings.Commands
{
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
        public string Help => Resources.DebugCommandHandler_Help_DebugOff;

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="argument">参数。</param>
        /// <returns>返回执行任务。</returns>
        public Task ExecuteAsync(CommandArgs argument)
        {
            CommandConsole.IsDebug = !argument.IsSubCommand("off");
            if (CommandConsole.IsDebug)
            {
                CommandConsole.Info(Resources.DebugCommandHandler_ExecuteAsync_DebugOn);
            }
            else
            {
                CommandConsole.Warning(Resources.DebugCommandHandler_ExecuteAsync_DebugOff);
            }

            return Task.CompletedTask;
        }
    }
}
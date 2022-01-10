namespace Gentings.Commands
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
}
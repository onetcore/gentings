using System.Collections.Concurrent;
using Gentings.Properties;

namespace Gentings.Commands
{
    /// <summary>
    /// 命令处理器工厂实现类。
    /// </summary>
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly ConcurrentDictionary<string, ICommandHandler> _commandHandlers;

        /// <summary>
        /// 初始化类<see cref="CommandHandlerFactory"/>。
        /// </summary>
        /// <param name="commandHandlers">命令处理器列表。</param>
        public CommandHandlerFactory(IEnumerable<ICommandHandler> commandHandlers)
        {
            _commandHandlers =
                new ConcurrentDictionary<string, ICommandHandler>(commandHandlers.ToDictionary(x => x.Command),
                    StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="commandName">命令名称。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回执行任务。</returns>
        public async Task ExecuteAsync(string commandName, string args)
        {
            switch (commandName)
            {
                case "exit":
                case "quit":
                    {
                        await CommandConsole.CloseAsync(10);
                        CommandConsole.TokenSource.Cancel();
                    }
                    break;
                case "help":
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(Resources.CommandHandlerFactory_CommandList);
                        foreach (var commandHandler in _commandHandlers.Values.OrderBy(x => x.Command))
                        {
                            CommandConsole.Display(commandHandler.Command, commandHandler.Help);
                        }

                        CommandConsole.Display("help", Resources.CommandHandlerFactory_ExecuteAsync_HelpDescription);
                        CommandConsole.Display("exit|quit", Resources.CommandHandlerFactory_ExecuteAsync_Quit);
                        Console.ResetColor();
                    }
                    break;
                default:
                    if (_commandHandlers.TryGetValue(commandName, out var handler))
                    {
                        try
                        {
                            await handler.ExecuteAsync(new CommandArgs(args));
                        }
                        catch (Exception exception)
                        {
                            CommandConsole.Error(exception.Message);
                        }
                    }
                    else
                    {
                        CommandConsole.Error(Resources.CommandHandlerFactory_ExecuteAsync_NotSupported, commandName);
                    }

                    break;
            }
        }
    }
}
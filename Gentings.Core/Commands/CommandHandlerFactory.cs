using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                        await Consoles.CloseAsync(10);
                        Consoles.TokenSource.Cancel();
                    }
                    break;
                case "help":
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(Resources.CommandHandlerFactory_CommandList);
                        foreach (var commandHandler in _commandHandlers.Values.OrderBy(x => x.Command))
                        {
                            Consoles.Display(commandHandler.Command, commandHandler.Help);
                        }

                        Consoles.Display("help", Resources.CommandHandlerFactory_ExecuteAsync_HelpDescription);
                        Consoles.Display("exit|quit", Resources.CommandHandlerFactory_ExecuteAsync_Quit);
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
                            Consoles.Error(exception.Message);
                        }
                    }
                    else
                    {
                        Consoles.Error(Resources.CommandHandlerFactory_ExecuteAsync_NotSupported, commandName);
                    }

                    break;
            }
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gentings.ConsoleApp
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
            _commandHandlers = new ConcurrentDictionary<string, ICommandHandler>(commandHandlers.ToDictionary(x => x.Command), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="commandName">命令名称。</param>
        /// <param name="argument">参数。</param>
        /// <returns>返回执行任务。</returns>
        public async Task ExecuteAsync(string commandName, string args)
        {
            switch (commandName)
            {
                case "exit":
                case "quit":
                    {
                        var delay = 10;
                        Consoles.Warning($"程序即将在“{delay}”秒后关闭...");
                        await DelayAsync(delay);
                        Consoles.TokenSource.Cancel();
                    }
                    break;
                case "help":
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("本程序可以使用的命令包含以下内容：");
                        foreach (var commandHandler in _commandHandlers.Values.OrderBy(x => x.Command))
                        {
                            Consoles.Display(commandHandler.Command, commandHandler.Help);
                        }
                        Consoles.Display("help", "显示帮助信息");
                        Consoles.Display("exit|quit", "推出应用程序");
                        Console.ResetColor();
                    }
                    break;
                default:
                    if (_commandHandlers.TryGetValue(commandName, out var handler))
                    {
                        try { await handler.ExecuteAsync(new CommandArgs(args)); }
                        catch (Exception exception)
                        { Consoles.Error(exception.Message); }
                    }
                    else
                        Consoles.Error($"不支持命令：.{commandName}，请使用.help命令查看可用命令");
                    break;
            }
        }

        private async Task DelayAsync(int delay)
        {
            if (delay < 1)
            {
                Consoles.Warning("关闭！");
                return;
            }
            Consoles.Warning($"{delay}");
            await Task.Delay(1000);
            await DelayAsync(--delay);
        }
    }
}

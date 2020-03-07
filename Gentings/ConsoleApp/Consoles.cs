﻿using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

namespace Gentings.ConsoleApp
{
    /// <summary>
    /// 控制台方法。
    /// </summary>
    public static class Consoles
    {
        /// <summary>
        /// 控制台使用全局取消标识。
        /// </summary>
        public static CancellationTokenSource TokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken());
        /// <summary>
        /// 是否显示调试信息。
        /// </summary>
        public static bool IsDebug { get; set; }

        /// <summary>
        /// 启动应用程序，在控制台程序中的Main方法中调用。
        /// </summary>
        /// <param name="args">参数。</param>
        public static void Start(string[] args)
        {
            Info("正在初始化应用程序！");
            Console.WriteLine();
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, service) => service.AddGentings(context.Configuration))
                .Build();
            host.StartAsync(TokenSource.Token);
            Console.WriteLine();
            Info("已经成功启动了应用程序，可以输入命令进行手动操作！");
            var commandHandlerFactory = host.Services.GetService(typeof(ICommandHandlerFactory)) as ICommandHandlerFactory;
            while (!TokenSource.IsCancellationRequested)
            {
                string command = Console.ReadLine()?.Trim();
                if (command?.Length > 0)
                {
                    if (command.StartsWith('.'))
                    {
                        command = command.TrimStart('.');
                        var commandName = command;
                        int index = command.IndexOf(' ');
                        if (index != -1)
                        {
                            commandName = command.Substring(0, index);
                            command = command.Substring(index).Trim();
                        }
                        else
                            command = null;
                        commandHandlerFactory.ExecuteAsync(commandName, command).Wait();
                    }
                    else
                    {
                        Error($"不支持命令“{command}”，请使用.help命令来获取帮助信息！");
                    }
                }
                Thread.Sleep(10);
            }
        }

        #region commands output
        /// <summary>
        /// 显示命令。
        /// </summary>
        /// <param name="command">命令。</param>
        /// <param name="help">帮助信息。</param>
        internal static void Display(string command, string help)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("     .{0}: ", command.ToLower());
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(help);
            Console.ResetColor();
        }

        /// <summary>
        /// 显示Debug信息。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="args">参数。</param>
        public static void Debug(string message, params object[] args)
        {
            if (IsDebug)
            {
                Console.WriteLine(message, args);
            }
        }

        /// <summary>
        /// 显示错误信息。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="args">参数。</param>
        public static void Error(string message, params object[] args)
        {
            WriteLine(ConsoleColor.Red, message, args);
        }

        /// <summary>
        /// 显示信息。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="args">参数。</param>
        public static void Info(string message, params object[] args)
        {
            WriteLine(ConsoleColor.Cyan, message, args);
        }

        /// <summary>
        /// 显示成功信息。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="args">参数。</param>
        public static void Success(string message, params object[] args)
        {
            WriteLine(ConsoleColor.Green, message, args);
        }

        /// <summary>
        /// 显示警告信息。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="args">参数。</param>
        public static void Warning(string message, params object[] args)
        {
            WriteLine(ConsoleColor.Yellow, message, args);
        }

        /// <summary>
        /// 输出控制台日志。
        /// </summary>
        /// <param name="color">字体颜色。</param>
        /// <param name="message">消息。</param>
        /// <param name="args">参数。</param>
        public static void WriteLine(ConsoleColor color, string message, params object[] args)
        {
            Write(color, message, args);
            Console.WriteLine();
        }

        /// <summary>
        /// 输出控制台日志。
        /// </summary>
        /// <param name="color">字体颜色。</param>
        /// <param name="message">消息。</param>
        /// <param name="args">参数。</param>
        public static void Write(ConsoleColor color, string message, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.Write(message, args);
            Console.ResetColor();
        }

        /// <summary>
        /// 输出控制台日志。
        /// </summary>
        /// <param name="color">字体颜色。</param>
        /// <param name="bgColor">背景颜色。</param>
        /// <param name="message">消息。</param>
        /// <param name="args">参数。</param>
        public static void WriteLine(ConsoleColor color, ConsoleColor bgColor, string message, params object[] args)
        {
            Write(color, bgColor, message, args);
            Console.WriteLine();
        }

        /// <summary>
        /// 输出控制台日志。
        /// </summary>
        /// <param name="color">字体颜色。</param>
        /// <param name="bgColor">背景颜色。</param>
        /// <param name="message">消息。</param>
        /// <param name="args">参数。</param>
        public static void Write(ConsoleColor color, ConsoleColor bgColor, string message, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = bgColor;
            Console.Write(message, args);
            Console.ResetColor();
        }
        #endregion
    }
}

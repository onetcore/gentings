using System.Text;
using Gentings.Properties;

namespace Gentings.Commands
{
    /// <summary>
    /// 命令参数。
    /// </summary>
    public class CommandArgs
    {
        private readonly IDictionary<string, string> _arguments = new Dictionary<string, string>();
        private readonly string _args;

        /// <summary>
        /// 初始化类<see cref="CommandArgs"/>。
        /// </summary>
        /// <param name="args">参数字符串。</param>
        public CommandArgs(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                IsEmpty = true;
                return;
            }

            _args = args;
            var index = 0;
            while (index < args.Length)
            {
                var current = args[index];
                if (current != '-')
                {
                    SubComamnds.Add(Read(ref index, ' ').ToLower());
                    Skip(ref index, ' ');
                    continue;
                }

                index++;
                var name = Read(ref index, ' ');
                Skip(ref index, ' ');
                current = args[index];
                if (current == '"' || current == '\'')
                {
                    index++;
                    _arguments[name] = Read(ref index, current).Trim();
                }
                else
                {
                    _arguments[name] = Read(ref index, '-').Trim();
                    continue;
                }

                index++;
            }
        }

        private void Skip(ref int index, char end)
        {
            while (index < _args.Length)
            {
                if (_args[index] == end)
                {
                    index++;
                    continue;
                }

                break;
            }
        }

        private string Read(ref int index, char end)
        {
            var builder = new StringBuilder();
            while (index < _args.Length)
            {
                var current = _args[index];
                if ((end == '"' || end == '\'') && current == '\\')
                {
                    //转义符
                    index++;
                    if (index < _args.Length)
                    {
                        current = _args[index];
                    }
                    else
                    {
                        throw new Exception(string.Format(Resources.CommandArgs_Read_MustEndsWith, end));
                    }
                }

                if (current == end)
                {
                    return builder.ToString();
                }

                builder.Append(current);
                index++;
            }

            return builder.ToString();
        }

        /// <summary>
        /// 判断是否包含参数，大小写敏感。
        /// </summary>
        /// <param name="name">参数名称。</param>
        /// <returns>返回判断结果。</returns>
        public bool Contains(string name) => _arguments.ContainsKey(name);

        /// <summary>
        /// 获取当前参数值，不包含“-”。
        /// </summary>
        /// <param name="name">名称，大小写敏感。</param>
        /// <returns>返回当前参数值。</returns>
        public string this[string name]
        {
            get
            {
                _arguments.TryGetValue(name, out var value);
                return value;
            }
        }

        /// <summary>
        /// 获取名称中的任何一个值，主要用于参数简写。
        /// </summary>
        /// <param name="names">名称集合，只要参数中包含一个就会返回当前值。</param>
        /// <returns>返回参数值，如果不存在返回<c>null</c>。</returns>
        public string GetArg(params string[] names)
        {
            foreach (var name in names)
            {
                if (_arguments.TryGetValue(name, out var value))
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// 子命令。
        /// </summary>
        public IList<string> SubComamnds { get; } = new List<string>();

        /// <summary>
        /// 判断是否有子命令。
        /// </summary>
        /// <param name="commandName">命令名称，忽略大小写。</param>
        /// <returns>返回判断结果。</returns>
        public bool IsSubCommand(string commandName) => SubComamnds.Contains(commandName.ToLower());

        /// <summary>
        /// 是否为空。
        /// </summary>
        public bool IsEmpty { get; }
    }
}
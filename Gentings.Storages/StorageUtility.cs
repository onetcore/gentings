﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gentings.Storages
{
    /// <summary>
    /// 存储辅助类型。
    /// </summary>
    public static class StorageUtility
    {
        /// <summary>
        /// 读取所有文件内容。
        /// </summary>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="encoding">编码。</param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回文件内容字符串。</returns>
        public static string ReadText(string path, Encoding encoding = null, FileShare share = FileShare.None)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, share);
            using var reader = new StreamReader(fs, encoding ?? Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 读取所有文件内容。
        /// </summary>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="encoding">编码。</param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回文件内容字符串。</returns>
        public static async Task<string> ReadTextAsync(string path, Encoding encoding = null, FileShare share = FileShare.None)
        {
            await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, share);
            using var reader = new StreamReader(fs, encoding ?? Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// 保存文件内容。
        /// </summary>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="text"></param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回写入任务实例对象。</returns>
        public static void SaveText(string path, string text, FileShare share = FileShare.None)
        {
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, share);
            using var sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(text);
        }

        /// <summary>
        /// 保存文件内容。
        /// </summary>
        /// <param name="path">文件的物理路径。</param>
        /// <param name="text"></param>
        /// <param name="share">文件共享选项。</param>
        /// <returns>返回写入任务实例对象。</returns>
        public static async Task SaveTextAsync(string path, string text, FileShare share = FileShare.None)
        {
            await using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, share);
            await using var sw = new StreamWriter(fs, Encoding.UTF8);
            await sw.WriteAsync(text);
        }

        /// <summary>
        /// 获取文件的编码格式。
        /// </summary>
        /// <param name="path">当前文件的物理路径。</param>
        /// <param name="defaultEncoding">默认编码，如果为<code>null</code>，则为<see cref="Encoding.Default"/>。</param>
        /// <returns>返回当前文件的编码。</returns>
        public static Encoding GetEncoding(string path, Encoding defaultEncoding = null)
        {
            defaultEncoding ??= Encoding.GetEncoding("GB2312");
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (fs.Length < 3)
            {
                return defaultEncoding;
            }

            var buffer = new byte[3];
            fs.Read(buffer, 0, 3);
            var unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            var unicodeBig = new byte[] { 0xFE, 0xFF, 0x00 };
            var utf8 = new byte[] { 0xEF, 0xBB, 0xBF };//带BOM

            if (buffer[0] == utf8[0] && buffer[1] == utf8[1] && buffer[2] == utf8[2] || IsUTF8(fs))
            {
                return Encoding.UTF8;
            }

            if (buffer[0] == unicodeBig[0] && buffer[1] == unicodeBig[1] && buffer[2] == unicodeBig[2])
            {
                return Encoding.BigEndianUnicode;
            }

            if (buffer[0] == unicode[0] && buffer[1] == unicode[1] && buffer[2] == unicode[2])
            {
                return Encoding.Unicode;
            }

            return defaultEncoding;
        }

        /// <summary>
        /// 没有BOM。
        /// </summary>
        /// <param name="stream">文件流。</param>
        /// <returns>返回判断结果。</returns>
        private static bool IsUTF8(Stream stream)
        {
            var counter = 1;//计算当前正分析的字符应还有的字节数
            stream.Position = 0;
            var current = stream.ReadByte();
            while (current != -1)
            {
                if (counter == 1)
                {
                    if (current >= 0x80)
                    {
                        //判断当前
                        while (((current <<= 1) & 0x80) != 0)
                        {
                            counter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始，如：110XXXXX.....1111110X
                        if (counter == 1 || counter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((current & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    counter--;
                }
                current = stream.ReadByte();//当前分析的字节
            }
            if (counter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        /// <summary>
        /// 转换文件编码。
        /// </summary>
        /// <param name="directoryName">当前文件夹物理路径。</param>
        /// <param name="searchPattern">文件匹配模式。</param>
        /// <param name="option">检索选项。</param>
        /// <param name="defaultEncoding">默认编码，如果为<code>null</code>，则为<see cref="Encoding.Default"/>。</param>
        /// <param name="destinationEncoding">转换的编码，如果为<code>null</code>，则为<see cref="Encoding.UTF8"/>。</param>
        /// <returns>返回转换任务。</returns>
        public static async Task ConvertAsync(string directoryName, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, Encoding defaultEncoding = null, Encoding destinationEncoding = null)
        {
            var directory = new DirectoryInfo(directoryName);
            if (!directory.Exists)
            {
                return;
            }

            destinationEncoding ??= Encoding.UTF8;
            foreach (var info in directory.GetFiles(searchPattern, option))
            {
                var current = GetEncoding(info.FullName, defaultEncoding);
                if (current == destinationEncoding)
                {
                    continue;
                }

                var content = File.ReadAllText(info.FullName, current);
                await using var fs = new FileStream(info.FullName, FileMode.Create, FileAccess.Write);
                await using var writer = new StreamWriter(fs, destinationEncoding);
                await writer.WriteAsync(content);
            }
        }
    }
}
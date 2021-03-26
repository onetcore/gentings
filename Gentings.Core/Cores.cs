﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gentings.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings
{
    /// <summary>
    /// 核心辅助类。
    /// </summary>
    public static class Cores
    {
        /// <summary>
        /// 生成固定字节的随机数的十六进制字符串。
        /// </summary>
        /// <param name="size">字节数。</param>
        /// <returns>返回固定字节的随机数的十六进制字符串。</returns>
        public static string GeneralKey(int size)
        {
            return RandomOAuthStateGenerator.Generate(size);
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();

            public static string Generate(int size)
            {
                const int bitsPerByte = 8;

                if (size % bitsPerByte != 0)
                {
                    throw new ArgumentException(Resources.RandomNumberGenerator_SizeInvalid, nameof(size));
                }

                var strengthInBytes = size / bitsPerByte;

                var data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return data.ToHexString();
            }
        }

        /// <summary>
        /// 将数组转换为十六进制字符串。
        /// </summary>
        /// <param name="bytes">当前数组。</param>
        /// <returns>返回十六进制字符串。</returns>
        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        /// <summary>
        /// 将十六进制字符串转换为二进制数组。
        /// </summary>
        /// <param name="hexString">十六进制字符串。</param>
        /// <returns>返回二进制数组。</returns>
        public static byte[] GetBytes(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var hex = hexString.Substring(i * 2, 2);
                bytes[i] = (byte)int.Parse(hex, NumberStyles.HexNumber);
            }

            return bytes;
        }

        /// <summary>
        /// 使用HMACMD5进行加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <param name="key">哈希算法密钥128位密钥，十六进制字符串。</param>
        /// <returns>返回加密后的16进制的字符串。</returns>
        // ReSharper disable once InconsistentNaming
        public static string HMACMD5(string text, string key)
        {
            var md5 = new HMACMD5(GetBytes(key));
            var hashed = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// 使用HMACSHA1进行加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <param name="key">哈希算法密钥128位密钥，十六进制字符串。</param>
        /// <returns>返回加密后的16进制的字符串。</returns>
        // ReSharper disable once InconsistentNaming
        public static string HMACSHA1(string text, string key)
        {
            var sha = new HMACSHA1(GetBytes(key));
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// 使用HMACSHA256进行加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <param name="key">哈希算法密钥128位密钥，十六进制字符串。</param>
        /// <returns>返回加密后的16进制的字符串。</returns>
        // ReSharper disable once InconsistentNaming
        public static string HMACSHA256(string text, string key)
        {
            var sha = new HMACSHA256(GetBytes(key));
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// 使用HMACSHA384进行加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <param name="key">哈希算法密钥128位密钥，十六进制字符串。</param>
        /// <returns>返回加密后的16进制的字符串。</returns>
        // ReSharper disable once InconsistentNaming
        public static string HMACSHA384(string text, string key)
        {
            var sha = new HMACSHA384(GetBytes(key));
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// 使用HMACSHA512进行加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <param name="key">哈希算法密钥128位密钥，十六进制字符串。</param>
        /// <returns>返回加密后的16进制的字符串。</returns>
        // ReSharper disable once InconsistentNaming
        public static string HMACSHA512(string text, string key)
        {
            var sha = new HMACSHA512(GetBytes(key));
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// MD5加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string Md5(string text)
        {
            var md5 = MD5.Create();
            var hashed = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// SHA1加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string Sha1(string text)
        {
            var sha1 = SHA1.Create();
            var hashed = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// SHA256加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string Sha256(string text)
        {
            var sha = SHA256.Create();
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// SHA384加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string Sha384(string text)
        {
            var sha = SHA384.Create();
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// SHA512加密。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string Sha512(string text)
        {
            var sha = SHA512.Create();
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hashed.ToHexString();
        }

        /// <summary>
        /// 获取页面区间。
        /// </summary>
        /// <param name="pageIndex">页码。</param>
        /// <param name="pages">总页数。</param>
        /// <param name="factor">显示项数。</param>
        /// <param name="end">返回结束索引。</param>
        /// <returns>返回开始索引。</returns>
        public static int GetRange(int pageIndex, int pages, int factor, out int end)
        {
            var item = factor / 2;
            var start = pageIndex - item;
            end = pageIndex + item;
            if (start < 1)
            {
                end += 1 - start;
                start = 1;
            }

            if (end > pages)
            {
                start -= (end - pages);
                end = pages;
            }

            if (end < 1)
            {
                end = 1;
            }

            if (start < 1)
            {
                return 1;
            }

            return start;
        }

        private static readonly DateTime _unixDate = new DateTime(1970, 1, 1);

        /// <summary>
        /// 获取当前时间对应的UNIX时间的秒数。
        /// </summary>
        public static long UnixNow => DateTime.Now.ToUnix();

        /// <summary>
        /// 将UNIX时间的秒数转换为日期。
        /// </summary>
        /// <param name="seconds">秒数。</param>
        /// <returns>返回当前日期值。</returns>
        public static DateTime FromUnix(int seconds)
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            return _unixDate.AddSeconds(seconds);
        }

        /// <summary>
        /// 将UNIX时间的毫秒数转换为日期。
        /// </summary>
        /// <param name="milliseconds">毫秒数。</param>
        /// <returns>返回当前日期值。</returns>
        public static DateTime FromUnix(long milliseconds)
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            return _unixDate.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// 获取当前时间对应的UNIX时间的秒数。
        /// </summary>
        /// <param name="date">当前时间。</param>
        /// <returns>返回当前时间对应的UNIX时间的秒数。</returns>
        public static long ToUnix(this DateTime date) => (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;

        /// <summary>
        /// 生成随机的最多13位的唯一ID字符串。
        /// </summary>
        /// <returns>返回随机的13位字符串。</returns>
        public static string NewId()
        {
            long i = 1;
            foreach (var b in Guid.NewGuid().ToByteArray())
            {
                i *= b + 1;
            }

            return Math.Abs(i - DateTime.Now.Ticks).ToBase36();
        }

        private static readonly string _base36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 将长整型转换为36进制的字符串。
        /// </summary>
        /// <param name="value">当前值。</param>
        /// <returns>返回转换后的字符串。</returns>
        public static string ToBase36(this long value)
        {
            var current = "";
            current += _base36[(int)(value % 36)];
            value /= 36;
            if (value > 36)
            {
                return value.ToBase36() + current;
            }

            return _base36[(int)value] + current;
        }

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <returns>返回加密后的字符串。</returns>
        public static string Encrypto(string text)
        {
            var rijndael = Rijndael.Create();
            rijndael.GenerateIV();
            rijndael.GenerateKey();
            var buffer = Encoding.UTF8.GetBytes(text);
            using var ms = new MemoryStream();
            var encrypto = rijndael.CreateEncryptor();
            var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(buffer, 0, buffer.Length);
            cs.FlushFinalBlock();
            var result = ms.ToArray();
            buffer = new byte[48 + result.Length];
            rijndael.IV.CopyTo(buffer, 0);
            result.CopyTo(buffer, 16);
            rijndael.Key.CopyTo(buffer, 16 + result.Length);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <returns>返回解密后的字符串。</returns>
        public static string Decrypto(string text)
        {
            var buffer = Convert.FromBase64String(text);
            var rijndael = Rijndael.Create();
            var iv = new byte[16];
            var key = new byte[32];
            using (var ms = new MemoryStream(buffer))
            {
                ms.Read(iv, 0, 16);
                buffer = new byte[buffer.Length - 48];
                ms.Read(buffer, 0, buffer.Length);
                ms.Read(key, 0, 32);
            }

            rijndael.IV = iv;
            rijndael.Key = key;
            using (var ms = new MemoryStream(buffer))
            {
                var encrypto = rijndael.CreateDecryptor();
                var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs, Encoding.UTF8);
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 判断当前<paramref name="item"/>是否包含在<paramref name="items"/>中。
        /// </summary>
        /// <param name="item">当前项。</param>
        /// <param name="items">列表实例。</param>
        /// <returns>返回判断结果。</returns>
        public static bool Included(this object item, IEnumerable items)
        {
            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (Equals(enumerator.Current, item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 将列表格式化为字符串。
        /// </summary>
        /// <param name="items">当前实例列表。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回格式化后的字符串。</returns>
        public static string Join(this IEnumerable items, string separator = ",")
        {
            var list = new List<object>();
            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            return string.Join(separator, list);
        }

        /// <summary>
        /// 将字符串分割为数值列表。
        /// </summary>
        /// <param name="items">当前数值字符串。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回数值列表。</returns>
        public static int[] ToInt32Array(this string items, string separator = ",")
        {
            if (string.IsNullOrEmpty(items))
                return null;
            return items.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x.Trim()))
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// 默认缓存时长。
        /// </summary>
        public static readonly TimeSpan DefaultCacheExpiration = TimeSpan.FromMinutes(3);

        /// <summary>
        /// 获取<see cref="IServiceProvider"/>实例，此方法只能用于单元测试。
        /// </summary>
        /// <param name="configuration">配置接口。</param>
        /// <param name="action">实例化容器。</param>
        /// <returns>返回服务提供者接口实例。</returns>
        public static IServiceProvider BuildServiceProvider(IConfiguration configuration,
            Action<IServiceBuilder> action = null)
        {
            var services = new ServiceCollection();
            var builder = services.AddGentings(configuration);
            action?.Invoke(builder);
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// 当前程序的版本。
        /// </summary>
        public static Version Version => Assembly.GetEntryAssembly()?.GetName().Version;

        private static readonly JsonSerializerOptions _defaultJsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// 将对象格式化成JSON字符串。
        /// </summary>
        /// <param name="instance">对象实例。</param>
        /// <param name="options">JSON序列化配置。</param>
        /// <returns>返回格式化后的字符串。</returns>
        public static string ToJsonString(this object instance, JsonSerializerOptions options = null)
        {
            if (instance == null)
            {
                return null;
            }

            options ??= _defaultJsonSerializerOptions;
            return JsonSerializer.Serialize(instance, options);
        }

        /// <summary>
        /// 将JSON字符串反序列化对象。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="json">JSON字符串。</param>
        /// <param name="options">JSON序列化配置实例。</param>
        /// <returns>返回实例对象。</returns>
        public static TModel FromJsonString<TModel>(string json, JsonSerializerOptions options = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return default;
                options ??= _defaultJsonSerializerOptions;
                return JsonSerializer.Deserialize<TModel>(json, options);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 获取枚举描述信息。
        /// </summary>
        /// <param name="value">枚举值。</param>
        /// <returns>返回枚举的描述信息。</returns>
        public static string ToDescriptionString(this Enum value)
        {
            var name = value.ToString();
            var info = value.GetType().GetField(name);
            if (info == null)
                return name;
            return info.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name;
        }

        /// <summary>
        /// 截取数组中的区间并且返回数组实例。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="array">当前数组实例。</param>
        /// <param name="index">开始索引。</param>
        /// <param name="length">长度。</param>
        /// <returns>返回截取的数组列表。</returns>
        public static IEnumerable<TModel> Slice<TModel>(this IEnumerable<TModel> array, int index, int length)
        {
            length = Math.Min(index + length, array.Count());
            for (var i = index; i < length; i++)
            {
                yield return array.ElementAt(i);
            }
        }

        /// <summary>
        /// 设置默认缓存时间，3分钟。
        /// </summary>
        /// <param name="cache">缓存实体接口。</param>
        /// <returns>返回缓存实体接口。</returns>
        public static ICacheEntry SetDefaultAbsoluteExpiration(this ICacheEntry cache)
        {
            return cache.SetAbsoluteExpiration(Cores.DefaultCacheExpiration);
        }
    }
}
using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Gentings.RabbitMQ
{
    /// <summary>
    /// 配置读取实现类。
    /// </summary>
    public class RabbitMQConfiguration : IRabbitMQConfiguration
    {
        private readonly ConcurrentDictionary<string, ConnectionFactory> _configuration = new ConcurrentDictionary<string, ConnectionFactory>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 初始化类<see cref="RabbitMQConfiguration"/>。
        /// </summary>
        /// <param name="configuration">配置接口实例。</param>
        public RabbitMQConfiguration(IConfiguration configuration)
        {
            var section = configuration.GetSection("RabbitMQ");
            foreach (var configurationSection in section.GetChildren())
            {
                _configuration.TryAdd(configurationSection.Key, configurationSection.Get<ConnectionFactory>());
            }
        }

        /// <summary>
        /// 获取频道实例。
        /// </summary>
        /// <param name="queue">队列名称。</param>
        /// <param name="name">主机配置名称。</param>
        /// <returns>返回频道实例。</returns>
        public IChannel GetOrCreateChannel(string queue, string name = "Default")
        {
            return Channel.GetOrCreate(queue, name, () => Connection.GetOrCreate(name, () =>
            {
                if (!_configuration.TryGetValue(name, out var factory))
                    throw new Exception("没有在配置文件中找到RabbitMQ的相关配置！");
                return factory;
            }));
        }
    }
}

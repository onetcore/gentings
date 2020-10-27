using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Gentings.AspNetCore;
using RabbitMQ.Client;

namespace Gentings.RabbitMQ
{
    /// <summary>
    /// 生产发布实现类。
    /// </summary>
    public class Publisher : IPublisher
    {
        private readonly IRabbitMQConfiguration _configuration;
        /// <summary>
        /// 初始化类<see cref="Publisher"/>。
        /// </summary>
        /// <param name="configuration">配置接口。</param>
        public Publisher(IRabbitMQConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 发布模型实例。
        /// </summary>
        /// <typeparam name="T">模型类型。</typeparam>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回发布结果。</returns>
        public ApiResult Queue<T>(T model)
        {
            var config = typeof(T).GetCustomAttribute<RabbitMQAttribute>(true);
            if (config == null)
                throw new Exception($"类型特性未定义：{typeof(T)}");
            return Queue(config.ExchangeName, config.ExchangeType, config.RoutingKey, model, config.Queue);
        }

        /// <summary>
        /// 发布对象实例。
        /// </summary>
        /// <param name="exchange">交换机名称。</param>
        /// <param name="exchangeType">交换机类型。</param>
        /// <param name="routingKey">路由键。</param>
        /// <param name="body">发送对象。</param>
        /// <param name="queue">队列名称。</param>
        /// <param name="persistent">是否永久性保存。</param>
        /// <param name="expires">过期时间（秒）。</param>
        /// <param name="headers">头部实例。</param>
        /// <param name="settingName">配置名称。</param>
        /// <returns>返回发布结果。</returns>
        public ApiResult Queue(
            string exchange,
            ExchangeType exchangeType,
            string routingKey,
            object body,
            string queue = null,
            bool persistent = true,
            int expires = 0,
            IDictionary<string, object> headers = null,
            string settingName = "Default")
        {
            try
            {
                if (body == null)
                    return ErrorCode.NullBody;
                var channel = _configuration.GetOrCreateChannel(routingKey, settingName);
                channel.ExchangeDeclare(exchange, exchangeType.ToString().ToLower(), persistent, false, null);
                if (!string.IsNullOrEmpty(queue))
                {
                    channel.QueueDeclare(queue, true, false, false, null);
                    channel.QueueBind(queue, exchange, routingKey, null);
                }

                var properties = channel.CreateBasicProperties();
                properties.Persistent = persistent;
                if (expires > 0)
                    properties.Expiration = (expires * 1000).ToString();
                properties.Headers = headers ?? new Dictionary<string, object>();

                var buffer = Encoding.UTF8.GetBytes(body.ToJsonString());
                channel.BasicPublish(exchange, routingKey, properties, buffer);
                return ErrorCode.Success;
            }
            catch (Exception exception)
            {
                return new ApiResult { Message = exception.Message, Code = (int)ErrorCode.Failure };
            }
        }
    }
}
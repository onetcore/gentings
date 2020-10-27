using System.Collections.Generic;
using Gentings.AspNetCore;

namespace Gentings.RabbitMQ
{
    /// <summary>
    /// 生产发布接口。
    /// </summary>
    public interface IPublisher : ISingletonService
    {
        /// <summary>
        /// 发布模型实例。
        /// </summary>
        /// <typeparam name="T">模型类型。</typeparam>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回发布结果。</returns>
        ApiResult Queue<T>(T model);

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
        ApiResult Queue(string exchange, ExchangeType exchangeType, string routingKey, object body, string queue = null,
            bool persistent = true, int expires = 0, IDictionary<string, object> headers = null, string settingName = "Default");
    }
}
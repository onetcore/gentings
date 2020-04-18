using System;
using Microsoft.Extensions.Logging;

namespace Gentings.Data.Initializers
{
    internal class DefaultInitializerHostedService : InitializerHostedService
    {
        /// <summary>
        /// 初始化类<see cref="DefaultInitializerHostedService"/>。
        /// </summary>
        /// <param name="serviceProvider">服务提供者。</param>
        /// <param name="installerManager">安装管理接口。</param>
        /// <param name="logger">日志接口。</param>
        public DefaultInitializerHostedService(IServiceProvider serviceProvider, IInitializerManager installerManager, ILogger<InitializerHostedService> logger) : base(serviceProvider, installerManager, logger)
        {
        }
    }
}
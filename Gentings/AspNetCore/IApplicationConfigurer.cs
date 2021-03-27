﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 应用程序配置接口。
    /// </summary>
    public interface IApplicationConfigurer : IServices
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 配置应用程序实例。
        /// </summary>
        /// <param name="app">应用程序构建实例。</param>
        /// <param name="configuration">配置接口。</param>
        void Configure(IApplicationBuilder app, IConfiguration configuration);
    }
}
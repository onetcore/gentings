using System;

namespace Gentings.Data.Initializers
{
    /// <summary>
    /// 注册码。
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// 用户名。
        /// </summary>
        public string UserName { get; set; } = "tester";

        /// <summary>
        /// 注册码。
        /// </summary>
        public string Password { get; set; } = Cores.Md5("gentings for aspnetcore");

        /// <summary>
        /// 过期时间。
        /// </summary>
        public DateTime Expired { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// 安装状态。
        /// </summary>
        public InitializerStatus Status { get; set; }
    }
}
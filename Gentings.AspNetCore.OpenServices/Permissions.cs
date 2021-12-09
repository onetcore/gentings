namespace Gentings.AspNetCore.OpenServices
{
    /// <summary>
    /// 权限列表。
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// 访问开放平台。
        /// </summary>
        public const string View = "openservices.view";

        /// <summary>
        /// 添加应用程序。
        /// </summary>
        public const string Create = "openservices.view";

        /// <summary>
        /// 编辑应用程序。
        /// </summary>
        public const string Update = "openservices.update";

        /// <summary>
        /// 删除应用程序。
        /// </summary>
        public const string Delete = "openservices.delete";

        /// <summary>
        /// 配置API服务。
        /// </summary>
        public const string Setting = "openservices.settings";
    }
}
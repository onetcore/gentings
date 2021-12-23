namespace Gentings.Security
{
    /// <summary>
    /// 后台权限。
    /// </summary>
    public class CorePermissions
    {
        /// <summary>
        /// 后台登录权限。
        /// </summary>
        public const string Administrator = "core.administrator";

        /// <summary>
        /// 拥有者权限，配置网站信息。
        /// </summary>
        public const string Owner = "core.owner";

        /// <summary>
        /// 开发者权限，建站时候初始化的信息。
        /// </summary>
        public const string Developer = "core.developer";

    }
}

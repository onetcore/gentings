using Gentings.Security.Properties;

namespace Gentings.Security.Permissions
{
    /// <summary>
    /// 默认权限。
    /// </summary>
    public class DefaultPermissions : PermissionProvider
    {
        /// <summary>
        /// 后台管理员，可以登录到后台。
        /// </summary>
        public const string Administrator = "core.admin";

        /// <summary>
        /// 拥有者权限，配置网站信息。
        /// </summary>
        public const string Owner = "core.owner";

        /// <summary>
        /// 开发者权限，建站时候初始化的信息。
        /// </summary>
        public const string Developer = "core.developer";

        /// <summary>
        /// 初始化权限实例。
        /// </summary>
        protected override void Init()
        {
            Add("admin", Resources.DefaultPermissions_Administrator_Name, Resources.DefaultPermissions_Administrator_Description);
            Add("owner", Resources.DefaultPermissions_Owner_Name, Resources.DefaultPermissions_Owner_Description);
            Add("developer", Resources.DefaultPermissions_Developer_Name, Resources.DefaultPermissions_Developer_Description);
        }
    }
}
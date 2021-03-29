using Gentings.Security.Properties;

namespace Gentings.Security.Permissions
{
    /// <summary>
    /// 默认权限。
    /// </summary>
    public class DefaultPermissions : PermissionProvider
    {
        /// <summary>
        /// 登录后台。
        /// </summary>
        public const string Administrator = "core.admin";

        /// <summary>
        /// 初始化权限实例。
        /// </summary>
        protected override void Init()
        {
            Add("admin", Resources.DefaultPermissions_Administrator_Name, Resources.DefaultPermissions_Administrator_Description);
        }
    }
}
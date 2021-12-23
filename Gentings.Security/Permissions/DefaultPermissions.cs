using Gentings.Security.Properties;

namespace Gentings.Security.Permissions
{
    /// <summary>
    /// 默认权限。
    /// </summary>
    public class DefaultPermissions : PermissionProvider
    {
        /// <summary>
        /// 初始化权限实例。
        /// </summary>
        protected override void Init()
        {
            Add("administrator", Resources.DefaultPermissions_Administrator_Name, Resources.DefaultPermissions_Administrator_Description);
            Add("owner", Resources.DefaultPermissions_Owner_Name, Resources.DefaultPermissions_Owner_Description);
            Add("developer", Resources.DefaultPermissions_Developer_Name, Resources.DefaultPermissions_Developer_Description);
        }
    }
}
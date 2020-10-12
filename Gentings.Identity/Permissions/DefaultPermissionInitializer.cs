namespace Gentings.Identity.Permissions
{
    internal class DefaultPermissionInitializer : PermissionInitializer
    {
        /// <summary>
        /// 初始化类<see cref="DefaultPermissionInitializer"/>。
        /// </summary>
        /// <param name="permissionManager">权限管理类。</param>
        public DefaultPermissionInitializer(IPermissionManager permissionManager) : base(permissionManager)
        {
        }
    }
}
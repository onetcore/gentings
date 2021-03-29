namespace Gentings.Security
{
    /// <summary>
    /// 用户角色接口。
    /// </summary>
    public interface IUserRole
    {
        /// <summary>
        /// 角色ID。
        /// </summary>
        int RoleId { get; set; }

        /// <summary>
        /// 用户ID。
        /// </summary>
        int UserId { get; set; }
    }
}
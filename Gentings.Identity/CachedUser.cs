using System.Text.Json.Serialization;

namespace Gentings.Identity
{
    /// <summary>
    /// 缓存用户实例。
    /// </summary>
    public class CachedUser
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名称。
        /// </summary>
        [JsonIgnore]
        public string UserName { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 真实姓名。
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 角色名称。
        /// </summary>
        [JsonIgnore]
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名称。
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 电话号码。
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
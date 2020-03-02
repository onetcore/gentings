using Gentings.Data;
using Gentings.Extensions;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 用户管理接口。
    /// </summary>
    public interface IUserManager : IObjectManager<User>, ISingletonService
    {

    }

    /// <summary>
    /// 用户管理接口。
    /// </summary>
    public class UserManager : ObjectManager<User>, IUserManager
    {
        /// <summary>
        /// 初始化类<see cref="UserManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        public UserManager(IDbContext<User> context) : base(context)
        {
        }
    }
}

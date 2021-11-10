using Gentings;
using Gentings.Data;
using Gentings.Extensions;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 管理员管理接口。
    /// </summary>
    public interface IUserManager : IObjectManager<User>, ISingletonService
    {
    }

    /// <summary>
    /// 管理员管理实现类。
    /// </summary>
    public class UserManager : ObjectManager<User>, IUserManager
    {
        /// <summary>
        /// 初始化类<see cref="User"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        public UserManager(IDbContext<User> context) : base(context)
        {
        }
    }

}


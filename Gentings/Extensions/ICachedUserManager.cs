using System.Threading.Tasks;

namespace Gentings.Extensions
{
    /// <summary>
    /// 缓存用户接口。
    /// </summary>
    public interface ICachedUserManager : IScopedService
    {
        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回缓存用户实例对象。</returns>
        IUser GetCachedUser(int id);

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回缓存用户实例对象。</returns>
        Task<IUser> GetCachedUserAsync(int id);
    }
}
using Gentings;
using Gentings.Data;
using Gentings.Extensions;
using Gentings.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 管理员管理接口。
    /// </summary>
    public interface IUserManager : IObjectManager<User>, Gentings.Security.IUserManager
    {
        /// <summary>
        /// 获取当前登录用户。
        /// </summary>
        /// <returns>返回当前登录用户。</returns>
        User GetUser();

        /// <summary>
        /// 刷新用户缓存。
        /// </summary>
        /// <param name="id">用户Id。</param>
        void Refresh(int id);
    }

    /// <summary>
    /// 管理员管理实现类。
    /// </summary>
    public class UserManager : ObjectManager<User>, IUserManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// 初始化类<see cref="User"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="contextAccessor">HTTP上下文访问实例。</param>
        /// <param name="cache">缓存接口实例。</param>
        public UserManager(IDbContext<User> context, IHttpContextAccessor contextAccessor, IMemoryCache cache) : base(context)
        {
            _contextAccessor = contextAccessor;
            _cache = cache;
        }

        private string GetCacheKey(int id)
        {
            return $"users:{id}";
        }

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回缓存用户实例对象。</returns>
        public IUser GetCachedUser(int id)
        {
            return _cache.GetOrCreate(GetCacheKey(id), ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                return Find(id);
            });
        }

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回缓存用户实例对象。</returns>
        public async Task<IUser> GetCachedUserAsync(int id)
        {
            return await _cache.GetOrCreateAsync(GetCacheKey(id), async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                return await FindAsync(id);
            });
        }

        /// <summary>
        /// 获取当前登录用户。
        /// </summary>
        /// <returns>返回当前登录用户。</returns>
        public User GetUser()
        {
            if (_contextAccessor.HttpContext == null)
                return new User();
            return Find(_contextAccessor.HttpContext.User.GetUserId());
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        public override bool IsDuplicated(User model)
        {
            return Context.Any(x => (x.UserName == model.UserName || x.NickName == model.NickName) && x.Id != model.Id);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        public override Task<bool> IsDuplicatedAsync(User model, CancellationToken cancellationToken = default)
        {
            return Context.AnyAsync(x => (x.UserName == model.UserName || x.NickName == model.NickName) && x.Id != model.Id, cancellationToken);
        }

        /// <summary>
        /// 刷新用户缓存。
        /// </summary>
        /// <param name="id">用户Id。</param>
        public void Refresh(int id)
        {
            _cache.Remove(GetCacheKey(id));
        }
    }
}


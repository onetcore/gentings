using Gentings.Data;
using Gentings.Data.Initializers;
using Gentings.Localization;
using System.Threading.Tasks;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 用户初始化。
    /// </summary>
    public class DataInitializer : IInitializer
    {
        private readonly IDbContext<User> _context;
        private readonly ILocalizer _localizer;

        /// <summary>
        /// 初始化类<see cref="DataInitializer"/>。
        /// </summary>
        /// <param name="context">用户数据操作接口。</param>
        /// <param name="localizer">本地日志接口实例。</param>
        public DataInitializer(IDbContext<User> context, ILocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        /// <summary>
        /// 优先级，越大越靠前。
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// 安装时候预先执行的接口。
        /// </summary>
        /// <returns>返回执行结果。</returns>
        public Task<bool> ExecuteAsync()
        {
            var user = new User();
            user.UserName = "admin";
            user.NickName = _localizer["admin"];
            user.Password = "adminadmin";
            user.Password = User.Hashed(user.UserName, user.Password);
            return _context.CreateAsync(user);
        }

        /// <summary>
        /// 判断是否禁用。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public Task<bool> IsDisabledAsync()
        {
            return _context.AnyAsync();
        }
    }
}
using Gentings.Extensions;
using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GS.Areas.Backend.Pages.Users
{
    /// <summary>
    /// 用户列表。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IUserManager _userManager;

        public IndexModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 用户查询实例。
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public UserQuery Query { get; set; }

        /// <summary>
        /// 用户列表。
        /// </summary>
        public IPageEnumerable<User> Items { get; private set; }

        public void OnGet()
        {
            Items = _userManager.Load(Query);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.FindAsync(id);
            if (user == null)
                return Error("用户不存在！");
            if (user?.UserName == UserName)
                return Error("不能删除自己的账户！");
            var result = await _userManager.DeleteAsync(id);
            return Json(result, user.NickName);
        }
    }
}
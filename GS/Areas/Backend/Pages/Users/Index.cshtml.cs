using Gentings.Extensions;
using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        public async Task<IActionResult> OnPostDeleteAsync(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("请先选择用户后再进行删除操作！");
            if (id.Any(x => x == UserId))
                return Error("不能删除自己的账户！");
            var result = await _userManager.DeleteAsync(id);
            return Json(result, "用户");
        }
    }
}
using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GS.Areas.Backend.Pages.Users
{
    /// <summary>
    /// 添加用户。
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly IUserManager _userManager;

        public EditModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public IActionResult OnGet(int id)
        {
            var user = _userManager.Find(id);
            if (user == null)
                return BadRequest();
            Input = new InputModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
                Email = user.Email,
                Avatar = user.Avatar
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindAsync(Input.UserId);
                if (user == null)
                    return Error("用户不存在！");
                user.NickName = Input.NickName;
                user.Email = Input.Email;
                user.Avatar = Input.Avatar;
                if (!await _userManager.IsDuplicatedAsync(user))
                {
                    var result = await _userManager.UpdateAsync(user);
                    return Json(result, Gentings.Extensions.DataAction.Updated, "用户");
                }
                else
                {
                    ModelState.AddModelError("Input.NickName", "昵称已经存在！");
                }
            }
            return Error();
        }

        /// <summary>
        /// 输入模型实例对象。
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// 输入模型类型。
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// 用户Id。
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// 用户名称。
            /// </summary>
            [Required(ErrorMessage = "用户名不能为空！")]
            public string UserName { get; set; }

            /// <summary>
            /// 昵称。
            /// </summary>
            public string? NickName { get; set; }

            /// <summary>
            /// 头像。
            /// </summary>
            public string? Avatar { get; set; }

            /// <summary>
            /// 电子邮件。
            /// </summary>
            [DataType(DataType.EmailAddress)]
            public string? Email { get; set; }
        }
    }
}

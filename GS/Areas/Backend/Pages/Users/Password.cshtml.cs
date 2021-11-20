using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GS.Areas.Backend.Pages.Users
{
    /// <summary>
    /// 添加用户。
    /// </summary>
    public class PasswordModel : ModelBase
    {
        private readonly IUserManager _userManager;

        public PasswordModel(IUserManager userManager)
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
                UserName = user.UserName
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
                user.Password = Extensions.Security.User.Hashed(user.UserName, Input.Password);
                user.UpdatedDate = DateTimeOffset.Now;
                var result = await _userManager.UpdateAsync(user.Id, new { user.Password, user.UpdatedDate });
                return Json(result, Gentings.Extensions.DataAction.Updated, "用户密码");
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
            /// 确认密码。
            /// </summary>
            [Required(ErrorMessage = "确认密码不能为空！")]
            [Compare("Password", ErrorMessage = "确认密码和密码不匹配！")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// 密码。
            /// </summary>
            [Required(ErrorMessage = "密码不能为空！")]
            public string Password { get; set; }
        }
    }
}

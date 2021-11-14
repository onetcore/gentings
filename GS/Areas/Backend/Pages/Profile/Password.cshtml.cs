using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GS.Areas.Backend.Pages.Profile
{
    /// <summary>
    /// 修改密码。
    /// </summary>
    public class PasswordModel : ModelBase
    {
        private readonly IUserManager _userManager;

        public PasswordModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 输入模型。
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// 用户名称。
            /// </summary>
            [Required(ErrorMessage = "用户名称不能为空！")]
            public string UserName { get; set; }
            /// <summary>
            /// 原始密码。
            /// </summary>
            [Required(ErrorMessage = "原始密码不能为空！")]
            public string SrcPassword { get; set; }
            /// <summary>
            /// 新密码。
            /// </summary>
            [Required(ErrorMessage = "新密码不能为空！")]
            [Compare("ConfirmPassword", ErrorMessage = "新密码和确认密码不匹配！")]
            public string Password { get; set; }
            /// <summary>
            /// 确认密码。
            /// </summary>
            [Required(ErrorMessage = "确认密码不能为空！")]
            public string ConfirmPassword { get; set; }
        }

        /// <summary>
        /// 输入模型实例。
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet()
        {
            Input = new InputModel { UserName = UserName };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = CurrentUser;
                if (!user.IsValid(Input.SrcPassword))
                {
                    ModelState.AddModelError("Input.SrcPassword", "原始密码不正确！");
                    return Page();
                }
                user.UserName = Input.UserName.Trim();
                user.Password = Extensions.Security.User.Hashed(user.UserName, Input.Password);
                user.UpdatedDate = DateTimeOffset.Now;
                if (await _userManager.UpdateAsync(user.Id, new { user.UserName, user.Password, user.UpdatedDate }))
                    return SuccessPage("你已经成功更新了密码，下次使用新密码进行登录！");
            }
            return ErrorPage("修改密码失败，请重试！");
        }
    }
}
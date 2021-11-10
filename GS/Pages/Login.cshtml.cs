using Gentings.AspNetCore;
using GS.Extensions.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GS.Pages
{
    /// <summary>
    /// 登录页面。
    /// </summary>
    public class LoginModel : ModelBase
    {
        private readonly IUserManager _userManager;
        /// <summary>
        /// 初始化类<see cref="LoginModel"/>。
        /// </summary>
        /// <param name="userManager">用户管理接口。</param>
        public LoginModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 用户登录输入模型。
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// 登录用户模型。
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// 用户名。
            /// </summary>
            [Required(ErrorMessage = "用户名不能为空!")]
            public string UserName { get; set; }

            /// <summary>
            /// 密码。
            /// </summary>
            [Required(ErrorMessage = "密码不能为空!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            /// 验证码。
            /// </summary>
            public string? Code { get; set; }

            /// <summary>
            /// 登录状态。
            /// </summary>
            public bool RememberMe { get; set; }
        }

        /// <summary>
        /// 返回的URL地址。
        /// </summary>
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// 错误消息。
        /// </summary>
        public string? ErrorMessage { get; private set; }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                if (Settings.ValidCode && !IsCodeValid("login", Input.Code))
                {
                    ModelState.AddModelError("Input.Code", "验证码不正确！");
                    return Page();
                }

                returnUrl ??= Url.Page("/Index", new { area = "Backend" })!;
                Input.UserName = Input.UserName.Trim();
                Input.Password = Input.Password.Trim();

                var user = await _userManager.FindAsync(x => x.UserName == Input.UserName);
                if (user == null)
                {
                    ErrorMessage = "用户名或者密码错误！";
                    return Page();
                }
                if (user.IsValid(Input.Password))
                {
                    user.LastLoginDate = DateTimeOffset.Now;
                    user.LoginIP = HttpContext.GetUserAddress();
                    await _userManager.UpdateAsync(user.Id, new { user.LoginIP, user.LastLoginDate });
                    await SignInAsync(user, CookieAuthenticationDefaults.AuthenticationScheme);
                    return LocalRedirect(returnUrl);
                }

                ErrorMessage = "用户名或者密码错误！";
                return Page();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
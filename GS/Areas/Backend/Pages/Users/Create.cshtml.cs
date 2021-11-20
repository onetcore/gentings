using GS.Extensions.Security;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GS.Areas.Backend.Pages.Users
{
    /// <summary>
    /// 添加用户。
    /// </summary>
    public class CreateModel : ModelBase
    {
        private readonly IUserManager _userManager;

        public CreateModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Input.NickName))
                    Input.NickName = Input.UserName;
                var user = new User
                {
                    UserName = Input.UserName,
                    NickName = Input.NickName,
                    Email = Input.Email,
                    Password = Extensions.Security.User.Hashed(Input.UserName, Input.Password)
                };
                if (!await _userManager.IsDuplicatedAsync(user))
                {
                    var result = await _userManager.CreateAsync(user);
                    return Json(result, Gentings.Extensions.DataAction.Created, "用户");
                }
                else
                {
                    ModelState.AddModelError("Input.UserName", "用户名已经存在！");
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
            /// 用户名称。
            /// </summary>
            [Required(ErrorMessage = "用户名不能为空！")]
            public string UserName { get; set; }

            /// <summary>
            /// 昵称。
            /// </summary>
            public string? NickName { get; set; }

            /// <summary>
            /// 密码。
            /// </summary>
            [Required(ErrorMessage = "密码不能为空！")]
            public string Password { get; set; }

            /// <summary>
            /// 确认密码。
            /// </summary>
            [Required(ErrorMessage = "确认密码不能为空！")]
            [Compare("Password", ErrorMessage = "密码和确认密码不匹配！")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// 电子邮件。
            /// </summary>
            [DataType(DataType.EmailAddress)]
            public string? Email { get; set; }
        }
    }
}

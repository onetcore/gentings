using Gentings.Extensions.Settings;
using Gentings.Storages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GS.Areas.Backend.Pages.Profile
{
    public class IndexModel : ModelBase
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IStorageDirectory _storageDirectory;

        public IndexModel(ISettingsManager settingsManager, IStorageDirectory storageDirectory)
        {
            _settingsManager = settingsManager;
            _storageDirectory = storageDirectory;
        }

        public class InputModel
        {
            /// <summary>
            /// 用户名。
            /// </summary>
            [Required(ErrorMessage = "用户名称不能为空！")]
            public string UserName { get; set; }
            /// <summary>
            /// 电子邮件。
            /// </summary>
            [DataType(DataType.EmailAddress)]
            public string? Email { get; set; }
            /// <summary>
            /// 昵称。
            /// </summary>
            public string? NickName { get; set; }
            /// <summary>
            /// 头像地址。
            /// </summary>
            public string? Avatar { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet()
        {
            Input = new InputModel
            {
                UserName = UserName,
                Email = CurrentUser.Email,
                NickName = CurrentUser.NickName,
                Avatar = CurrentUser.Avatar
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = CurrentUser;
                user.Email = Input.Email;
                user.NickName = Input.NickName;
                user.Avatar = Input.Avatar;
                if (await _settingsManager.SaveSettingsAsync(user))
                    return SuccessPage("你已经成功更新了用户信息！");
            }
            return ErrorPage("更新用户信息失败，请重试！");
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            var result = await _storageDirectory.SaveAsync(file, "user", "avatar.png");
            return Json(result);
        }
    }
}
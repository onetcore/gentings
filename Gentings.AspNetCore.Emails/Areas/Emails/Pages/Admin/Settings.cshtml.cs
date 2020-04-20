using Gentings.Extensions.Emails;
using Gentings.Extensions.Settings;
using Gentings.Identity.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Admin
{
    [PermissionAuthorize(Permissions.Settings)]
    public class SettingsModel : ModelBase
    {
        private readonly ISettingsManager _settingsManager;

        public SettingsModel(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        [BindProperty]
        public EmailSettings Input { get; set; }

        public void OnGet()
        {
            Input = _settingsManager.GetSettings<EmailSettings>();
        }

        public IActionResult OnPost()
        {
            if (Input.Enabled && !ModelState.IsValid)
                return ErrorPage("更改邮件信息配置失败！");

            if (_settingsManager.SaveSettings(Input))
            {
                Log("修改了邮件配置！");
                return SuccessPage("你已经成功配置了信息！");
            }

            return ErrorPage("更改邮件信息配置失败！");
        }
    }
}
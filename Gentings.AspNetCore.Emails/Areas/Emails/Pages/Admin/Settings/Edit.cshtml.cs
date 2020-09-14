using Gentings.Extensions.Emails;
using Gentings.Identity.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Admin.Settings
{
    [PermissionAuthorize(Permissions.Settings)]
    public class EditModel : ModelBase
    {
        private readonly IEmailSettingsManager _settingsManager;

        public EditModel(IEmailSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        [BindProperty]
        public EmailSettings Input { get; set; }

        public void OnGet(int id)
        {
            Input = _settingsManager.Find(id);
        }

        public IActionResult OnPost()
        {
            if (Input.Enabled && !ModelState.IsValid)
                return Error("更改邮件信息配置失败！");

            if (_settingsManager.Save(Input))
            {
                Log("修改了邮件配置！");
                return Success("你已经成功配置了信息！");
            }

            return Error("更改邮件信息配置失败！");
        }
    }
}
using System.Collections.Generic;
using Gentings.Extensions.Emails;
using Gentings.Identity.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Admin.Settings
{
    [PermissionAuthorize(Permissions.Email)]
    public class IndexModel : ModelBase
    {
        private readonly IEmailSettingsManager _settingsManager;

        public IndexModel(IEmailSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public IEnumerable<EmailSettings> Settings { get; private set; }

        public void OnGet()
        {
            Settings = _settingsManager.Fetch();
        }

        public IActionResult OnPostDelete(int id)
        {
            var result = _settingsManager.Delete(id);
            return Json(result, "配置");
        }
    }
}
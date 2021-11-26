using Gentings.Extensions.Emails;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Backend.Settings
{
    /// <summary>
    /// 邮件配置列表。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IEmailSettingsManager _settingsManager;
        /// <summary>
        /// 初始化类<see cref="IndexModel"/>。
        /// </summary>
        /// <param name="settingsManager">邮件配置接口。</param>
        public IndexModel(IEmailSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        /// <summary>
        /// 邮件配置列表。
        /// </summary>
        public IEnumerable<EmailSettings> Items { get; private set; }

        /// <summary>
        /// 获取邮件配置列表。
        /// </summary>
        public void OnGet()
        {
            Items = _settingsManager.Fetch();
        }

        /// <summary>
        /// 删除邮件配置。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult OnPostDelete(int id)
        {
            var result = _settingsManager.Delete(id);
            return Json(result, "配置");
        }
    }
}
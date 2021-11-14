using Gentings.Extensions.Settings;
using GS.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GS.Areas.Backend.Pages
{
    /// <summary>
    /// 网站配置。
    /// </summary>
    public class SettingsModel : ModelBase
    {
        private readonly ISettingsManager _settingsManager;
        /// <summary>
        /// 初始化类<see cref="SettingsModel"/>。
        /// </summary>
        /// <param name="settingsManager">配置管理接口实例。</param>
        public SettingsModel(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        /// <summary>
        /// 输入模型。
        /// </summary>
        [BindProperty]
        public SiteSettings Input { get; set; }

        public void OnGet()
        {
            Input = Settings;
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Input.SiteName))
            {
                ModelState.AddModelError("Input.SiteName", "网站名称不能为空！");
                return Error();
            }
            var result = _settingsManager.SaveSettings(Input);
            if (result)
                return SuccessPage("恭喜你，你已经成功更新了网站配置！");
            return Page("更新网站配置失败，请重试！");
        }
    }
}
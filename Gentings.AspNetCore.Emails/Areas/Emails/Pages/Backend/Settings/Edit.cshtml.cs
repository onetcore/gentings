using Gentings.Extensions.Emails;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Backend.Settings
{
    /// <summary>
    /// 添加和编辑电子邮件配置。
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly IEmailSettingsManager _settingsManager;
        /// <summary>
        /// 初始化类<see cref="EditModel"/>。
        /// </summary>
        /// <param name="settingsManager">邮件配置管理接口。</param>
        public EditModel(IEmailSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        /// <summary>
        /// 电子邮件配置输入模型。
        /// </summary>
        [BindProperty]
        public EmailSettings Input { get; set; }

        /// <summary>
        /// 获取电子邮件配置。
        /// </summary>
        /// <param name="id">配置Id。</param>
        public void OnGet(int id)
        {
            Input = _settingsManager.Find(id);
        }

        /// <summary>
        /// 保存电子邮件配置。
        /// </summary>
        public IActionResult OnPost()
        {
            var valid = true;
            if (string.IsNullOrEmpty(Input.SmtpServer))
            {
                valid = false;
                ModelState.AddModelError("Input.SmtpServer", "SMTP服务器不能为空！");
            }
            if (Input.SmtpPort <= 0)
            {
                valid = false;
                ModelState.AddModelError("Input.SmtpPort", "SMTP端口不能为空！");
            }
            if (string.IsNullOrEmpty(Input.SmtpUserName))
            {
                valid = false;
                ModelState.AddModelError("Input.SmtpUserName", "SMTP用户名不能为空！");
            }
            if (string.IsNullOrEmpty(Input.SmtpPassword))
            {
                valid = false;
                ModelState.AddModelError("Input.SmtpPassword", "SMTP密码不能为空！");
            }
            if (valid)
            {
                if (_settingsManager.Save(Input))
                {
                    Log("修改了邮件配置！");
                    return Success("你已经成功配置了信息！");
                }

                return Error("更改邮件信息配置失败！");
            }

            return Error();
        }
    }
}
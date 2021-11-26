using Gentings.Extensions;
using Gentings.Extensions.Emails;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Backend
{
    /// <summary>
    /// 邮件发送列表。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IEmailManager _messageManager;
        /// <summary>
        /// 初始化类<see cref="IndexModel"/>。
        /// </summary>
        /// <param name="messageManager">电子邮件管理接口。</param>
        public IndexModel(IEmailManager messageManager)
        {
            _messageManager = messageManager;
        }

        /// <summary>
        /// 邮件查询实例。
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public EmailQuery Query { get; set; }

        /// <summary>
        /// 邮件列表。
        /// </summary>
        public IPageEnumerable<Email> Emails { get; private set; }

        /// <summary>
        /// 获取邮件列表。
        /// </summary>
        public void OnGet()
        {
            Emails = _messageManager.Load(Query);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("请先选择邮件再进行删除操作！");
            var result = await _messageManager.DeleteAsync(id);
            return Json(result, DataAction.Deleted, "邮件");
        }
    }
}
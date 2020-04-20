using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gentings.Extensions.Emails;
using Gentings.Identity.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Admin
{
    [PermissionAuthorize(Permissions.Email)]
    public class SendModel : ModelBase
    {
        private readonly IEmailManager _messageManager;

        public SendModel(IEmailManager messageManager)
        {
            _messageManager = messageManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public int Id { get; set; }

            [DisplayName("标题")]
            [Required(ErrorMessage = "{0}不能为空！")]
            public string Title { get; set; }

            [DisplayName("内容")]
            [Required(ErrorMessage = "{0}不能为空！")]
            public string Content { get; set; }

            /// <summary>
            /// 源代码。
            /// </summary>
            public string Source { get; set; }

            [DisplayName("邮件地址")]
            [Required(ErrorMessage = "{0}不能为空！")]
            public string To { get; set; }
        }

        public void OnGet(int id)
        {
            var message = _messageManager.Find(id) ?? new Email();
            Input = new InputModel
            {
                Title = message.Title,
                Content = message.Content,
                To = message.To,
                Id = id,
                Source = message.Source ?? message.Content
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Error();
            Email message;
            if (Input.Id > 0)
            {
                message = _messageManager.Find(Input.Id);
                if (message == null)
                    return Error("邮件不存在！");
                message.Title = Input.Title;
                message.Content = Input.Content;
                message.To = Input.To;
                message.Source = Input.Source;
                var hashKey = message.HashKey;
                message.HashKey = null;
                if (hashKey == message.HashKey || _messageManager.Update(Input.Id, new { Input.Title, Input.Content, message.ExtendProperties, Input.To, message.HashKey, Status = EmailStatus.Pending, TryTimes = 0 }))
                {
                    Notifier.Send(UserId, "邮件", "发送了一个电子邮件");
                    return Success("你已经成功发送邮件！");
                }
                return Error("发送邮件失败！");
            }

            message = new Email
            {
                UserId = UserId,
            };
            message.Title = Input.Title;
            message.Content = Input.Content;
            message.Source = Input.Source;
            message.To = Input.To;
            if (_messageManager.Save(message))
            {
                Notifier.Send(UserId, "邮件", "发送了一个电子邮件");
                return Success("你已经成功发送邮件！");
            }
            return Error("发送邮件失败！");
        }
    }
}
using Gentings.Extensions;
using Gentings.Extensions.Emails;
using Gentings.Identity.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Admin
{
    [PermissionAuthorize(Permissions.Email)]
    public class IndexModel : ModelBase
    {
        private readonly IEmailManager _messageManager;

        public IndexModel(IEmailManager messageManager)
        {
            _messageManager = messageManager;
        }

        [BindProperty(SupportsGet = true)]
        public EmailQuery Query { get; set; }

        public IPageEnumerable<Email> Emails { get; private set; }

        public void OnGet()
        {
            Emails = _messageManager.Load(Query);
        }
    }
}
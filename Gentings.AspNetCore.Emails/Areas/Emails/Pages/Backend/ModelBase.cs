using Microsoft.AspNetCore.Authorization;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Backend
{
    [Authorize]
    public abstract class ModelBase : AspNetCore.ModelBase
    {
    }
}

using Gentings.Security;

namespace Gentings.AspNetCore.Emails.Areas.Emails.Pages.Backend
{
    [PermissionAuthorize]
    public abstract class ModelBase : AspNetCore.ModelBase
    {
    }
}

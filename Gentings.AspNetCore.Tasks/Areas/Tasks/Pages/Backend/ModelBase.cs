using Microsoft.AspNetCore.Authorization;

namespace Gentings.AspNetCore.Tasks.Areas.Tasks.Pages.Backend
{
    [Authorize]
    public abstract class ModelBase : AspNetCore.ModelBase
    {
    }
}

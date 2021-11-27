using Microsoft.AspNetCore.Authorization;

namespace Gentings.AspNetCore.Storages.Areas.Tasks.Storages.Backend
{
    /// <summary>
    /// 模型基类。
    /// </summary>
    [Authorize]
    public abstract class ModelBase : AspNetCore.ModelBase
    {

    }
}

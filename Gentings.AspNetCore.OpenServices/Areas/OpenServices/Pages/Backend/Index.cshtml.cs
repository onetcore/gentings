using Gentings.AspNetCore.Options;
using Gentings.Extensions;
using Gentings.Extensions.OpenServices;
using Gentings.Security;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.OpenServices.Areas.OpenServices.Pages.Backend
{
    /// <summary>
    /// 应用程序列表。
    /// </summary>
    [TUserModelUI(typeof(IndexModel<>))]
    public abstract class IndexModel : ModelBase
    {
        /// <summary>
        /// 应用程序列表。
        /// </summary>
        public IPageEnumerable<Application> Items { get; protected set; }

        /// <summary>
        /// 应用程序查询实例。
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public ApplicationQuery Query { get; set; }

        /// <summary>
        /// 获取应用程序页面实例。
        /// </summary>
        public virtual void OnGet() => throw new NotImplementedException();

        /// <summary>
        /// 删除应用程序。
        /// </summary>
        /// <param name="id">应用程序Id。</param>
        /// <returns>返回删除应用程序结果。</returns>
        public virtual Task<IActionResult> OnPostDeleteAsync(Guid[] id) => throw new NotImplementedException();
    }

    internal class IndexModel<TUser> : IndexModel where TUser : class, IUser
    {
        private readonly IApplicationManager _applicationManager;
        public IndexModel(IApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        public override void OnGet()
        {
            var query = new ApplicationQuery<TUser>(Query);
            Items = _applicationManager.Load(query);
        }

        public override async Task<IActionResult> OnPostDeleteAsync(Guid[] id)
        {
            if (id == null || id.Length == 0)
                return Error("请选择应用后再进行删除操作！");
            var result = await _applicationManager.DeleteAsync(id);
            return Json(result, "应用");
        }
    }
}

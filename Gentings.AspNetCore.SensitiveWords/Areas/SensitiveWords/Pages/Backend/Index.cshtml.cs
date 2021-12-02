using Gentings.Extensions;
using Gentings.Extensions.SensitiveWords;
using Gentings.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.SensitiveWords.Areas.SensitiveWords.Pages.Backend
{
    /// <summary>
    /// 敏感词汇管理。
    /// </summary>
    [PermissionAuthorize(Permissions.Sensitive)]
    public class IndexModel : ModelBase
    {
        private readonly ISensitiveWordManager _sensitiveWordManager;
        /// <summary>
        /// 初始化类<see cref="IndexModel"/>。
        /// </summary>
        /// <param name="sensitiveWordManager">敏感词汇管理接口。</param>
        public IndexModel(ISensitiveWordManager sensitiveWordManager)
        {
            _sensitiveWordManager = sensitiveWordManager;
        }

        /// <summary>
        /// 敏感词汇查询实例。
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public SensitiveWordQuery Query { get; set; }

        /// <summary>
        /// 敏感词汇查询列表。
        /// </summary>
        public IPageEnumerable<SensitiveWord> Words { get; set; }

        /// <summary>
        /// 获取敏感词汇。
        /// </summary>
        public void OnGet()
        {
            Words = _sensitiveWordManager.Load(Query);
        }

        /// <summary>
        /// 删除敏感词汇。
        /// </summary>
        /// <param name="id">词汇Id列表。</param>
        /// <returns>返回删除结果。</returns>
        public IActionResult OnPostDelete(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("请选择实例后再进行删除操作！");
            var result = _sensitiveWordManager.Delete(id);
            return Json(result, "敏感词汇");
        }

        /// <summary>
        /// 上传文件导入敏感词汇。
        /// </summary>
        /// <param name="file">上传文件实例。</param>
        /// <returns>返回上传结果。</returns>
        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            if (file.Length == 0)
                return Error("请选择有内容的文件后再上传！");
            var result = await _sensitiveWordManager.ImportAsync(file);
            if (result == -1)
                return Error("请选择有内容的文件后再上传！");
            if (result > 0)
                return Success($"恭喜你已经成功导入了 {result} 个敏感词汇！");
            return Error("上传敏感词汇失败，请重试！");
        }
    }
}
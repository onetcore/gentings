using Microsoft.AspNetCore.Mvc;

namespace GS.Pages
{
    public class IndexModel : ModelBase
    {
        public IActionResult OnGet()
        {
            return ErrorPage("测试一下，消息显示！");
        }
    }
}
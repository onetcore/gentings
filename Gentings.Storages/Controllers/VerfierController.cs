using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = Gentings.AspNetCore.ControllerBase;

namespace Gentings.Storages.Controllers
{
    /// <summary>
    /// 验证码。
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VerfierController : ControllerBase
    {
        /// <summary>
        /// 验证码。
        /// </summary>
        [Route("vcode-{key}.png")]
        public IActionResult Index(string key)
        {
            int number = 6, fontSize = 16, height = 32;
            if (Request.Query.TryGetValue("n", out var qs) && int.TryParse(qs, out var value))
            {
                number = value;
            }

            if (Request.Query.TryGetValue("s", out qs) && int.TryParse(qs, out value))
            {
                fontSize = value;
            }

            if (Request.Query.TryGetValue("h", out qs) && int.TryParse(qs, out value))
            {
                height = value;
            }

            using var ms = Verifiers.Create(out var code, number, fontSize, height);
            Response.Cookies.Append(key, Verifiers.Hashed(code), new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(3) });
            Response.Body.Dispose();
            return File(ms.ToArray(), @"image/png");
        }
    }
}
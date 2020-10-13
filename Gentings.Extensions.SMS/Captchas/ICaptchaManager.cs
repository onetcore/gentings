using System.Threading.Tasks;

namespace Gentings.Extensions.SMS.Captchas
{
    /// <summary>
    /// 验证码管理接口。
    /// </summary>
    public interface ICaptchaManager : ISingletonService
    {
        /// <summary>
        /// 获取验证码。
        /// </summary>
        /// <param name="phoneNumber">电话号码。</param>
        /// <param name="type">类型。</param>
        /// <param name="id">预留Id。</param>
        /// <returns>返回验证码实例。</returns>
        Task<Captcha> GetCaptchaAsync(string phoneNumber, string type, int id = 0);

        /// <summary>
        /// 更新验证码实例。
        /// </summary>
        /// <param name="phoneNumber">电话号码。</param>
        /// <param name="type">类型。</param>
        /// <param name="code">验证码。</param>
        /// <param name="minutes">过期分钟数。</param>
        /// <param name="id">预留Id。</param>
        /// <returns>返回保存结果。</returns>
        Task<bool> SaveCaptchAsync(string phoneNumber, string type, string code, int minutes = 15, int id = 0);

        /// <summary>
        /// 更新验证码实例。
        /// </summary>
        /// <param name="captcha">验证码实例。</param>
        /// <returns>返回保存结果。</returns>
        Task<bool> SaveCaptchAsync(Captcha captcha);

        /// <summary>
        /// 移除长期未使用的电话号码。
        /// </summary>
        /// <returns>返回移除结果。</returns>
        Task<bool> ClearAsync();
    }
}
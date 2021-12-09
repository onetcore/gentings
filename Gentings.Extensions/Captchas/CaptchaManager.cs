using System;
using System.Threading.Tasks;
using Gentings.Data;

namespace Gentings.Extensions.Captchas
{
    /// <summary>
    /// 验证码管理类。
    /// </summary>
    public class CaptchaManager : ICaptchaManager
    {
        private readonly IDbContext<Captcha> _context;
        /// <summary>
        /// 初始化类<see cref="CaptchaManager"/>。
        /// </summary>
        /// <param name="context">数据库操作上下文实例。</param>
        public CaptchaManager(IDbContext<Captcha> context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取验证码。
        /// </summary>
        /// <param name="phoneNumber">电话号码。</param>
        /// <param name="type">类型。</param>
        /// <param name="id">预留Id。</param>
        /// <returns>返回验证码实例。</returns>
        public virtual Task<Captcha> GetCaptchaAsync(string phoneNumber, string type, int id = 0)
        {
            return _context.FindAsync(x => x.PhoneNumber == phoneNumber && x.Type == type && x.Id == id);
        }

        /// <summary>
        /// 更新验证码实例。
        /// </summary>
        /// <param name="phoneNumber">电话号码。</param>
        /// <param name="type">类型。</param>
        /// <param name="code">验证码。</param>
        /// <param name="minutes">过期分钟数。</param>
        /// <param name="id">预留Id。</param>
        /// <returns>返回保存结果。</returns>
        public virtual Task<bool> SaveCaptchAsync(string phoneNumber, string type, string code, int minutes = 15, int id = 0)
        {
            var captchaExpiredDate = DateTimeOffset.Now.AddMinutes(minutes);
            return SaveCaptchAsync(new Captcha
            {
                ExpiredDate = captchaExpiredDate,
                Code = code,
                PhoneNumber = phoneNumber,
                Type = type,
                Id = id
            });
        }

        /// <summary>
        /// 更新验证码实例。
        /// </summary>
        /// <param name="captcha">验证码实例。</param>
        /// <returns>返回保存结果。</returns>
        public virtual async Task<bool> SaveCaptchAsync(Captcha captcha)
        {
            if (await _context.AnyAsync(x => x.PhoneNumber == captcha.PhoneNumber && x.Type == captcha.Type && x.Id == captcha.Id))
            {
                return await _context.UpdateAsync(x => x.PhoneNumber == captcha.PhoneNumber && x.Type == captcha.Type && x.Id == captcha.Id,
                    new { captcha.Code, captcha.ExpiredDate });
            }

            return await _context.CreateAsync(captcha);
        }

        /// <summary>
        /// 移除长期未使用的电话号码。
        /// </summary>
        /// <returns>返回移除结果。</returns>
        public virtual Task<bool> ClearAsync()
        {
            var date = DateTimeOffset.Now.AddDays(-7);
            return _context.DeleteAsync(x => x.ExpiredDate < date);
        }
    }
}
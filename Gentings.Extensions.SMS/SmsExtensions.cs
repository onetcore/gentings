using System;
using System.Text.RegularExpressions;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 扩展方法类。
    /// </summary>
    public static class SmsExtensions
    {
        //移动：134(0 - 8) 、135、136、137、138、139、147、150、151、152、157、158、159、178、182、183、184、187、188、198 
        private static readonly Regex _mobileRegex =
            new Regex(
                "^((134)|(135)|(136)|(137)|(138)|(139)|(147)|(150)|(151)|(152)|(157)|(158)|(159)|(178)|(182)|(183)|(184)|(187)|(188)|(198))\\d{8}$",
                RegexOptions.Compiled | RegexOptions.Singleline);

        //联通：130、131、132、145、155、156、175、176、185、186、166
        private static readonly Regex _unicomRegex =
            new Regex("^((130)|(131)|(132)|(155)|(156)|(145)|(185)|(186)|(176)|(175)|(170)|(171)|(166))\\d{8}$",
                RegexOptions.Compiled | RegexOptions.Singleline);

        //电信：133、153、173、177、180、181、189、199 
        private static readonly Regex _telecomRegex =
            new Regex("^((133)|(153)|(173)|(177)|(180)|(181)|(189)|(199))\\d{8}$",
                RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// 获取当前电话号码的运营商，如果不匹配默认返回<see cref="ServiceType.Mobile"/>。
        /// </summary>
        /// <param name="phoneNumber">电话号码。</param>
        /// <returns>返回判断结果。</returns>
        public static ServiceType GetServiceType(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return ServiceType.None;
            phoneNumber = phoneNumber.Trim();
            if (_mobileRegex.IsMatch(phoneNumber))
                return ServiceType.Mobile;
            if (_telecomRegex.IsMatch(phoneNumber))
                return ServiceType.Telecom;
            if (_unicomRegex.IsMatch(phoneNumber))
                return ServiceType.Unicom;
            return ServiceType.None;
        }

        /// <summary>
        /// 获取短信数量，一条短信按照70个字符计算，多余一条按照65个字符进行计算。
        /// </summary>
        /// <param name="msg">短信内容。</param>
        /// <returns>返回短信数量。</returns>
        public static int GetSmsCount(this string msg)
        {
            if (string.IsNullOrEmpty(msg)) return 0;
            if (msg.Length > 70) return (int)Math.Ceiling(msg.Length * 1.0 / SmsSettings.MultiSize);
            return 1;
        }
    }
}
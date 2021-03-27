using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// MVC扩展类。
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// 添加当前页面试图引入库实例。
        /// </summary>
        /// <param name="viewData">当前试图字典实例。</param>
        /// <param name="libraries">引入库实例。</param>
        public static void AddLibraries(this ViewDataDictionary viewData, ImportLibrary libraries)
        {
            libraries |= viewData.GetLibraries();
            viewData[nameof(ImportLibrary)] = libraries;
        }

        /// <summary>
        /// 获取当前页面试图引入库实例。
        /// </summary>
        /// <param name="viewData">当前试图字典实例。</param>
        /// <returns>返回当前页面试图引入库实例。</returns>
        public static ImportLibrary GetLibraries(this ViewDataDictionary viewData)
        {
            if (viewData.TryGetValue(nameof(ImportLibrary), out var value) &&
                value is ImportLibrary data)
                return data;
            return ImportLibrary.None;
        }

        /// <summary>
        /// 创建JWT访问Token。
        /// </summary>
        /// <param name="configuration">配置实例。</param>
        /// <param name="claims">用户声明列表。</param>
        /// <returns>返回Token字符串。</returns>
        public static string CreateJwtSecurityToken(this IConfiguration configuration, IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToInt32(configuration["Jwt:Expires"]));

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
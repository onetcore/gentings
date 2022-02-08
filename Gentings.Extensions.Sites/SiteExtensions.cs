using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 扩展方法类。
    /// </summary>
    public static class SiteExtensions
    {
        /// <summary>
        /// 拼接链接属性。
        /// </summary>
        /// <param name="builder">当前链接标签。</param>
        /// <param name="link">链接实例。</param>
        public static void Merge(this TagBuilder builder, ILinkable link)
        {
            if (link.Target == OpenTarget.Frame)
                builder.MergeAttribute("target", link.FrameName, true);
            else if (link.Target != OpenTarget.Self)
                builder.MergeAttribute("target", $"_{link.Target.ToString().ToLower()}");
            if (link.Rel != null)
                builder.MergeAttribute("rel", link.Rel.ToString()!.ToLower());
            builder.MergeAttribute("href", link.LinkUrl, true);
        }
    }
}

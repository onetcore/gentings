﻿namespace Gentings.Sites.Categories
{
    /// <summary>
    /// 分类基类。
    /// </summary>
    public abstract class CategoryBase : Extensions.Categories.CategoryBase, ISiteIdObject
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        public int SiteId { get; set; }
    }
}
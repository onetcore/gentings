﻿using System.Collections;
using System.Collections.Generic;

namespace Gentings.Extensions
{
    /// <summary>
    /// 分页迭代接口。
    /// </summary>
    public interface IPageEnumerable : IEnumerable
    {
        /// <summary>
        /// 页码。
        /// </summary>
        int Page { get; }

        /// <summary>
        /// 每页显示记录数。
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 总记录数。
        /// </summary>
        int Size { get; }

        /// <summary>
        /// 总页数。
        /// </summary>
        int Pages { get; }
    }

    /// <summary>
    /// 分页迭代接口。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public interface IPageEnumerable<out TModel> : IPageEnumerable, IEnumerable<TModel>
    {
    }
}
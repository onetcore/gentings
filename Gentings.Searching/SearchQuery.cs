﻿using Gentings.Extensions;

namespace Gentings.Searching
{
    /// <summary>
    /// 查询实例对象。
    /// </summary>
    public class SearchQuery : QueryBase<SearchDescriptor>
    {
        /// <summary>
        /// 搜索名称。
        /// </summary>
        public string? Q { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<SearchDescriptor> context)
        {
            base.Init(context);
            context.InnerJoin<SearchInIndex>((s, i) => s.Id == i.SearchId)
                .InnerJoin<SearchInIndex, SearchIndex>((i, si) => i.IndexId == si.Id)
                .OrderByDescending<SearchIndex>(x => x.Priority)
                .Where<SearchIndex>(s => s.Name!.Contains(Q!))
                .Select()
                .Select<SearchIndex>(x => x.Priority);
        }
    }
}
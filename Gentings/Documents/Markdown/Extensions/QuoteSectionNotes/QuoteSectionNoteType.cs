namespace Gentings.Documents.Markdown.Extensions.QuoteSectionNotes
{
    /// <summary>
    /// 引用节点类型。
    /// </summary>
    public enum QuoteSectionNoteType
    {
        /// <summary>
        /// Markdown引用。
        /// </summary>
        MarkdownQuote = 0,
        /// <summary>
        /// 元素节点。
        /// </summary>
        DFMSection,
        /// <summary>
        /// 通知引用。
        /// </summary>
        DFMNote,
        /// <summary>
        /// 视频引用。
        /// </summary>
        DFMVideo
    }
}

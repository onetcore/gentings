namespace Gentings.Extensions.Sites.Menus
{
    /// <summary>
    /// a和link标签的rel属性。
    /// </summary>
    public enum LinkRel
    {
        /// <summary>
        /// 文档的可选版本（例如打印页、翻译页或镜像）。
        /// </summary>
        Alternate,
        /// <summary>
        /// 文档的外部样式表。
        /// </summary>
        Stylesheet,
        /// <summary>
        /// 集合中的第一个文档。
        /// </summary>
        Start,
        /// <summary>
        /// 集合中的下一个文档。
        /// </summary>
        Next,
        /// <summary>
        /// 集合中的前一个文档。
        /// </summary>
        Prev,
        /// <summary>
        /// 文档目录。
        /// </summary>
        Contents,
        /// <summary>
        /// 文档索引。
        /// </summary>
        Index,
        /// <summary>
        /// 文档中所用字词的术语表或解释。
        /// </summary>
        Glossary,
        /// <summary>
        /// 包含版权信息的文档。
        /// </summary>
        Copyright,
        /// <summary>
        /// 文档的章。
        /// </summary>
        Chapter,
        /// <summary>
        /// 文档的节。
        /// </summary>
        Section,
        /// <summary>
        /// 文档的子段。
        /// </summary>
        Subsection,
        /// <summary>
        /// 文档附录。
        /// </summary>
        Appendix,
        /// <summary>
        /// 帮助文档。
        /// </summary>
        Help,
        /// <summary>
        /// 相关文档。
        /// </summary>
        Bookmark,
        /// <summary>
        /// Google 使用 "nofollow"，用于指定 Google 搜索引擎不要跟踪链接。
        /// </summary>
        Nofollow,
        /// <summary>
        /// 
        /// </summary>
        Licence,
        /// <summary>
        /// 
        /// </summary>
        Tag,
        /// <summary>
        /// 
        /// </summary>
        Friend,
    }
}
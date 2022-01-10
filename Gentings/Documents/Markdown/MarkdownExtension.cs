namespace Gentings.Documents.Markdown
{
    /// <summary>
    /// 扩展标签。
    /// </summary>
    [Flags]
    public enum MarkdownExtension : long
    {
        Common = 0,
        Advanced = 0x00000001,
        Pipetables = 0x00000002,
        GfmPipetables = 0x00000004,
        Emphasisextras = 0x00000008,
        Listextras = 0x00000010,
        Hardlinebreak = 0x00000020,
        Footnotes = 0x00000040,
        Footers = 0x00000080,
        Citations = 0x00000100,
        Attributes = 0x00000200,
        Gridtables = 0x00000400,
        Abbreviations = 0x00000800,
        Emojis = 0x00001000,
        Definitionlists = 0x00002000,
        Customcontainers = 0x00004000,
        Figures = 0x00008000,
        Mathematics = 0x00010000,
        Bootstrap = 0x00020000,
        Medialinks = 0x00040000,
        Smartypants = 0x00080000,
        Autoidentifiers = 0x00100000,
        Tasklists = 0x00200000,
        Diagrams = 0x00400000,
        Nofollowlinks = 0x00800000,
        Noopenerlinks = 0x01000000,
        Noreferrerlinks = 0x02000000,
        Nohtml = 0x04000000,
        Yaml = 0x08000000,
        NonasciiNoescape = 0x10000000,
        Autolinks = 0x20000000,
        Globalization = 0x40000000,
    }
}

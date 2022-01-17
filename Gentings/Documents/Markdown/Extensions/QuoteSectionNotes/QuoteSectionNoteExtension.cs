using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Gentings.Documents.Markdown.Extensions.QuoteSectionNotes
{
    /// <summary>
    /// 引用节点通知扩展类。
    /// </summary>
    public class QuoteSectionNoteExtension : IMarkdownExtension
    {
        void IMarkdownExtension.Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Replace<QuoteBlockParser>(new QuoteSectionNoteParser()))
            {
                pipeline.BlockParsers.Insert(0, new QuoteSectionNoteParser());
            }
        }

        void IMarkdownExtension.Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                QuoteSectionNoteRender quoteSectionNoteRender = new QuoteSectionNoteRender();

                if (!renderer.ObjectRenderers.Replace<QuoteBlockRenderer>(quoteSectionNoteRender))
                {
                    renderer.ObjectRenderers.Insert(0, quoteSectionNoteRender);
                }
            }
        }
    }
}

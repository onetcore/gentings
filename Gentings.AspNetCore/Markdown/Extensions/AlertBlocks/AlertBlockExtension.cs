using Markdig;
using Markdig.Renderers;

namespace Gentings.AspNetCore.Markdown.Extensions.AlertBlocks
{
    /// <summary>
    /// Bootstrap展示alert的div引用。
    /// </summary>
    public class AlertBlockExtension : IMarkdownExtension
    {
        /// <summary>
        /// 附加到管道构建实例中。
        /// </summary>
        /// <param name="pipeline">当前管道构建实例。</param>
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<AlertBlockParser>())
                pipeline.BlockParsers.Insert(0, new AlertBlockParser());
        }

        /// <summary>
        /// 附加到管道构建实例中。
        /// </summary>
        /// <param name="pipeline">当前管道构建实例。</param>
        /// <param name="renderer">Markdown呈现实例对象。</param>
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (!renderer.ObjectRenderers.Contains<AlertBlockRenderer>())
                renderer.ObjectRenderers.Insert(0, new AlertBlockRenderer());
        }
    }
}

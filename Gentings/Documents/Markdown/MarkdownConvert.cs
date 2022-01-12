using Gentings.Documents.Markdown.Extensions.AlertBlocks;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.Bootstrap;
using Markdig.Extensions.Tables;
using System.Collections.Concurrent;

namespace Gentings.Documents.Markdown
{
    /// <summary>
    /// Markdown分析器。
    /// </summary>
    public static class MarkdownConvert
    {
        private static MarkdownPipelineBuilder UseAdvancedExtensions(MarkdownPipelineBuilder pipeline)
        {
            return pipeline
                .UseAbbreviations()
                .UseAutoIdentifiers(AutoIdentifierOptions.AllowOnlyAscii)
                .UseCitations()
                .UseCustomContainers()
                .UseDefinitionLists()
                .UseEmphasisExtras()
                .UseFigures()
                .UseFooters()
                .UseFootnotes()
                .UseGridTables()
                .UseMathematics()
                .UseMediaLinks()
                .UsePipeTables()
                .UseListExtras()
                .UseTaskLists()
                .UseDiagrams()
                .UseAutoLinks()
                .UseGenericAttributes(); // Must be last as it is one parser that is modifying other parsers
        }

        /// <summary>
        /// 使用Markdig扩展。
        /// </summary>
        /// <typeparam name="TExtension">扩展类型。</typeparam>
        /// <param name="builder">当前管道构建实例。</param>
        /// <returns>返回管道构建实例。</returns>
        public static MarkdownPipelineBuilder Use<TExtension>(this MarkdownPipelineBuilder builder)
            where TExtension : IMarkdownExtension, new()
        {
            return builder.Use(new TExtension());
        }

        /// <summary>
        /// 使用Markdig扩展。
        /// </summary>
        /// <typeparam name="TExtension">扩展类型。</typeparam>
        /// <param name="builder">当前管道构建实例。</param>
        /// <param name="extension">扩展实例。</param>
        /// <returns>返回管道构建实例。</returns>
        public static MarkdownPipelineBuilder Use<TExtension>(this MarkdownPipelineBuilder builder, TExtension extension)
            where TExtension : IMarkdownExtension
        {
            if (!builder.Extensions.Contains<TExtension>())
                builder.Extensions.Insert(0, extension);
            return builder;
        }

        /// <summary>
        /// 解析Markdown源代码，并且返回HTML字符串。
        /// </summary>
        /// <param name="source">Markdown源代码。</param>
        /// <param name="action">Markdown配置实例。</param>
        /// <returns>返回解析后的HTML字符串。</returns>
        public static string ToHtml(string source, Action<MarkdownPipelineBuilder> action = null)
        {
            var pipeline = new MarkdownPipelineBuilder();
            if (action != null) action(pipeline);
            else UseAdvancedExtensions(pipeline);

            return ToHtml(source, pipeline);
        }

        /// <summary>
        /// 解析Markdown源代码，并且返回HTML字符串。
        /// </summary>
        /// <param name="source">Markdown源代码。</param>
        /// <param name="extensions">Markdown扩展配置实例。</param>
        /// <returns>返回解析后的HTML字符串。</returns>
        public static string ToHtml(string source, MarkdownExtension extensions)
        {
            var pipeline = Create(extensions);
            return ToHtml(source, pipeline);
        }

        /// <summary>
        /// 解析Markdown源代码，并且返回HTML字符串。
        /// </summary>
        /// <param name="source">Markdown源代码。</param>
        /// <param name="extensions">Markdown配置实例。</param>
        /// <returns>返回解析后的HTML字符串。</returns>
        public static string ToHtml(string source, MarkdownPipelineBuilder pipeline)
        {
            if (pipeline.Extensions.Contains<BootstrapExtension>())
            {
                pipeline.Use<AlertBlockExtension>();
            }

            var build = pipeline.Build();
            return Markdig.Markdown.ToHtml(source, build);
        }

        private static readonly ConcurrentDictionary<MarkdownExtension, MarkdownPipelineBuilder> _builders = new ConcurrentDictionary<MarkdownExtension, MarkdownPipelineBuilder>();
        /// <summary>
        /// 实例化一个管道构建实例。
        /// </summary>
        /// <param name="extensions">Markdown扩展类型。</param>
        /// <returns>返回一个管道构建实例。</returns>
        public static MarkdownPipelineBuilder Create(MarkdownExtension extensions)
        {
            return _builders.GetOrAdd(extensions, x =>
            {
                var pipeline = new MarkdownPipelineBuilder();
                if (extensions == MarkdownExtension.Common)
                {
                    UseAdvancedExtensions(pipeline);
                }
                else
                {
                    foreach (MarkdownExtension extension in Enum.GetValues(typeof(MarkdownExtension)))
                    {
                        if ((extension & extensions) == MarkdownExtension.Common) continue;
                        switch (extension)
                        {
                            case MarkdownExtension.Advanced:
                                UseAdvancedExtensions(pipeline);
                                break;
                            case MarkdownExtension.Pipetables:
                                pipeline.UsePipeTables();
                                break;
                            case MarkdownExtension.GfmPipetables:
                                pipeline.UsePipeTables(new PipeTableOptions { UseHeaderForColumnCount = true });
                                break;
                            case MarkdownExtension.Emphasisextras:
                                pipeline.UseEmphasisExtras();
                                break;
                            case MarkdownExtension.Listextras:
                                pipeline.UseListExtras();
                                break;
                            case MarkdownExtension.Hardlinebreak:
                                pipeline.UseSoftlineBreakAsHardlineBreak();
                                break;
                            case MarkdownExtension.Footnotes:
                                pipeline.UseFootnotes();
                                break;
                            case MarkdownExtension.Footers:
                                pipeline.UseFooters();
                                break;
                            case MarkdownExtension.Citations:
                                pipeline.UseCitations();
                                break;
                            case MarkdownExtension.Attributes:
                                pipeline.UseGenericAttributes();
                                break;
                            case MarkdownExtension.Gridtables:
                                pipeline.UseGridTables();
                                break;
                            case MarkdownExtension.Abbreviations:
                                pipeline.UseAbbreviations();
                                break;
                            case MarkdownExtension.Emojis:
                                pipeline.UseEmojiAndSmiley();
                                break;
                            case MarkdownExtension.Definitionlists:
                                pipeline.UseDefinitionLists();
                                break;
                            case MarkdownExtension.Customcontainers:
                                pipeline.UseCustomContainers();
                                break;
                            case MarkdownExtension.Figures:
                                pipeline.UseFigures();
                                break;
                            case MarkdownExtension.Mathematics:
                                pipeline.UseMathematics();
                                break;
                            case MarkdownExtension.Bootstrap:
                                pipeline.UseBootstrap();
                                pipeline.Use<AlertBlockExtension>();
                                break;
                            case MarkdownExtension.Medialinks:
                                pipeline.UseMediaLinks();
                                break;
                            case MarkdownExtension.Smartypants:
                                pipeline.UseSmartyPants();
                                break;
                            case MarkdownExtension.Autoidentifiers:
                                pipeline.UseAutoIdentifiers(AutoIdentifierOptions.AllowOnlyAscii);
                                break;
                            case MarkdownExtension.Tasklists:
                                pipeline.UseTaskLists();
                                break;
                            case MarkdownExtension.Diagrams:
                                pipeline.UseDiagrams();
                                break;
                            case MarkdownExtension.Nofollowlinks:
                                pipeline.UseReferralLinks("nofollow");
                                break;
                            case MarkdownExtension.Noopenerlinks:
                                pipeline.UseReferralLinks("noopener");
                                break;
                            case MarkdownExtension.Noreferrerlinks:
                                pipeline.UseReferralLinks("noreferrer");
                                break;
                            case MarkdownExtension.Nohtml:
                                pipeline.DisableHtml();
                                break;
                            case MarkdownExtension.Yaml:
                                pipeline.UseYamlFrontMatter();
                                break;
                            case MarkdownExtension.NonasciiNoescape:
                                pipeline.UseNonAsciiNoEscape();
                                break;
                            case MarkdownExtension.Autolinks:
                                pipeline.UseAutoLinks();
                                break;
                            case MarkdownExtension.Globalization:
                                pipeline.UseGlobalization();
                                break;
                        }
                    }
                }
                return pipeline;
            });
        }
    }
}

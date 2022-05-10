using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Gentings.Documents.Markdown.Extensions.QuoteSectionNotes
{
    /// <summary>
    /// 引用节点通知呈现类。
    /// </summary>
    public class QuoteSectionNoteRender : HtmlObjectRenderer<QuoteSectionNoteBlock>
    {
        /// <summary>
        /// 输出HTML代码。
        /// </summary>
        /// <param name="renderer">HTML呈现实例。</param>
        /// <param name="obj">当前节点通知代码块实例。</param>
        protected override void Write(HtmlRenderer renderer, QuoteSectionNoteBlock obj)
        {
            renderer.EnsureLine();
            switch (obj.QuoteType)
            {
                case QuoteSectionNoteType.MarkdownQuote:
                    WriteQuote(renderer, obj);
                    break;
                case QuoteSectionNoteType.DFMSection:
                    WriteSection(renderer, obj);
                    break;
                case QuoteSectionNoteType.DFMNote:
                    WriteNote(renderer, obj);
                    break;
                case QuoteSectionNoteType.DFMVideo:
                    WriteVideo(renderer, obj);
                    break;
                default:
                    break;
            }
        }

        private void WriteNote(HtmlRenderer renderer, QuoteSectionNoteBlock obj)
        {
            var syntax = SectionNoteType.GetSyntax($"[!{obj.NoteTypeString}]");
            if (syntax == null) return;
            var noteHeading = $"<h5><i class=\"{syntax.Icon}\"></i> {syntax.Text}</h5>";
            renderer.Write("<div").Write($" class=\"alert alert-{syntax.AlertClass}\"").WriteAttributes(obj).WriteLine(">");
            var savedImplicitParagraph = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = false;
            renderer.WriteLine(noteHeading);
            renderer.WriteChildren(obj);
            renderer.ImplicitParagraph = savedImplicitParagraph;
            renderer.WriteLine("</div>");
        }

        private void WriteSection(HtmlRenderer renderer, QuoteSectionNoteBlock obj)
        {
            string attribute = string.IsNullOrEmpty(obj.SectionAttributeString) ?
                        string.Empty :
                        $" {obj.SectionAttributeString}";
            renderer.Write("<div").Write(attribute).WriteAttributes(obj).WriteLine(">");
            var savedImplicitParagraph = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = false;
            renderer.WriteChildren(obj);
            renderer.ImplicitParagraph = savedImplicitParagraph;
            renderer.WriteLine("</div>");
        }

        private void WriteQuote(HtmlRenderer renderer, QuoteSectionNoteBlock obj)
        {
            renderer.Write("<blockquote").WriteAttributes(obj).WriteLine(">");
            var savedImplicitParagraph = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = false;
            renderer.WriteChildren(obj);
            renderer.ImplicitParagraph = savedImplicitParagraph;
            renderer.WriteLine("</blockquote>");
        }

        private void WriteVideo(HtmlRenderer renderer, QuoteSectionNoteBlock obj)
        {
            var modifiedLink = string.Empty;

            if (!string.IsNullOrWhiteSpace(obj?.VideoLink))
            {
                modifiedLink = FixUpLink(obj.VideoLink);
            }

            renderer.Write("<div class=\"embeddedvideo\"").WriteAttributes(obj!).Write(">");
            renderer.Write($"<iframe src=\"{modifiedLink}\" frameborder=\"0\" allowfullscreen=\"true\"></iframe>");
            renderer.WriteLine("</div>");
        }

        public static string FixUpLink(string link)
        {
            if (!link.Contains("https"))
            {
                link = link.Replace("http", "https");
            }
            if (Uri.TryCreate(link, UriKind.Absolute, out var videoLink))
            {
                var host = videoLink.Host;
                var query = videoLink.Query;
                if (query.Length > 1)
                {
                    query = query[1..];
                }

                if (host.Equals("channel9.msdn.com", StringComparison.OrdinalIgnoreCase))
                {
                    // case 1, Channel 9 video, need to add query string param
                    if (string.IsNullOrWhiteSpace(query))
                    {
                        query = "nocookie=true";
                    }
                    else
                    {
                        query = query + "&nocookie=true";
                    }
                }
                else if (host.Equals("youtube.com", StringComparison.OrdinalIgnoreCase) || host.Equals("www.youtube.com", StringComparison.OrdinalIgnoreCase))
                {
                    // case 2, YouTube video
                    host = "www.youtube-nocookie.com";
                }

                var builder = new UriBuilder(videoLink) { Host = host, Query = query };
                link = builder.Uri.ToString();
            }

            return link;
        }
    }
}

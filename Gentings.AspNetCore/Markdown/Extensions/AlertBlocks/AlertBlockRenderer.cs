using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Gentings.AspNetCore.Markdown.Extensions.AlertBlocks
{
    /// <summary>
    /// Alert呈现实现类。
    /// </summary>
    public class AlertBlockRenderer : HtmlObjectRenderer<AlertBlock>
    {
        protected override void Write(HtmlRenderer renderer, AlertBlock alert)
        {
            renderer.EnsureLine();
            var attributes = alert.TryGetAttributes() ?? new HtmlAttributes();
            attributes.AddClass("alert");
            attributes.AddClass("alert-" + alert.Syntax.AlertClass);
            renderer.Write("<div").WriteAttributes(attributes).WriteLine(">");
            renderer.Write("<h5><i class=\"").Write(alert.Syntax.Icon).Write("\"></i> ").Write(alert.Syntax.Text).WriteLine("</h5>");
            var savedImplicitParagraph = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = false;
            renderer.WriteChildren(alert);
            renderer.ImplicitParagraph = savedImplicitParagraph;
            renderer.WriteLine("</div>");
            renderer.EnsureLine();
        }
    }
}

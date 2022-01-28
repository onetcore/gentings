//using Gentings.AspNetCore.TagHelpers;
//using Gentings.Extensions.Sites.Sections;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Razor.TagHelpers;

//namespace Gentings.Extensions.Sites.TagHelpers
//{
//    /// <summary>
//    /// 呈现自定义的节点。
//    /// </summary>
//    [HtmlTargetElement("gt:sections")]
//    public class SectionsTagHelper : PageTagHelperBase
//    {
//        /// <summary>
//        /// 访问并呈现当前标签实例。
//        /// </summary>
//        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
//        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
//        public override async void Process(TagHelperContext context, TagHelperOutput output)
//        {
//            if (Context == null)
//            {
//                output.SuppressOutput();
//                return;
//            }

//            output.TagName = null;
//            var sectionManager = GetRequiredService<ISectionManager>();
//            foreach (var section in Context.Sections)
//            {
//                var builder = new TagBuilder(section.TagName ?? "section");
//                var isFluid = section.IsFluid ?? Context.Page.IsFluid ?? Context.Settings.IsFluid;
//                if (section.Name != null)
//                    builder.AddCssClass(section.Name);
//                if (isFluid == true)
//                    builder.AddCssClass("container-fluid");
//                else if (isFluid == false)
//                    builder.AddCssClass("container");
//                var sctx = new SectionContext(section, Context, ViewContext, builder);
//                var sectionType = sectionManager.GetSectionRender(section.SectionType);
//                await sectionType.ProcessAsync(sctx, output);
//                output.PostElement.AppendHtml(builder);
//            }
//        }
//    }

//    /// <summary>
//    /// 呈现自定义的节点。
//    /// </summary>
//    [HtmlTargetElement("gt:section", Attributes = AttributeName)]
//    public class SectionTagHelper : PageTagHelperBase
//    {
//        /// <summary>
//        /// 初始化类<see cref="SectionTagHelper"/>。
//        /// </summary>
//        /// <param name="sectionManager">节点管理接口实例。</param>
//        public SectionTagHelper(ISectionManager sectionManager)
//        {
//            _sectionManager = sectionManager;
//        }

//        private const string AttributeName = "data";
//        private readonly ISectionManager _sectionManager;

//        /// <summary>
//        /// 节点实例。
//        /// </summary>
//        [HtmlAttributeName(AttributeName)]
//        public Section? Section { get; set; }

//        /// <summary>
//        /// 访问并呈现当前标签实例。
//        /// </summary>
//        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
//        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
//        public override async void Process(TagHelperContext context, TagHelperOutput output)
//        {
//            if (Context == null || Section == null)
//                return;

//            var builder = new TagBuilder(Section.TagName ?? "section");
//            var isFluid = Section.IsFluid ?? Context.Page.IsFluid ?? Context.Settings.IsFluid;
//            if (Section.Name != null)
//                builder.AddCssClass(Section.Name);
//            if (isFluid == true)
//                builder.AddCssClass("container-fluid");
//            else if (isFluid == false)
//                builder.AddCssClass("container");
//            builder.GenerateId(Section.UniqueId, "");
//            var sctx = new SectionContext(Section, Context, ViewContext, builder);
//            var sectionType = _sectionManager.GetSectionRender(Section.SectionType);
//            await sectionType.ProcessAsync(sctx, output);
//            output.Render(builder);
//        }
//    }
//}

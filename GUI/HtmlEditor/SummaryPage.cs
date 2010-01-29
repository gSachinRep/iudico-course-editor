using System.Text;
using System.Web;
using HtmlWriterAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlWriterTag = System.Web.UI.HtmlTextWriterTag;


namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Course.Manifest;
    using Common;
    /// <summary>
    /// Represents the summary page of course
    /// </summary>
    public class SummaryPage : HtmlPageBase
    {
        public SummaryPage(ItemType item)
            : base(item)
        {          
            PageItem.Sequencing = new SequencingType {objectives = new ObjectivesType()};
        }

        public override void Store()
        {                  
            using (var w = new FFHtmlWriter(PageItem))
            {
                w.RenderBeginTag(HtmlWriterTag.Html);

                w.RenderBeginTag(HtmlWriterTag.Head);
                WriteIncludedScripts(w);
                w.RenderEndTag();

                w.AddAttribute("onload", "doInitialize();");
                w.AddAttribute("onunload", "doTerminate();");

                w.RenderBeginTag(HtmlWriterTag.Body);
                var initScript = new StringBuilder("resetPoints();");
                var hasQuestions = false;
                {
                    w.AddAttribute(HtmlWriterAttribute.Border, "1");
                    w.AddAttribute(HtmlWriterAttribute.Width, "100%");
                    w.RenderBeginTag(HtmlWriterTag.Table);

                    w.RenderBeginTag(HtmlWriterTag.Tr);
                    w.RenderBeginTag(HtmlWriterTag.Th);
                    w.Write("Page:");
                    w.RenderEndTag();

                    w.RenderBeginTag(HtmlWriterTag.Th);
                    w.Write("Score:");
                    w.RenderEndTag();
                    w.RenderEndTag();

                    PageItem.Sequencing.objectives.objective.Clear();
                    PageItem.Sequencing.objectives.primaryObjective = new ObjectivesTypePrimaryObjective("PRIMARYOBJ");

                    foreach (var item in Course.Course.Organization.Items)
                    {
                        if (item.PageType == PageType.Question)
                        {
                            hasQuestions = true;
                            var obj = item.Sequencing.objectives = new ObjectivesType { primaryObjective = new ObjectivesTypePrimaryObjective(item.Identifier, true) };
                            obj.primaryObjective.mapInfo.Add(ObjectiveMappingType.CreateForQuestion(item.Identifier));
                            PageItem.Sequencing.objectives.objective.Add(ObjectivesTypeObjective.CreateGlobalObj(item.Identifier));
                            w.RenderBeginTag(HtmlWriterTag.Tr);
                            {
                                w.RenderBeginTag(HtmlWriterTag.Td);
                                w.Write(HttpUtility.HtmlEncode(item.Title));
                                w.RenderEndTag();

                                w.AddAttribute(HtmlWriterAttribute.Id, item.Identifier);
                                w.RenderBeginTag(HtmlWriterTag.Td);
                                w.RenderEndTag();
                            }
                            w.RenderEndTag();
                            int? passRank;
                            var total = item.GetTotalPoints(out passRank);
                            initScript.AppendFormat("updateSummaryPageItem('{0}',{1},{2});", item.Identifier, total, passRank.ToString().IsNull("null"));
                        }
                    }
                    w.RenderEndTag();
                }
                w.Write("<br/>");
                w.AddAttribute(HtmlWriterAttribute.Id, "totalSummary");
                w.RenderBeginTag(HtmlWriterTag.Div);
                w.RenderEndTag();
                initScript.Append("document.getElementById('totalSummary').innerText=getTotalMessage();");

                WriteTraceLogElement(w);

                w.RenderBeginTag(HtmlWriterTag.Script);
                if (hasQuestions)
                {
                    w.Write(initScript);
                }
                w.RenderEndTag();
                w.RenderEndTag();
                w.RenderEndTag();
            }
        }
    }
}
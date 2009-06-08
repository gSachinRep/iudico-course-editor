using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlStyleAttribute = System.Web.UI.HtmlTextWriterStyle;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System.Windows.Forms;
    using System.Xml;

    ///<summary>
    /// Represents formatted text in Examination designer
    ///</summary>
    [HtmlSerializeSettings(SerializeElems.Size | SerializeElems.Position)]
    public class HtmlCodeSnippet : HtmlDesignMovableControl
    {
        public override void WriteHtml(HtmlWriter w)
        {
            base.WriteHtml(w);
            HtmlSerializeHelper<HtmlCodeSnippet>.WriteRootElementAttributes(w, this);
            w.AddAttribute(HtmlAttribute.Name, "snippet");
            w.AddStyleAttribute(HtmlStyleAttribute.Overflow, "auto");
            w.RenderBeginTag(HtmlTag.Div);
            w.Write(((CodeSnippet)Control).HtmlCode);
            w.RenderEndTag();
        }

        protected override Control CreateWindowControl()
        {
            return new CodeSnippet();
        }

        protected override void Parse(XmlNode node)
        {
            base.Parse(node);
            HtmlSerializeHelper<HtmlCodeSnippet>.ReadRootElementAttributes(node, this);
            ((CodeSnippet)Control).HtmlCode = node.InnerXml;
        }
    }
}
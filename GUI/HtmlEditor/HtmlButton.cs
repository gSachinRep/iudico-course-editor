using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System.Windows.Forms;
    using System.Xml;

    ///<summary>
    /// Represents Submit button in Examination designer
    ///</summary>
    [HtmlSerializeSettings(SerializeElems.ALL)]
    public class HtmlButton : HtmlDesignMovableControl
    {
        protected override Control CreateWindowControl()
        {
            return new Button { Text = "Submit" };
        }

        public override void WriteHtml(HtmlWriter w)
        {
            base.WriteHtml(w);
            HtmlSerializeHelper<HtmlButton>.WriteRootElementAttributes(w, this);
            w.AddAttribute(HtmlAttribute.Type, "button");
            w.AddAttribute(HtmlAttribute.Value, HtmlUtility.QuotesEncode(Control.Text));
            w.AddAttribute(HtmlAttribute.Onclick, "scoObj.Commit()");
            w.RenderBeginTag(HtmlTag.Input);
            w.RenderEndTag();
        }

        protected override void Parse(XmlNode node)
        {
            base.Parse(node);
            HtmlSerializeHelper<HtmlButton>.ReadRootElementAttributes(node, this);
            Control.Text = HtmlUtility.QuotesDecode(node.Attributes["value"].Value);
        }
    }
}
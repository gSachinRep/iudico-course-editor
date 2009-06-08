using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System.Web;
    using System.Windows.Forms;
    using System.Xml;

    [HtmlSerializeSettings(SerializeElems.Position)]
    public class HtmlSimpleQuestion : HtmlTestControl
    {
        private const string CorrectCombinationIsNotDefined = "Correct combination is not defined";

        protected new SimpleQuestion Control;

        public override string CorrectAnswer
        {
            get { return Control.Answer; }
            set
            {
                Control.Answer = value;
                ReValidate();
            }
        }

        public override void WriteHtml(HtmlWriter w)
        {
            base.WriteHtml(w);
            HtmlSerializeHelper<HtmlSimpleQuestion>.WriteRootElementAttributes(w, this);

            w.AddAttribute(HtmlAttribute.Name, Control.SingleCase ? "gen:single" : "gen:multy");
            w.RenderBeginTag(HtmlTag.Div);
            w.RenderBeginTag(HtmlTag.P);
            w.Write(HttpUtility.HtmlEncode(Control.Question));
            w.RenderEndTag();

            foreach (var tb in Control.textBoxesList)
            {
                w.AddAttribute(HtmlAttribute.Type, Control.SingleCase ? "radio" : "checkbox");
                if (Control.SingleCase)
                    w.AddAttribute(HtmlAttribute.Name, Name);
                w.RenderBeginTag(HtmlTag.Input);
                w.RenderEndTag();

                w.RenderBeginTag(HtmlTag.Span);
                w.Write(HttpUtility.HtmlEncode(tb.Text));
                w.RenderEndTag();

                w.RenderBeginTag(HtmlTag.Br);
                w.RenderEndTag();
            }
            w.RenderEndTag();
        }

        public override string GetScoTestInitializer()
        {
            return string.Format("new complexTest('{0}')", Name);
        }

        protected override Control CreateWindowControl()
        {
            Control = new SimpleQuestion();
            Control.SingleCaseChanged += ReValidate;
            return Control;
        }

        protected override void Parse(XmlNode node)
        {
            base.Parse(node);
            HtmlSerializeHelper<HtmlSimpleQuestion>.ReadRootElementAttributes(node, this);

            Control.SingleCase = node.Attributes["name"].Value.EndsWith("single");
            Control.Question = node.SelectSingleNode("p").InnerText;
            var spans = node.SelectNodes("span");
            Control.EnsureCount(spans.Count);
            var i = 0;
            foreach (XmlNode s in spans)
            {
                Control.textBoxesList[i++].Text = s.InnerText;
            }
        }

        protected override void InternalValidate()
        {
            base.InternalValidate();
            if (Control.SingleCase && !CorrectAnswer.Contains("1"))
            {
                AddError(CorrectCombinationIsNotDefined);
            }
        }
    }
}
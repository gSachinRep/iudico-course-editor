using System.Text.RegularExpressions;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Xml;

    [HtmlSerializeSettings(SerializeElems.ALL)]
    public class HtmlTextBox : HtmlTestControl, IJavaScriptInitializable
    {
        private const string CORRECT_ANSWER_PROPERTY = "Correct answer",
            CORRECT_ANSWER_PROPERTY_ERROR_TEXT = "'" + CORRECT_ANSWER_PROPERTY + "' property must be specified";

        [Category("Data")]
        [DisplayName(CORRECT_ANSWER_PROPERTY)]
        [Browsable(true)]
        [CanBeNull]
        public override string CorrectAnswer
        {
            get { return _CorrectAnswer; }
            set
            {
                if (!_CorrectAnswer.IsEqual(value))
                {
                    _CorrectAnswer = value;
                    ReValidate();
                }
            }
        }

        [Category("Data")]
        [Description("Gets or sets text that will be displayed in TextBox in italic style while user is typed nothing")]
        [DisplayName("Empty text")]
        [CanBeNull]
        public string EmptyText
        {
            get { return _EmptyText; }
            set { _EmptyText = value; }
        }

        public string GetJavaScriptInitializer()
        {
            return EmptyText.IsNotNull() ? string.Format("textBoxInit(document.all['{0}'], '{1}');", Name, EmptyText) : null;
        }

        public override void WriteHtml([NotNull]HtmlWriter w)
        {
            base.WriteHtml(w);
            HtmlSerializeHelper<HtmlTextBox>.WriteRootElementAttributes(w, this);
            if (Control.Text.IsNotNull())
            {
                w.AddAttribute(HtmlAttribute.Value, HtmlUtility.QuotesEncode(Control.Text));
            }
            if (EmptyText.IsNotNull())
            {
                w.AddAttribute("onfocus", "textBoxFocus(this)");
                w.AddAttribute("onblur", "textBoxBlur(this)");
            }

            w.AddAttribute(HtmlAttribute.Type, "text");
            w.RenderBeginTag(HtmlTag.Input);
            w.RenderEndTag();
        }

        public override string GetScoTestInitializer()
        {
            return string.Format("new simpleTest('{0}', '{1}')", Name, CorrectAnswer);
        }

        protected override void Parse([NotNull]XmlNode node)
        {
            base.Parse(node);
            HtmlSerializeHelper<HtmlTextBox>.ReadRootElementAttributes(node, this);
            var at = node.Attributes["value"];
            Control.Text = HtmlUtility.QuotesDecode(at != null ? at.Value : node.InnerText);
        }

        protected override Control CreateWindowControl()
        {
            return new TextBox();
        }

        protected override void InternalValidate()
        {
            base.InternalValidate();
            if (CorrectAnswer.IsNull())
            {
                AddError(CORRECT_ANSWER_PROPERTY, CORRECT_ANSWER_PROPERTY_ERROR_TEXT);
            }
        }

        public static Regex TextBoxInit;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _CorrectAnswer, _EmptyText;
    }
}
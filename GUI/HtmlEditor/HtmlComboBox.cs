using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Xml;
    using Common;

    ///<summary>
    /// Represents Drop-Down elements in Html Editor
    ///</summary>
    [HtmlSerializeSettings(SerializeElems.ALL)]
    [BindProperty("Control", "Items")]
    public class HtmlComboBox : HtmlTestControl
    {
        ///<summary>
        /// Win-Control represent this one
        ///</summary>
        public new ComboBox Control;

        private const string CORRECT_INDEX_PROPERTY = "Correct index",
            CORRECT_INDEX_PROPERTY_ERROR = "'" + CORRECT_INDEX_PROPERTY + "' must be specified",
            CORRECT_INDEX_PROPERTY_ERROR2 = "'" + CORRECT_INDEX_PROPERTY + "' is incorrect",
            CORRECT_INDEX_PROPERTY_ERROR3 = "'" + CORRECT_INDEX_PROPERTY + "' must be positive and less that ComboBox items count";

        ///<summary>
        /// Index of Drop-Down that should be determined as correct
        ///</summary>
        [DefaultValue(0)]
        [DisplayName(CORRECT_INDEX_PROPERTY)]
        [Description("Index of Drop-Down that should be determined as correct")]
        public int? CorrectIndex
        {
            get { return _CorrectIndex; }
            set
            {   
                if ((_CorrectIndex ?? int.MinValue) != (value ?? int.MinValue))
                {
                    if (_CorrectIndex == null && value != null)
                    {
                        RemoveError(CORRECT_INDEX_PROPERTY);
                    }
                    _CorrectIndex = value;
                    if (value == null)
                    {
                        AddError(CORRECT_INDEX_PROPERTY, CORRECT_INDEX_PROPERTY_ERROR);
                    }
                    else
                    {
                        RemoveError(CORRECT_INDEX_PROPERTY_ERROR2);
                        if (value.Value >= Control.Items.Count)
                        {
                            AddError(CORRECT_INDEX_PROPERTY_ERROR2);
                        }
                    }
                }
            }
        }

        [NotNull]
        public override string CorrectAnswer
        {
            get { return CorrectIndex.ToString(); }
            set { CorrectIndex = value.IsNull() ? null : (int?)int.Parse(value); }
        }

        public override void WriteHtml([NotNull]HtmlWriter w)
        {
            base.WriteHtml(w);
            HtmlSerializeHelper<HtmlComboBox>.WriteRootElementAttributes(w, this);
            w.RenderBeginTag(HtmlTag.Select);

            var count = Control.Items.Count;
            if (count > 0)
            {
                w.AddAttribute(HtmlAttribute.Selected, "true");
            }
            for (var i = 0; i < count; i++)
            {
                w.AddAttribute(HtmlAttribute.Value, i.ToString());
                w.RenderBeginTag(HtmlTag.Option);
                w.Write(Control.Items[i].ToString().HttpEncode());
                w.RenderEndTag();
            }

            w.RenderEndTag();
        }

        [NotNull]
        public override string GetScoTestInitializer()
        {
            return string.Format("new simpleTest('{0}', '{1}')", Name, CorrectAnswer);
        }

        [NotNull]
        protected override Control CreateWindowControl()
        {
            return new ComboBox();
        }

        protected override void Parse([NotNull]XmlNode node)
        {
            base.Parse(node);
            HtmlSerializeHelper<HtmlComboBox>.ReadRootElementAttributes(node, this);
            foreach (XmlNode sub in node.ChildNodes)
            {
                Control.Items.Add(sub.InnerText.HttpDecode());
            }
        }

        protected override void InternalValidate()
        {
            base.InternalValidate();
            if (CorrectIndex == null)
            {
                AddError(CORRECT_INDEX_PROPERTY,CORRECT_INDEX_PROPERTY_ERROR);
            }
            else if (CorrectIndex < 0 || CorrectIndex >= Control.Items.Count)
            {
                AddError(CORRECT_INDEX_PROPERTY, CORRECT_INDEX_PROPERTY_ERROR3);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? _CorrectIndex;
    }
}
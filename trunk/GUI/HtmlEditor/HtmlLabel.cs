using System;
using System.Drawing;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    [HtmlSerializeSettings(SerializeElems.Position | SerializeElems.Font)]
    public class HtmlLabel : HtmlDesignMovableControl
    {
        public new Label Control;

        public override void WriteHtml(HtmlWriter w)
        {
            base.WriteHtml(w);
            HtmlSerializeHelper<HtmlLabel>.WriteRootElementAttributes(w, this);
            w.RenderBeginTag(HtmlTag.Span);
            w.Write((Control.Text).HttpEncode().Replace(Environment.NewLine, "<br />"));
            w.RenderEndTag();
        }

        protected override Control CreateWindowControl()
        {
            var l = new Label { Text = "New Label", AutoSize = false };
            l.Font = new Font(l.Font.FontFamily, 10, GraphicsUnit.Point);
            l.Click += (sender, e) => ((Control)sender).Focus();
            l.TextChanged += Control_TextChanged;
            l.Resize += Control_Resized;
            return Control = l;
        }

        protected override void Parse(XmlNode node)
        {
            base.Parse(node);
            HtmlSerializeHelper<HtmlLabel>.ReadRootElementAttributes(node, this);
            Control.Text = node.InnerXml.HttpDecode().Replace("<br />", Environment.NewLine);
        }

        private void Control_Resized(object sender, EventArgs e)
        {
            float hRatio = (float)Control.Size.Height / Control.PreferredHeight;
            float wRatio = (float)Control.Size.Width / Control.PreferredWidth;
            float ratio = wRatio < hRatio ? wRatio : hRatio;
            if (!float.IsInfinity(ratio) && ratio > 0)
            {
                Font f = Control.Font;
                Control.Font = new Font(f.FontFamily, f.Size * ratio, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
            }
        }

        private void Control_TextChanged(object sender, EventArgs e)
        {
            Control.Size = new Size(Control.PreferredWidth, Control.PreferredHeight);
        }
    }
}
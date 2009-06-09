using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlStyleAttribute = System.Web.UI.HtmlTextWriterStyle;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI.HtmlEditor.HighlightControl
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Web;
    using System.Windows.Forms;
    using System.Xml;
    using Course;

    ///<summary>
    ///  Control to show highlighted programming code
    ///</summary>
    [HtmlSerializeSettings(SerializeElems.Position | SerializeElems.Size)]
    public class HtmlHighlightedCode : HtmlDesignMovableControl, IJavaScriptInitializable, IDeleteNotifiable
    {
        /// <summary>
        /// Represents language to highlight
        /// </summary>
        [Flags]
        public enum LANGUAGE
        {
            Axapta = 1,
            Cpp = 2,
            Delphi = 4,
            HTML = 8,
            Java = 16,
            JavaScript = 32,
            Perl = 64,
            PHP = 128,
            Python = 256,
            RIB = 512,
            RSL = 1024,
            Ruby = 2048,
            Smalltalk = 4096,
            SQL = 8192,
            VBScript = 16384
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LANGUAGE? _Language;

        private const string HIGHLIGHT_JS_FILENAME = "highlight.js",
                             SAMPLE_CSS_FILENAME = "sample.css",
                             HIGHLIGHT_DIRECTORY = "HighlightLanguages", LANG_IS_NULL_ERROR = "'Language' property must be specified";

        ///<summary>
        /// Programming language to highlight
        ///</summary>
        [Category("Data")]
        [DisplayName("Language")]
        [DebuggerNonUserCode]
        public LANGUAGE? Language
        {
            get { return _Language; }
            set
            {
                if (_Language != value)
                {
                    if (_Language != null)
                    {
                        ReleaseFileResources();
                    }
                    _Language = value;
                    if (value != null)
                    {
                        RemoveError(LANG_IS_NULL_ERROR);
                        GetFileResources();
                    }
                }
                ReValidate();
            }
        }

        [CanBeNull]
        public string GetJavaScriptInitializer()
        {
            var page = GetParentPage();
            if (Control.Text.IsNotNull() && Language != null && ((page.InitializedLanguages & Language) == 0))
            {
                page.InitializedLanguages |= Language.Value;
                return string.Format("initHighlightingOnLoad('{0}');", (Language.ToString().ToLower()));
            }
            return null;
        }

        public override void WriteHtml([NotNull]HtmlWriter w)
        {
            base.WriteHtml(w);
            HtmlSerializeHelper<HtmlHighlightedCode>.WriteRootElementAttributes(w, this);
            var ls = Language.ToString().ToLower();
            w.AddAttribute(HtmlAttribute.Name, "code");
            w.AddStyleAttribute(HtmlStyleAttribute.Overflow, "auto");
            w.RenderBeginTag(HtmlTag.Span);
            w.AddAttribute(HtmlAttribute.Class, ls);
            w.WriteFullBeginTag(string.Concat("pre><code class=\"", ls, "\""));
            w.Write(Control.Text.HttpEncode());
            w.WriteFullBeginTag("/code></pre");
            w.RenderEndTag();
        }

        protected override void InternalValidate()
        {
            base.InternalValidate();
            if (Language == null)
            {
                AddError(LANG_IS_NULL_ERROR);
            }
        }

        new public void NotifyDelete()
        {
            base.NotifyDelete();
            if (Language != null)
            {
                ReleaseFileResources();
            }
        }

        new public void NotifyUndoDelete()
        {
            base.NotifyUndoDelete();
            if (Language != null)
            {
                GetFileResources();
            }
        }

        [NotNull]
        protected override Control CreateWindowControl()
        {
            return new TextBox
            {
                Multiline = true,
                Size = new Size(200, 130),
                AcceptsTab = true,
                AcceptsReturn = true,
                ScrollBars = ScrollBars.Both
            };
        }

        protected override void Parse([NotNull]XmlNode node)
        {
            base.Parse(node);
            HtmlSerializeHelper<HtmlHighlightedCode>.ReadRootElementAttributes(node, this);
            node = node.SelectSingleNode("pre");
            Control.Text = node.InnerText.HttpDecode();
            node = node.SelectSingleNode("code");
            Language = (LANGUAGE)Enum.Parse(typeof(LANGUAGE), node.Attributes["class"].Value, true);
        }

        private void ReleaseFileResources()
        {
            Course.ReleaseCourseResource(HIGHLIGHT_DIRECTORY, HIGHLIGHT_JS_FILENAME, SAMPLE_CSS_FILENAME, Language.ToString().ToLower() + ".js");
            HtmlPageBase page = GetParentPage();
            page.Scripts.RemoveAllWithRoot(HIGHLIGHT_DIRECTORY, HIGHLIGHT_JS_FILENAME);
            page.Styles.RemoveAllWithRoot(HIGHLIGHT_DIRECTORY, SAMPLE_CSS_FILENAME);
        }

        private void GetFileResources()
        {
            HtmlPageBase page = GetParentPage();
            page.Scripts.IncludeAllWithRoot(HIGHLIGHT_DIRECTORY, HIGHLIGHT_JS_FILENAME);
            page.Styles.IncludeAllWithRoot(HIGHLIGHT_DIRECTORY, SAMPLE_CSS_FILENAME);
            Course.EnsureCourseContainsResources(Course.LANGUAGES_NAMESPACE, HIGHLIGHT_DIRECTORY, HIGHLIGHT_JS_FILENAME, SAMPLE_CSS_FILENAME, Language.ToString().ToLower() + ".js");
        }
    }
}
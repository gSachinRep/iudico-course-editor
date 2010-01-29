using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using HtmlWriterTag = System.Web.UI.HtmlTextWriterTag;
using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlWriterAttribute = System.Web.UI.HtmlTextWriterAttribute;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Course;
    using Common;
    using Course.Manifest;

    /// <summary>
    /// Represents html page which can be edited using <see cref="PageEditor"/>
    /// </summary>
    public partial class HtmlPage : HtmlPageBase, IJavaScriptInitializable
    {
        private const string PASS_RANK_PROPERTY = "Pass rank",
                             PASS_RANK_PROPERTY_ERROR = "'" + PASS_RANK_PROPERTY + "' must be specified",
                             PASS_RANK_PROPERTY_ERROR2 = "'" + PASS_RANK_PROPERTY + "' must be a positive number",
                             PASS_RANK_IS_MORE_THAN_SUM_OF_RANKS = "'Pass Rank' is more then sum of Ranks of all exam controls",
                             NO_BUTTONS_ERROR = "No buttons is present on examination",
                             MORE_THEN_ONE_BUTTON = "Too many submit buttons present";

        private int? _PassRank = 1;

        ///<summary>
        /// Determines how many points should take user to pass this test
        ///</summary>
        [DisplayName(PASS_RANK_PROPERTY)]
        [Description("Determines how many points should take user to pass this test")]
        [DebuggerNonUserCode]
        public int? PassRank
        {                                 
            get { return _PassRank; }
            set
            {
                if (_PassRank != value)
                {
                    _PassRank = value;
                    ReValidate();
                }
            }
        }

        ///<summary>
        /// Gets or sets Win-Control parent of page
        ///</summary>
        [DebuggerNonUserCode]
        [CanBeNull]
        new public Control Parent
        {
            get
            {
                return IsWinControlCreated ? Control.Parent : null;
            }
            set
            {
                if (!IsWinControlCreated && value != null)
                {
                    ReCreateControl();
                }
                if (IsWinControlCreated && (Control.Parent = value) != null)
                {
                    ReValidate();
                }
            }
        }

        ///<summary>
        /// Gets page assigned to <see cref="item" />
        ///</summary>
        ///<param name="item">Item which assigned to page we need to find</param>
        ///<returns></returns>
        new public static HtmlPage GetPage(ItemType item)
        {
            HtmlPage resultPage = (HtmlPage)HtmlPageBase.GetPage(item);
            if (resultPage != null)
            {
                resultPage._PassRank = 1;
            }
            return resultPage;
        }

        public string GetJavaScriptInitializer()
        {
            InitializedLanguages = 0;
            var scripts = new StringBuilder();
            var result = new StringBuilder(@"scoObj=new SCOObj(");
            result.Append(PassRank.ToString().IsNull("null"));
            
            foreach (var hc in HtmlControls)
            {
                var tc = hc as HtmlTestControl;
                if (tc != null)
                {
                    result.Append(",");
                    result.Append(tc.GetScoTestInitializer());
                }
                var initializable = hc as IJavaScriptInitializable;
                if (initializable != null)
                {
                    var intS = initializable.GetJavaScriptInitializer();
                    if (!string.IsNullOrEmpty(intS))
                    {
                        scripts.Append(intS);
                        if (!intS.EndsWith(";"))
                        {
                            scripts.Append(';');
                        }
                    }
                }
            }
            result.Append(");");
            result.Append(scripts);
            return result.ToString();
        }


        ///<summary>
        /// Writes head html
        ///</summary>
        ///<param name="w"></param>
        public void WriteHeadText(HtmlWriter w)
        {

            WriteHeadMetaCharset(w);
            WriteIncludedScripts(w);
            WriteIncludedStyles(w);
        }

        /// <summary>
        /// Writes charset type into meta-tag
        /// </summary>
        /// <param name="w"></param>
        public void WriteHeadMetaCharset(HtmlWriter w)
        {
            // meta-tag
            w.AddAttribute("http-equiv", "Content-Type");
            w.AddAttribute("content", "text/html; charset=" + w.Encoding.HeaderName);

            w.RenderBeginTag(HtmlWriterTag.Meta);
            w.RenderEndTag();
            //
        }

        protected override void InternalValidate()
        {
            base.InternalValidate();
            if (_PassRank == null)
            {
                AddError(PASS_RANK_PROPERTY, PASS_RANK_PROPERTY_ERROR);
            }
            else if (_PassRank <= 0)
            {
                AddError(PASS_RANK_PROPERTY_ERROR2);
            }
            else if (HasPageItem)
            {
                int? passRank;
                int p = PageItem.GetTotalPoints(out passRank);
                if (passRank != null && p < passRank)
                {
                    AddError(PASS_RANK_IS_MORE_THAN_SUM_OF_RANKS);
                }
            }

            int buttonsCount = 0;
            foreach (Control c in Control.Controls)
            {
                if (c.Tag is HtmlButton)
                {
                    buttonsCount++;
                }
            }
            if (buttonsCount != 1)
            {
                if (buttonsCount == 0)
                {
                    AddError(NO_BUTTONS_ERROR);
                }
                else
                {
                    AddError(MORE_THEN_ONE_BUTTON);
                }
            }
        }

        ///<summary>
        /// Get list of questions of page
        ///</summary>
        ///<returns></returns>
        [NotNull]
        public IList<Question> GetQuestions()
        {
            var result = new List<Question>();
            foreach (var hc in HtmlControls)
            {
                var tc = hc as HtmlTestControl;
                if (tc != null)
                {
                    result.Add(tc.StoreAnswersItem());
                }
            }
            return result;
        }

        public override void Store()
        {
            using (var w = new FFHtmlWriter(PageItem))
            {
                w.RenderBeginTag(HtmlWriterTag.Html);
                w.RenderBeginTag(HtmlWriterTag.Head);
                WriteHeadText(w);
                w.RenderEndTag();

                w.RenderBeginTag(HtmlWriterTag.Body);
                WriteInnerHtml(w);
                WriteTraceLogElement(w);

                var scripts = GetJavaScriptInitializer();
                if (scripts.IsNotNull())
                {
                    w.RenderBeginTag(HtmlWriterTag.Script);
                    w.Write(scripts);
                    w.RenderEndTag();
                }

                w.RenderEndTag();
                w.RenderEndTag();
                Course.Answers.Update(PageItem.Identifier, PassRank, GetQuestions());
            }
        }

        private void SetQuestions([NotNull]IList<Question> qs)
        {
            var index = 0;
            foreach (var hc in HtmlControls)
            {
                var t = hc as HtmlTestControl;
                if (t != null)
                {
                    var q = qs[index++];
                    t.ReadAnswerItem(q);
                }
            }
            if (index != qs.Count)
            {
                throw new FireFlyException("Incorrect answers file: Count of test controls and answers is not the same");
            }
        }

        protected override void OnParsed()
        {
            base.OnParsed();
            var org = Course.Answers.Organizations[Course.Organization.identifier];
            var item = org.Items[PageItem.Identifier];
            PassRank = item.PassRank;
            SetQuestions(item.Questions);
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var m in _RedoStack)
            {
                m.Dispose();
            }
            _RedoStack.Clear();
            foreach (var m in _UndoStack)
            {
                m.Dispose();
            }
            _UndoStack.Clear();
        }
    }
}
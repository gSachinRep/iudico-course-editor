using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml;
using Control=System.Windows.Forms.Control;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Common;
    using Course;

    ///<summary>
    /// Represent Exam item that allow user to type programming code to run
    ///</summary>
    [HtmlSerializeSettings(SerializeElems.ALL)]
    public class HtmlCompiledTest : HtmlTestControl
    {
        private const string LANGUAGE_IS_NOT_SPECIFIED_ERROR = "Language is not specified";

        [Browsable(false)]
        public override string CorrectAnswer
        {
            get
            {
                 throw new NotSupportedException();
            }
            set
            {
                 throw new NotSupportedException();
            }
        }

        ///<summary>
        /// Maximum size of memory that program can use in Kb
        ///</summary>
        [Category("Data")]
        [DisplayName("Memory Limit")]
        [Description("Maximum size of memory that program can use in Kb")]
        public decimal MemoryLimit
        {
            get {   return _MemoryLimit / (decimal)1024; }
            set { _MemoryLimit = (long)Math.Round(1024 * value); }
        }

        ///<summary>
        /// Maximum time period that program can use in seconds
        ///</summary>
        [Category("Data")]
        [DisplayName("Time Limit")]
        [Description("Maximum time period that program can use in seconds")]
        public decimal TimeLimit
        {
            get
            {
                 return _TimeLimit / (decimal)1000;
            }
            set { _TimeLimit = (long)Math.Round(1000 * value); }
        }

        ///<summary>
        /// Limit of memory size that program can use in Kb
        ///</summary>
        [Category("Data")]
        [DisplayName("Output Limit")]
        [Description("Limit of memory size that program can use in Kb")]
        public decimal OutputLimit
        {
            get { return _OutputLimit / (decimal)1024; }
            set { _OutputLimit = (long)Math.Round(1024 * value); }
        }

        ///<summary>
        /// Programming language that should be used for implementation
        ///</summary>
        [Category("Data")]
        [Description("Programming language that should be used for implementation")]
        public CompiledQuestion.LANGUAGE? Language
        {
            get { return _Language; }
            set
            {
                _Language = value;
                ReValidate();
            }                   
        }

        ///<summary>
        /// Set of cases should be used to check user code
        ///</summary>
        [Category("Data")]
        [Editor(typeof(TestCasesEditor), typeof(UITypeEditor))]
        [DisplayName("Test Cases")]
        [Description("Set of cases should be used to check user code")]
        public List<CompiledTestCase> TestCases
        {
            get
            {
                if (_TestCases == null)
                {
                    _TestCases = new List<CompiledTestCase>();
                }
                 return _TestCases;
            }
            set { _TestCases = value; }
        }

        public override Question StoreAnswersItem()
        {
            return new CompiledQuestion(Rank, _MemoryLimit, _OutputLimit, _TimeLimit, Language) { Tests = TestCases };
        }

        public override void ReadAnswerItem(Question q)
        {
            var cq = (CompiledQuestion) q;
            Rank = cq.Rank;
            MemoryLimit = cq.MemoryLimit;
            TimeLimit = cq.TimeLimit;
            OutputLimit = cq.OutputLimit;
            Language = cq.Language;
            TestCases = cq.Tests;
        }

        protected override void InternalValidate()
        {
            base.InternalValidate();
            if (Language == null)
            {
                AddError(LANGUAGE_IS_NOT_SPECIFIED_ERROR);
            }
        }

        public override void WriteHtml(HtmlTextWriter w)
        {
            base.WriteHtml(w);
            HtmlSerializeHelper<HtmlCompiledTest>.WriteRootElementAttributes(w, this);
            w.RenderBeginTag(HtmlTextWriterTag.Textarea);
            if (Control.Text.IsNotNull())
            {
                w.Write(Control.Text);
            }
            w.RenderEndTag();
        }

        public override string GetScoTestInitializer()
        {
            return string.Format("new simpleTest('{0}')", Name);
        }

        protected override Control CreateWindowControl()
        {
            return new TextBox
            {
                Multiline = true, 
                Size = new Size(300, 250), 
                ScrollBars = ScrollBars.Both
            };
        }

        protected override void Parse(XmlNode node)
        {
            base.Parse(node);
            HtmlSerializeHelper<HtmlCompiledTest>.ReadRootElementAttributes(node, this);
            Control.Text = node.InnerText;
        }

        private long _MemoryLimit;
        private long _TimeLimit;
        private long _OutputLimit;
        private List<CompiledTestCase> _TestCases;
        private CompiledQuestion.LANGUAGE? _Language;
                                        
    }
}

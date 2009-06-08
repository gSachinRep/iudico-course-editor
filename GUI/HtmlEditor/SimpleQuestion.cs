using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlStyleAttribute = System.Web.UI.HtmlTextWriterStyle;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    public partial class SimpleQuestion : UserControl
    {
        private readonly List<Control> controlsList = new List<Control>();

        public readonly List<TextBox> textBoxesList = new List<TextBox>();

        public SimpleQuestion()
        {
            InitializeComponent();
        }

        public bool SingleCase
        {
            get { return singleCase.Checked; }
            set { singleCase.Checked = value; }
        }

        public string Answer
        {
            get
            {
                var answer = new StringBuilder();

                foreach (var c in controlsList)
                {
                    var check = c is RadioButton ? (c as RadioButton).Checked : ((CheckBox)c).Checked;
                    answer.Append(check ? "1" : "0");
                }
                return answer.ToString();
            }
            set
            {
                controlsCount.Value = value.Length;
                if (SingleCase)
                {
                    for (var i = 0; i < value.Length; i++)
                    {
                        ((RadioButton)controlsList[i]).Checked = value[i] == '1';
                    }
                }
                else
                {
                    for (var i = 0; i < value.Length; i++)
                    {
                        ((CheckBox)controlsList[i]).Checked = value[i] == '1';
                    }
                }
            }
        }

        public string Question
        {
            get { return QuestionText.Text; }
            set { QuestionText.Text = value; }
        }

        public List<string> ControlsText
        {
            set
            {
                var controlsText = value;
                for (int i = 0; i < controlsText.Count; i++)
                {
                    AddControl();
                    textBoxesList[i].Text = controlsText[i];
                }
            }
        }

        public event Action SingleCaseChanged;

        private void DeleteControl()
        {
            if (controlsList.Count != 0)
            {
                Controls.Remove(controlsList[controlsList.Count - 1]);
                Controls.Remove(textBoxesList[textBoxesList.Count - 1]);

                controlsList.Remove(controlsList[controlsList.Count - 1]);
                textBoxesList.Remove(textBoxesList[textBoxesList.Count - 1]);
            }
        }

        private void UpdateControls()
        {
            for (var i = 0; i < controlsList.Count; i++)
            {
                var control = createControl();
                control.Location = controlsList[i].Location;
                control.Size = controlsList[i].Size;
                control.Anchor = controlsList[i].Anchor;

                Controls.Remove(controlsList[i]);
                controlsList[i] = control;
                Controls.Add(controlsList[i]);
            }
        }

        private Control createControl()
        {
            if (SingleCase)
            {
                return new RadioButton();
            }
            else
            {
                return new CheckBox();
            }
        }

        private void AddControl()
        {
            var control = createControl();
            var answerText = new TextBox();
            control.Location = (new Point(6, (controlPanel.Location.Y + 35) + (controlsList.Count*25)));
            control.Size = new Size(14, 13);
            control.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;

            answerText.Location = new Point(control.Location.X + 20, control.Location.Y - 3);
            answerText.Size = new Size(Size.Width - 40, 20);
            answerText.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            controlsList.Add(control);
            textBoxesList.Add(answerText);
            Controls.Add(answerText);
            Controls.Add(control);
        }

        public void EnsureCount(int count)
        {
            while (controlsList.Count < count)
            {
                AddControl();
            }
            while (controlsList.Count > count)
            {
                DeleteControl();
            }
        }

        public void WriteHtml(HtmlWriter w, string name)
        {
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            OnGotFocus(EventArgs.Empty); // To select control in editor
        }

        private void singleCase_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
            if (SingleCaseChanged != null)
            {
                SingleCaseChanged();
            }
        }

        private void controlsCount_ValueChanged(object sender, EventArgs e)
        {
            EnsureCount((int) controlsCount.Value);
        }
    }
}
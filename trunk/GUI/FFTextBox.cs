using System;
using System.Windows.Forms;

namespace FireFly.CourseEditor.GUI
{
    public class FFTextBox : TextBox
    {
        public bool WasChanged { get; private set; }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            WasChanged = true;
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                WasChanged = false;
            }
        }
    }
}

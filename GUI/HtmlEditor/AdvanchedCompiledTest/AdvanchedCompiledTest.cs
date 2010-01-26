using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
  public partial class AdvancedCompiledTest : UserControl
  {
    public AdvancedCompiledTest()
    {
      InitializeComponent();
    }

    public void AdvancedCompiledTest_Resize(object sender, EventArgs e)
    {
      TextBoxBefore.Left = 10;
      TextBoxBefore.Top = 5;
      TextBoxUserCode.Left = 10;
      TextBoxUserCode.Top = this.Height / 3 + 5;
      TextBoxAfter.Left = 10;
      TextBoxAfter.Top = 2 * this.Height / 3 + 5;

      TextBoxBefore.Width = this.Width - 20;
      TextBoxUserCode.Width = this.Width - 20;
      TextBoxAfter.Width = this.Width - 20;
      TextBoxBefore.Height = this.Height/3 - 10;
      TextBoxUserCode.Height = this.Height/3 - 10;
      TextBoxAfter.Height = this.Height/3 - 10;
    }
  }
}

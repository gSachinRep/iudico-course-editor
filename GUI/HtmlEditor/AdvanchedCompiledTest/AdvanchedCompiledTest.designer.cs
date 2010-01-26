namespace FireFly.CourseEditor.GUI.HtmlEditor
{
  partial class AdvancedCompiledTest
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.TextBoxBefore = new System.Windows.Forms.TextBox();
      this.TextBoxAfter = new System.Windows.Forms.TextBox();
      this.TextBoxUserCode = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // TextBoxBefore
      // 
      this.TextBoxBefore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TextBoxBefore.Location = new System.Drawing.Point(15, 16);
      this.TextBoxBefore.Multiline = true;
      this.TextBoxBefore.Name = "TextBoxBefore";
      this.TextBoxBefore.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TextBoxBefore.Size = new System.Drawing.Size(212, 65);
      this.TextBoxBefore.TabIndex = 1;
      // 
      // TextBoxAfter
      // 
      this.TextBoxAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TextBoxAfter.Location = new System.Drawing.Point(15, 160);
      this.TextBoxAfter.Multiline = true;
      this.TextBoxAfter.Name = "TextBoxAfter";
      this.TextBoxAfter.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TextBoxAfter.Size = new System.Drawing.Size(212, 77);
      this.TextBoxAfter.TabIndex = 2;
      // 
      // TextBoxUserCode
      // 
      this.TextBoxUserCode.Enabled = false;
      this.TextBoxUserCode.Location = new System.Drawing.Point(15, 87);
      this.TextBoxUserCode.Multiline = true;
      this.TextBoxUserCode.Name = "TextBoxUserCode";
      this.TextBoxUserCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TextBoxUserCode.Size = new System.Drawing.Size(212, 67);
      this.TextBoxUserCode.TabIndex = 3;
      // 
      // AdvancedCompiledTest
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.TextBoxUserCode);
      this.Controls.Add(this.TextBoxAfter);
      this.Controls.Add(this.TextBoxBefore);
      this.Name = "AdvancedCompiledTest";
      this.Size = new System.Drawing.Size(245, 251);
      this.Resize += new System.EventHandler(this.AdvancedCompiledTest_Resize);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.TextBox TextBoxBefore;
    public System.Windows.Forms.TextBox TextBoxAfter;
    public System.Windows.Forms.TextBox TextBoxUserCode;

  }
}

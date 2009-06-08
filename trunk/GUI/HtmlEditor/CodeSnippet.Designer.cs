namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    partial class CodeSnippet
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
            this.codeSnippetBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // codeSnippetBrowser
            // 
            this.codeSnippetBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.codeSnippetBrowser.Location = new System.Drawing.Point(0, 0);
            this.codeSnippetBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.codeSnippetBrowser.Name = "codeSnippetBrowser";
            this.codeSnippetBrowser.Size = new System.Drawing.Size(176, 51);
            this.codeSnippetBrowser.TabIndex = 0;
            this.codeSnippetBrowser.TabStop = false;
            // 
            // CodeSnippet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.codeSnippetBrowser);
            this.Name = "CodeSnippet";
            this.Size = new System.Drawing.Size(175, 66);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser codeSnippetBrowser;


    }
}

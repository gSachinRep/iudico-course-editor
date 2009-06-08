namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    partial class SimpleQuestion
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
            this.controlPanel = new System.Windows.Forms.Panel();
            this.controlsCount = new System.Windows.Forms.NumericUpDown();
            this.singleCase = new System.Windows.Forms.CheckBox();
            this.QuestionText = new System.Windows.Forms.RichTextBox();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.controlsCount)).BeginInit();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPanel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.controlPanel.Controls.Add(this.controlsCount);
            this.controlPanel.Controls.Add(this.singleCase);
            this.controlPanel.Location = new System.Drawing.Point(0, 102);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(216, 27);
            this.controlPanel.TabIndex = 0;
            // 
            // controlsCount
            // 
            this.controlsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlsCount.Location = new System.Drawing.Point(170, 2);
            this.controlsCount.Name = "controlsCount";
            this.controlsCount.Size = new System.Drawing.Size(39, 20);
            this.controlsCount.TabIndex = 5;
            this.controlsCount.ValueChanged += new System.EventHandler(this.controlsCount_ValueChanged);
            // 
            // singleCase
            // 
            this.singleCase.AutoSize = true;
            this.singleCase.Location = new System.Drawing.Point(8, 7);
            this.singleCase.Name = "singleCase";
            this.singleCase.Size = new System.Drawing.Size(82, 17);
            this.singleCase.TabIndex = 4;
            this.singleCase.Text = "Single Case";
            this.singleCase.UseVisualStyleBackColor = true;
            this.singleCase.CheckedChanged += new System.EventHandler(this.singleCase_CheckedChanged);
            // 
            // QuestionText
            // 
            this.QuestionText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.QuestionText.EnableAutoDragDrop = true;
            this.QuestionText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.QuestionText.Location = new System.Drawing.Point(8, 3);
            this.QuestionText.Name = "QuestionText";
            this.QuestionText.Size = new System.Drawing.Size(200, 93);
            this.QuestionText.TabIndex = 3;
            this.QuestionText.Text = "";
            this.QuestionText.WordWrap = false;
            // 
            // SimpleQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.QuestionText);
            this.Controls.Add(this.controlPanel);
            this.MinimumSize = new System.Drawing.Size(216, 176);
            this.Name = "SimpleQuestion";
            this.Size = new System.Drawing.Size(216, 176);
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.controlsCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.RichTextBox QuestionText;
        private System.Windows.Forms.CheckBox singleCase;
        private System.Windows.Forms.NumericUpDown controlsCount;
    }
}

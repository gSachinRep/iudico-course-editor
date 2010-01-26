namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    partial class PageEditor
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
          this.components = new System.ComponentModel.Container();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageEditor));
          this.tsComponents = new System.Windows.Forms.ToolStrip();
          this.miUndo = new System.Windows.Forms.ToolStripButton();
          this.miRedo = new System.Windows.Forms.ToolStripButton();
          this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this.btnPointer = new System.Windows.Forms.ToolStripButton();
          this.btnLabel = new System.Windows.Forms.ToolStripButton();
          this.btnTextEdit = new System.Windows.Forms.ToolStripButton();
          this.btnButton = new System.Windows.Forms.ToolStripButton();
          this.btnComboBox = new System.Windows.Forms.ToolStripButton();
          this.btnSimpleQuestion = new System.Windows.Forms.ToolStripButton();
          this.btnCodeSnippet = new System.Windows.Forms.ToolStripButton();
          this.btnHighlightedText = new System.Windows.Forms.ToolStripButton();
          this.btnCompiledTest = new System.Windows.Forms.ToolStripButton();
          this.btnAdvancedCompiledTest = new System.Windows.Forms.ToolStripButton();
          this.cmsHtmlElement = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.miEditInMSWord = new System.Windows.Forms.ToolStripMenuItem();
          this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
          this.miProperties = new System.Windows.Forms.ToolStripMenuItem();
          this.tsComponents.SuspendLayout();
          this.cmsHtmlElement.SuspendLayout();
          this.SuspendLayout();
          // 
          // tsComponents
          // 
          this.tsComponents.GripMargin = new System.Windows.Forms.Padding(0);
          this.tsComponents.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
          this.tsComponents.ImageScalingSize = new System.Drawing.Size(26, 26);
          this.tsComponents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miUndo,
            this.miRedo,
            this.toolStripSeparator1,
            this.btnPointer,
            this.btnLabel,
            this.btnTextEdit,
            this.btnButton,
            this.btnComboBox,
            this.btnSimpleQuestion,
            this.btnCodeSnippet,
            this.btnHighlightedText,
            this.btnCompiledTest,
            this.btnAdvancedCompiledTest});
          this.tsComponents.Location = new System.Drawing.Point(0, 0);
          this.tsComponents.Name = "tsComponents";
          this.tsComponents.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
          this.tsComponents.Size = new System.Drawing.Size(474, 33);
          this.tsComponents.TabIndex = 1;
          this.tsComponents.Text = "Components";
          // 
          // miUndo
          // 
          this.miUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.miUndo.Image = global::FireFly.CourseEditor.Properties.Resources.Undo;
          this.miUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.miUndo.Name = "miUndo";
          this.miUndo.Size = new System.Drawing.Size(30, 30);
          this.miUndo.Tag = "Undo";
          this.miUndo.Text = "Undo";
          this.miUndo.ToolTipText = "Undo";
          this.miUndo.Click += new System.EventHandler(this.miUndo_Click);
          // 
          // miRedo
          // 
          this.miRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.miRedo.Image = global::FireFly.CourseEditor.Properties.Resources.Redo;
          this.miRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.miRedo.Name = "miRedo";
          this.miRedo.Size = new System.Drawing.Size(30, 30);
          this.miRedo.Tag = "Redo";
          this.miRedo.Text = "Redo";
          this.miRedo.Click += new System.EventHandler(this.miRedo_Click);
          // 
          // toolStripSeparator1
          // 
          this.toolStripSeparator1.Name = "toolStripSeparator1";
          this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
          // 
          // btnPointer
          // 
          this.btnPointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnPointer.Image = global::FireFly.CourseEditor.Properties.Resources.HtmlToolBar_Cursor;
          this.btnPointer.ImageTransparentColor = System.Drawing.Color.White;
          this.btnPointer.Name = "btnPointer";
          this.btnPointer.Size = new System.Drawing.Size(30, 30);
          this.btnPointer.Tag = "Pointer";
          this.btnPointer.Text = "Pointer";
          this.btnPointer.ToolTipText = "Select Object";
          // 
          // btnLabel
          // 
          this.btnLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnLabel.Image = global::FireFly.CourseEditor.Properties.Resources.HtmlToolBar_Label;
          this.btnLabel.ImageTransparentColor = System.Drawing.Color.White;
          this.btnLabel.Name = "btnLabel";
          this.btnLabel.Size = new System.Drawing.Size(30, 30);
          this.btnLabel.Tag = "Label";
          this.btnLabel.Text = "Label";
          // 
          // btnTextEdit
          // 
          this.btnTextEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnTextEdit.Image = global::FireFly.CourseEditor.Properties.Resources.HtmlToolBar_Edit;
          this.btnTextEdit.ImageTransparentColor = System.Drawing.Color.White;
          this.btnTextEdit.Name = "btnTextEdit";
          this.btnTextEdit.Size = new System.Drawing.Size(30, 30);
          this.btnTextEdit.Tag = "TextBox";
          this.btnTextEdit.Text = "TextBox";
          this.btnTextEdit.ToolTipText = "Edit Field";
          // 
          // btnButton
          // 
          this.btnButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnButton.Image = global::FireFly.CourseEditor.Properties.Resources.HtmlToolBar_Button;
          this.btnButton.ImageTransparentColor = System.Drawing.Color.White;
          this.btnButton.Name = "btnButton";
          this.btnButton.Size = new System.Drawing.Size(30, 30);
          this.btnButton.Tag = "Button";
          this.btnButton.Text = "Submit Button";
          this.btnButton.ToolTipText = "Submit Button";
          // 
          // btnComboBox
          // 
          this.btnComboBox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnComboBox.Image = global::FireFly.CourseEditor.Properties.Resources.HtmlToolBar_ComboBox;
          this.btnComboBox.ImageTransparentColor = System.Drawing.Color.White;
          this.btnComboBox.Name = "btnComboBox";
          this.btnComboBox.Size = new System.Drawing.Size(30, 30);
          this.btnComboBox.Tag = "ComboBox";
          this.btnComboBox.Text = "ComboBox";
          this.btnComboBox.ToolTipText = "Combo Box";
          // 
          // btnSimpleQuestion
          // 
          this.btnSimpleQuestion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnSimpleQuestion.Image = global::FireFly.CourseEditor.Properties.Resources.HtmlToolBar_SimpleQuestion;
          this.btnSimpleQuestion.ImageTransparentColor = System.Drawing.Color.White;
          this.btnSimpleQuestion.Name = "btnSimpleQuestion";
          this.btnSimpleQuestion.Size = new System.Drawing.Size(30, 30);
          this.btnSimpleQuestion.Tag = "SimpleQuestion";
          this.btnSimpleQuestion.Text = "SimpleQuestion";
          this.btnSimpleQuestion.ToolTipText = "Simple Question";
          // 
          // btnCodeSnippet
          // 
          this.btnCodeSnippet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnCodeSnippet.Image = global::FireFly.CourseEditor.Properties.Resources.word_big;
          this.btnCodeSnippet.ImageTransparentColor = System.Drawing.Color.White;
          this.btnCodeSnippet.Name = "btnCodeSnippet";
          this.btnCodeSnippet.Size = new System.Drawing.Size(30, 30);
          this.btnCodeSnippet.Tag = "CodeSnippet";
          this.btnCodeSnippet.Text = "CodeSnippet";
          this.btnCodeSnippet.ToolTipText = "Code Snippet";
          // 
          // btnHighlightedText
          // 
          this.btnHighlightedText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnHighlightedText.Image = ((System.Drawing.Image)(resources.GetObject("btnHighlightedText.Image")));
          this.btnHighlightedText.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.btnHighlightedText.Name = "btnHighlightedText";
          this.btnHighlightedText.Size = new System.Drawing.Size(30, 30);
          this.btnHighlightedText.Tag = "HighlightedText";
          this.btnHighlightedText.Text = "HighlightedText";
          this.btnHighlightedText.ToolTipText = "Highlighted Text";
          // 
          // btnCompiledTest
          // 
          this.btnCompiledTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnCompiledTest.Image = ((System.Drawing.Image)(resources.GetObject("btnCompiledTest.Image")));
          this.btnCompiledTest.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.btnCompiledTest.Name = "btnCompiledTest";
          this.btnCompiledTest.Size = new System.Drawing.Size(30, 30);
          this.btnCompiledTest.Text = "Compiled Test";
          // 
          // btnAdvancedCompiledTest
          // 
          this.btnAdvancedCompiledTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.btnAdvancedCompiledTest.Image = ((System.Drawing.Image)(resources.GetObject("btnAdvancedCompiledTest.Image")));
          this.btnAdvancedCompiledTest.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.btnAdvancedCompiledTest.Name = "btnAdvancedCompiledTest";
          this.btnAdvancedCompiledTest.Size = new System.Drawing.Size(30, 30);
          this.btnAdvancedCompiledTest.Text = "AdvancedCompiledTest";
          // 
          // cmsHtmlElement
          // 
          this.cmsHtmlElement.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditInMSWord,
            this.miDelete,
            this.miProperties});
          this.cmsHtmlElement.Name = "cmsHtmlElement";
          this.cmsHtmlElement.Size = new System.Drawing.Size(178, 70);
          this.cmsHtmlElement.Opening += new System.ComponentModel.CancelEventHandler(this.cmsHtmlElement_Opening);
          // 
          // miEditInMSWord
          // 
          this.miEditInMSWord.Image = global::FireFly.CourseEditor.Properties.Resources.word;
          this.miEditInMSWord.Name = "miEditInMSWord";
          this.miEditInMSWord.ShortcutKeys = System.Windows.Forms.Keys.F4;
          this.miEditInMSWord.Size = new System.Drawing.Size(177, 22);
          this.miEditInMSWord.Text = "Edit in MS Word";
          this.miEditInMSWord.Click += new System.EventHandler(this.editInMSWordToolStripMenuItem_Click);
          // 
          // miDelete
          // 
          this.miDelete.Image = global::FireFly.CourseEditor.Properties.Resources.delete_16;
          this.miDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
          this.miDelete.Name = "miDelete";
          this.miDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
          this.miDelete.Size = new System.Drawing.Size(177, 22);
          this.miDelete.Text = "&Delete";
          this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
          // 
          // miProperties
          // 
          this.miProperties.Image = global::FireFly.CourseEditor.Properties.Resources.properties_16;
          this.miProperties.ImageTransparentColor = System.Drawing.Color.Fuchsia;
          this.miProperties.Name = "miProperties";
          this.miProperties.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Return)));
          this.miProperties.Size = new System.Drawing.Size(177, 22);
          this.miProperties.Text = "&Properties";
          // 
          // PageEditor
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.Controls.Add(this.tsComponents);
          this.Name = "PageEditor";
          this.Size = new System.Drawing.Size(474, 362);
          this.tsComponents.ResumeLayout(false);
          this.tsComponents.PerformLayout();
          this.cmsHtmlElement.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsComponents;
        private System.Windows.Forms.ToolStripButton btnPointer;
        private System.Windows.Forms.ToolStripButton btnTextEdit;
        private System.Windows.Forms.ToolStripButton btnButton;
        private System.Windows.Forms.ContextMenuStrip cmsHtmlElement;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripMenuItem miProperties;
        private System.Windows.Forms.ToolStripButton btnLabel;
        private System.Windows.Forms.ToolStripButton btnComboBox;
        private System.Windows.Forms.ToolStripButton btnSimpleQuestion;
        private System.Windows.Forms.ToolStripButton btnCodeSnippet;
        private System.Windows.Forms.ToolStripMenuItem miEditInMSWord;
        private System.Windows.Forms.ToolStripButton btnHighlightedText;
        private System.Windows.Forms.ToolStripButton btnCompiledTest;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton miUndo;
        private System.Windows.Forms.ToolStripButton miRedo;
        private System.Windows.Forms.ToolStripButton btnAdvancedCompiledTest;

    }
}

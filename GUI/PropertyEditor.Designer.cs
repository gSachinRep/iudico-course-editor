namespace FireFly.CourseEditor.GUI
{
    public partial class PropertyEditor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.cbScope = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(0, 28);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(208, 273);
            this.propertyGrid.TabIndex = 0;
            // 
            // cbScope
            // 
            this.cbScope.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScope.FormattingEnabled = true;
            this.cbScope.Location = new System.Drawing.Point(0, 3);
            this.cbScope.Name = "cbScope";
            this.cbScope.Size = new System.Drawing.Size(208, 21);
            this.cbScope.TabIndex = 1;
            this.cbScope.SelectedIndexChanged += new System.EventHandler(this.cbScope_SelectedIndexChanged);
            // 
            // PropertyEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 302);
            this.Controls.Add(this.cbScope);
            this.Controls.Add(this.propertyGrid);
            this.MinimumSize = new System.Drawing.Size(165, 260);
            this.Name = "PropertyEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TabText = "Properties";
            this.Text = "Properties";
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ComboBox cbScope;

    }
}
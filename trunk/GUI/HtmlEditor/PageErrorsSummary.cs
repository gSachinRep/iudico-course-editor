using System.Windows.Forms;
using System.Diagnostics;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Common;

    ///<summary>
    /// Control to display summary of errors of <see cref="HtmlPage" />
    ///</summary>
    public class PageErrorsSummary : UserControl
    {
        ///<summary>
        /// Create new object of <see cref="PageErrorsSummary" /> class
        ///</summary>
        public PageErrorsSummary()
        {
            InitializeComponent();
        }

        ///<summary>
        /// Binds control to <see cref="page" />
        ///</summary>
        ///<param name="page">Instance of <see cref="HtmlPage" /> control needs to bind to</param>
        public void Bind([NotNull]HtmlPage page)
        {
            if (IsBinded)
            {
                UnBind();
            }
            page.ErrorFixed += Page_ErrorFixed;
            page.ErrorFound += Page_ErrorFound;
            page.BeginValidate += Page_BeginValidate;
            page.EndValidate += Page_EndValidate;
            _Page = page;
            UpdateSummary();
        }

        /// <summary>
        /// Unbinds control from page previously binded
        /// </summary>
        public void UnBind()
        {
            Debug.Assert(IsBinded);
            _Page.ErrorFixed -= Page_ErrorFixed;
            _Page.ErrorFound -= Page_ErrorFound;
            _Page.BeginValidate -= Page_BeginValidate;
            _Page.EndValidate -= Page_EndValidate;
            _Page = null;
            UpdateSummary();
        }

        /// <summary>
        /// Gets value indicates is control binded to page
        /// </summary>
        public bool IsBinded
        {
            get { return _Page != null; }
        }

        private void Page_ErrorFound(IValidateble page, Error e)
        {
            if (!_InValidationState)
            {
                UpdateSummary();
            }
        }

        private void Page_ErrorFixed(IValidateble page, Error e)
        {
            if (!_InValidationState)
            {
                UpdateSummary();
            }
        }

        private void Page_BeginValidate(IValidateble page)
        {
            _InValidationState = true;
            
        }

        private void Page_EndValidate(IValidateble page)
        {
            _InValidationState = false;
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            Debug.WriteLine("Updating", "PageErrorSummary");
            if (IsBinded)
            {
                if (_Page.IsValid)
                {
                   Hide();  
                }
                else
                {
                    Show();
                    errorsLabel.Text = _Page.Errors.GetErrorsSummary(_Page);
                }
            }
            else
            {
                Hide(); 
            }
        }

        private HtmlPage _Page;
        private bool _InValidationState;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.errorsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // errorsLabel
            // 
            this.errorsLabel.AutoSize = true;
            this.errorsLabel.BackColor = System.Drawing.Color.White;
            this.errorsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsLabel.ForeColor = System.Drawing.Color.Red;
            this.errorsLabel.Location = new System.Drawing.Point(0, 0);
            this.errorsLabel.Name = "errorsLabel";
            this.errorsLabel.Size = new System.Drawing.Size(37, 26);
            this.errorsLabel.TabIndex = 0;
            this.errorsLabel.Text = "1 error\r\n2 error";
            // 
            // PageErrorsSummary
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.errorsLabel);
            this.Name = "PageErrorsSummary";
            this.Size = new System.Drawing.Size(37, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label errorsLabel;

        #endregion
    }
}

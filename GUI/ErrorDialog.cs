using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class ErrorDialog : Form
    {
        private static readonly ShowErrorDelegate showErrorInvoker = MessageBox.Show;

        public ErrorDialog()
        {
            if (instance != null)
            {
                throw new FireFlyException("ErrorDialog is already created");
            }
            InitializeComponent();
            instance = this;

            btnOk.Click += ((sender, e) => Close());
            btnTerminate.Click += ((sender, e) => Application.Exit());
        }

        public static ErrorDialog Instance
        {
            get { return instance; }
        }

        public static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (instance == null)
                {
                    instance = new ErrorDialog();
                }
                instance.ShowDialog(e.ExceptionObject as Exception);
            };
            Application.ThreadException += (sender, e) =>
            {
                if (instance == null)
                {
                    instance = new ErrorDialog();
                }
                instance.ShowDialog(e.Exception);
            };
        }

        public static void ShowError(string ErrorMessage)
        {
            var f = Forms.Main;
            if (f != null)
            {
                f.Invoke(showErrorInvoker, f, ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ShowError(string errorMessage, params object[] args)
        {
            ShowError(string.Format(errorMessage, args));
        }

        private void ShowDialog(Exception e)
        {
            if (e == null)
            {
                e = new FireFlyException("(No message defined)");
            }
            tbException.Text = e.Message;
            tbCallStack.Text = e.StackTrace;

            try
            {
                if (!Debugger.IsAttached)
                {
                    Process.Start(string.Concat("mailto:", Properties.Settings.Default.SupportEMailAddress,
                        "?subject=", "[Exception] ", e.Message,
                        "&body=", e.StackTrace.Replace("&", "%26")));
                }
            }
            catch
            {
            }

            ShowDialog();
        }

        private void btnMoreInfo_Click(object sender, EventArgs e)
        {
            moreInfoOpen = !moreInfoOpen;
            if (moreInfoOpen)
            {
                Size = new Size(Size.Width, 275);
                btnMoreInfo.Text = "Hide Additional Info";
            }
            else
            {
                Size = new Size(Size.Width, 142);
                btnMoreInfo.Text = "Show Additional Info";
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool moreInfoOpen;

        private static ErrorDialog instance;
    }
}
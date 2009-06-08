/*
namespace FireFly.CourseEditor.GUI.WaitForm
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Форма прогресса.
    /// </summary>
    internal class _WaitForm : Form
    {
        private const int imageCount = 3;
        private static Image[] _frames;
        private Timer animationTimer;
        private IContainer components;
        private int currentImage;
        private Label label1;
        private PictureBox pictureBox1;

        internal _WaitForm()
        {
            InitializeComponent();
            animationTimer.Enabled = true;
        }

        private static Image[] Images
        {
            get
            {
                if (_frames == null)
                {
                    _frames = new Image[imageCount];
                    for (int i = 0; i < imageCount; i++)
                        _frames[i] = Image.FromStream(typeof (_WaitForm).Assembly.GetManifestResourceStream(
                            string.Format("FireFly.CourseEditor.GUI.WaitForm.gear{0}.png", i)));
                }
                return _frames;
            }
        }

        public void DoCenter()
        {
            var mainForm = Forms.Main;
            Left = mainForm.Left + (mainForm.Width - Width)/2;
            Top = mainForm.Top + (mainForm.Height - Height)/2;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            DoCenter();
        }

        public new void Show()
        {
            base.Show();
            DoCenter();
        }

        internal void SetMessage(string message)
        {
            Text = message;
            label1.Text = message;
        }

        protected override void Dispose(bool disposing)
        {
            animationTimer.Dispose();
            base.Dispose(disposing);
        }

        private void _animationTimer_Tick(object sender, EventArgs e)
        {
            if (currentImage == imageCount)
                currentImage = 0;

            pictureBox1.Image = Images[currentImage];
            currentImage++;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof (_WaitForm));
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // animationTimer
            // 
            this.animationTimer.Interval = 150;
            this.animationTimer.Tick += new System.EventHandler(this._animationTimer_Tick);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // _WaitForm
            // 
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "_WaitForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }

    public sealed class WaitForm
    {
        //private static readonly _WaitForm form;
        private static string _message;

        public static string Message
        {
            get { return _message; }
            set
            {
                form.Invoke(new FormSetMessageDelegate(FormSetMessage), value);
                _message = value;
            }
        }

        //private static bool _initComplete;
        //private static int count = 0;

        public static void Show()
        {
            //if (count++ <= 0)
            //{
            //    Program.MainForm.Enabled = false;

            //    if (form == null)
            //    {
            //        ThreadPool.QueueUserWorkItem(
            //            delegate
            //                {
            //                form = new _WaitForm();
            //                _initComplete = true;
            //                Application.Run(form);
            //            });
            //    }
            //    else
            //        form.Invoke(new FormDelegate(FormShow));

            //    count = 1;
            //}
        }

        public static void Hide()
        {
            //if (--count == 0)
            //{
            //    while (!_initComplete || !form.IsHandleCreated)
            //        Thread.Sleep(0);

            //    form.Invoke(new FormDelegate(FormHide));

            //    Program.MainForm.Enabled = true;
            //    Program.MainForm.Activate(); 
            //}
        }


        private static void FormHide()
        {
            form.Hide();
        }
        private static void FormShow()
        {
            form.Show();
        }

        private static void FormSetMessage(string message)
        {
            form.SetMessage(message);
        }


        private delegate void FormDelegate();

        #region Nested type: FormSetMessageDelegate

        private delegate void FormSetMessageDelegate(string message);

        #endregion
    }
}
*/
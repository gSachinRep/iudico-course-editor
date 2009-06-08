using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System;
using System.Reflection;
using System.Drawing;

namespace FireFly.CourseEditor.GUI
{
    ///<summary>
    /// Splash form
    ///</summary>
    public sealed partial class SplashForm : Form
    {
        ///<summary>
        /// Creates new instance of SplashForm
        ///</summary>
        public SplashForm()
        {
            InitializeComponent();
            //Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("FireFly.CourseEditor.Images.Splash.jpg");
            //Debug.Assert(st != null);
            //Image img = Image.FromStream(st);
            //BackgroundImage = img;
            //TransparencyKey = Color.White;
            //ClientSize = new Size(img.Width, img.Height + progressBar1.Height);
        }

        ///<summary>
        /// Show splash form
        ///</summary>
        [Conditional("FALSE_CONDITION")]
        public static void ShowSplash(int progressStepCount)
        {
            Debug.Assert(__FormCreatedEvent == null && __ProgressDoneEvent == null);
            __FormCreatedEvent = new ManualResetEvent(false);
            __ProgressDoneEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(InternalShow, progressStepCount);
        }

        ///<summary>
        /// Hide splash form
        ///</summary>
        [Conditional("FALSE_CONDITION")]
        public static void HideSplash()
        {
            ThreadPool.QueueUserWorkItem(InternalHide, null);
        }

        ///<summary>
        /// Notify <see cref="SplashForm"/> that next step is done to increment progress status
        ///</summary>
        [Conditional("FALSE_CONDITION")]
        public static void StepDone()
        {
            ThreadPool.QueueUserWorkItem(InternalStep, null);
        }

        private static void InternalShow(object progressStepCount)
        {
            Debug.Assert(__Form == null);
            __Form = new SplashForm();
            __Form.progressBar1.Maximum = PROGRESS_STEP_ITEM_COUNT * (int)progressStepCount;
            __Form.Shown += (s, e) => __FormCreatedEvent.Set();
            __Form.ShowDialog();
        }

        private static void InternalStep(object nothing)
        {
            __FormCreatedEvent.WaitOne(); // To ensure splash screen is shown
            Debug.Assert(__Form != null);
            ProgressBar bar = __Form.progressBar1;
            for (int i = PROGRESS_STEP_ITEM_COUNT; i > 0; i--) // to get smooth progress
            {
                lock (typeof(SplashForm))
                {
                    if (bar.Value < bar.Maximum)
                    {
                        __Form.Invoke((Action) bar.PerformStep);
                    }
                    else
                    {
                        return;
                    }
                }
                Thread.Sleep(1);
                if (bar.Value >= bar.Maximum)
                {
                    __ProgressDoneEvent.Set();
                }
            }
        }

        private static void InternalHide(object nothing)
        {
            Thread.Sleep(10); // Just wait
            WaitHandle.WaitAll(new[] {__FormCreatedEvent, __ProgressDoneEvent});

            __Form.Invoke((Action<bool>)__Form.Dispose, true);
            __Form = null;

            ((IDisposable)__FormCreatedEvent).Dispose();
            __FormCreatedEvent = null;

            ((IDisposable)__ProgressDoneEvent).Dispose();
            __ProgressDoneEvent = null;

            GC.Collect();
        }

        private static SplashForm __Form;
        private const int PROGRESS_STEP_ITEM_COUNT = 80;
        private static ManualResetEvent __FormCreatedEvent;
        private static ManualResetEvent __ProgressDoneEvent;
    }
}

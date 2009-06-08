using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    [StartupInitializable]
    public class ErrorControl : Control
    {
        public static void Initialize()
        {
            var stream = Assembly.GetEntryAssembly().GetManifestResourceStream("FireFly.CourseEditor.Images.Error.bmp");
            if (stream == null)
            {
                throw new FireFlyException("Cannot load 'FireFly.CourseEditor.Images.Error.bmp'");
            }
            __Icon = Image.FromStream(stream);
            __ToolTip = new ToolTip();
        }

        public ErrorControl(Control control)
            : this(control, string.Empty)
        {
#if CHECKERS
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
#endif
        }

        public void SetErrors(IErrors errors)
        {
#if CHECKERS
            if (errors == null || errors.Count == 0)
            {
                throw new ArgumentNullException("errors");
            }
#endif

            __ToolTip.SetToolTip(this, errors.GetErrorsSummary());
        }

        private ErrorControl(Control parent, string text)
            : base(parent, text)
        {
            Size = __Icon.Size;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(__Icon, 0, 0);
        }

        private static Image __Icon;
        private static ToolTip __ToolTip;
    }
}

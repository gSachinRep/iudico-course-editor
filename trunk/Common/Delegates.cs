using System.Windows.Forms;

namespace FireFly.CourseEditor.Common
{
    public delegate DialogResult ShowErrorDelegate([NotNull]IWin32Window p1, [NotNull]string p2, [NotNull]string p3, MessageBoxButtons p4, MessageBoxIcon p5);
}

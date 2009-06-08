namespace FireFly.CourseEditor.GUI
{
    using System.Windows.Forms;
    using Common;

    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            pgMainConfig.SelectedObject = new FFTypeDescriptor(FFConfig.Instance);
            btnOk.Click += ((sender, e) => Close());
        }

        private void ResetDefaultsButton_Click(object sender, System.EventArgs e)
        {
            FFConfig.Instance.ResetDefaults();
            pgMainConfig.SelectedObject = new FFTypeDescriptor(FFConfig.Instance);
        }
    }
}
using System.IO;
using System.Windows.Forms;

namespace FireFly.CourseEditor.GUI
{
    using Properties;

    ///<summary>
    /// Web browser with monitor file and auto-refresh ability
    ///</summary>
    public class FFWebBrowser : WebBrowser
    {
        private readonly FileSystemWatcher _Watcher;
        private bool _Refreshed;

        private string _FileName;
        private string _Params;

        // TODO: Implement in-memory browsing.
        ///<summary>
        /// Creates new instance of <see cref="FFWebBrowser" />
        ///</summary>
        public FFWebBrowser()
        {
            _Watcher = new FileSystemWatcher
                           {
                               NotifyFilter = (NotifyFilters.Attributes | NotifyFilters.LastWrite),
                               SynchronizingObject = this,
                               EnableRaisingEvents = false
                           };
            
            // using any other application it works correctly but MSWord made me to add NotifyFilters.Attributes [Volodya Shtenyovych]
            _Watcher.Changed += (sender, e) =>
            {
                _Refreshed = !_Refreshed;
                if (_Refreshed)
                {
                    Navigate("file:///" + _FileName + _Params);
                }
            };
        }

        private void NavigateFileAndParams(string fileName, string @params)
        {
            _Watcher.EnableRaisingEvents = false;
            _FileName = fileName;
            _Params = @params;
            Navigate("file:///" + fileName + @params);
            _Watcher.Path = Path.GetDirectoryName(fileName);
            _Watcher.Filter = Path.GetFileName(fileName);
            _Watcher.EnableRaisingEvents = true;
        }

        public void NavigateItem(string fileName, string @params)
        {
            var ps = @params ?? "";

            if (Settings.Default.Options_EnableLMSEmulation)
            {
                ps += (ps == "" ? "?" : "&") + "EmulateLMS=true";
            }
            if (ps != "" && !ps.StartsWith("?"))
            {
                ps = "?" + ps;
            }
            NavigateFileAndParams(fileName, ps);
        }
    }
}
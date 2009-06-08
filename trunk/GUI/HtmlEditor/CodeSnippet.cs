using System.Diagnostics;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;
    using Course;
    using Microsoft.Office.Interop.Word;
    using Sgml;

    public partial class CodeSnippet : UserControl
    {
        private string _htmlCode;

        public CodeSnippet()
        {
            InitializeComponent();
        }

        public string HtmlCode
        {
            get { return _htmlCode; }
            set
            {
                _htmlCode = value;
                codeSnippetBrowser.DocumentText = AddAbsoulutePath(_htmlCode);
            }
        }

        public void EditInWord()
        {
            string fileName = Path.Combine(Course.FullPath, string.Format(@"{0}.html", Name));
            WriteToNewWordFile(fileName, HtmlCode);
            Process p = CourseDesigner.EditUsingWord(fileName);
            if (p != null)
            {
                using (p)
                {
                    p.WaitForExit();
                }
            }
            HtmlCode = TransformHtmlToXHTML(FileUtils.ReadAllText(fileName));
            File.Delete(fileName);
        }

        public static void WriteToNewWordFile(string path, string text)
        {
            ApplicationClass wordApp = new ApplicationClass();

            object nullobj = Missing.Value;
            object file = path;
            object fileFormat = WdSaveFormat.wdFormatHTML;

            FileUtils.WriteAllText(path, text);

            var doc = wordApp.Documents.Add(ref nullobj, ref nullobj, ref nullobj, ref nullobj);

            if (text.IsNull())// If file open in first time save as word html
            {
                doc.SaveAs(ref file, ref fileFormat, ref nullobj, ref nullobj,
                    ref nullobj, ref nullobj, ref nullobj, ref nullobj,
                    ref nullobj, ref nullobj, ref nullobj,
                    ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj);
            }
            (doc as _Document).Close(ref nullobj, ref nullobj, ref nullobj);
            wordApp.Quit(ref nullobj, ref nullobj, ref nullobj);
        }

        public static string TransformHtmlToXHTML(string inputHtml)
        {
            var sgmlReader = new SgmlReader {DocType = "HTML"};
            var stringReader = new StringReader(inputHtml);
            sgmlReader.InputStream = stringReader;
            var stringWriter = new StringWriter();
            using (var xmlWriter = new XmlTextWriter(stringWriter))
            {
                sgmlReader.Read();

                while (!sgmlReader.EOF)
                {
                    xmlWriter.WriteNode(sgmlReader, true);
                }
            }
            return RemoveCopyOfImage(stringWriter.ToString());
        }

        private static string AddAbsoulutePath(string inputHtml)
        {
            var aPath = string.Format("<v:imagedata src=\"{0}\\", Course.FullPath);
            return inputHtml.Replace("<v:imagedata src=\"", aPath);
        }

        private static string RemoveCopyOfImage(string inputhtml)
        {
            return Regex.Replace(inputhtml,
                @"<img width=""[\d]+"" height=""[\d]+"" src=""[\S\]+"" [alt=""[\w\d]*]*"" v:shapes=""[\w\d]+"" />",
                string.Empty);
        }
    }
}
using System;
using System.IO;
using System.Web.UI;
using System.Text;
using FireFly.CourseEditor.Course.Manifest;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    public class FFHtmlWriter : HtmlTextWriter, IDisposable
    {
        //protected FFHtmlWriter(TextWriter writer): 
        //    base(writer, string.Empty)
        //{
        //    //Indent = 0;
        //}

        public FFHtmlWriter(ItemType item)
            : base(new StreamWriter(item.PageHref, false, Encoding.UTF8) { AutoFlush = false })
        {
        }

        public override string NewLine
        {
            get
            {
                return string.Empty;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        void IDisposable.Dispose()
        {
            InnerWriter.Flush();
            InnerWriter.Dispose();
            Dispose();
        }
    }
}

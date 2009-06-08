using System.IO;
using System.Text;
using FireFly.CourseEditor.GUI;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.Properties
{
    internal partial class Settings
    {
        public LastCoursesXml LastCoursesXml
        {
            get
            {
                var lc = LastCourses;
                if (lc.IsNotNull())
                {
                    return (LastCoursesXml)LastCoursesXml.Serializer.Deserialize(new StringReader(lc));
                }
                return new LastCoursesXml();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    LastCourses = null;
                }
                else
                {
                    var sb = new StringBuilder();
                    LastCoursesXml.Serializer.Serialize(new StringWriter(sb), value);
                    LastCourses = sb.ToString();
                }
            }
        }
    }
}

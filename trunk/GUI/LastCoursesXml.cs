using System.Collections.Generic;
using System.Xml.Serialization;

namespace FireFly.CourseEditor.GUI
{
    public class LastCoursesXml : List<string>
    {
        public static XmlSerializer Serializer;
    }
}

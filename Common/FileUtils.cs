using System.IO;
using System.Text;

namespace FireFly.CourseEditor.Common
{
    public static class FileUtils
    {
        /// <summary>
        /// This methods should be used instead of <see cref="File.ReadAllText(string)"/> because of encoding should be figgured out automatically
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>String content of the specified file</returns>
        [NotNull]
        public static  string ReadAllText([NotNull] string path)
        {
            using (var sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }

        public static void WriteAllText([NotNull] string path, [NotNull] string content)
        {
            File.WriteAllText(path, content, Encoding.Unicode);
        }
    }
}

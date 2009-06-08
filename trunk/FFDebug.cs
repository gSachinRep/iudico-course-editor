using System;

namespace FireFly.CourseEditor
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    public enum Cattegory
    {
        Course,
        MovableControl,
        HtmlControl
    }

    public static class FFDebug
    {
        [Conditional("DEBUG")]
        public static void EnterMethod(Cattegory cattegory, string @params)
        {
            var frames = new StackTrace().GetFrames();
            var method = frames[1].GetMethod();
            var logMessage = new StringBuilder(method.DeclaringType.Name);
            logMessage.Append('.');
            logMessage.Append(method.Name);
            logMessage.Append('(');
            logMessage.Append(@params);
            logMessage.Append(");");
            EnterMethod2(cattegory, logMessage.ToString());
        }

        [Conditional("DEBUG")]
        public static void EnterMethod2(Cattegory cattegory, string message)
        {
            Debug.WriteLine(message, cattegory.ToString());
            Debug.Indent();
        }

        [Conditional("DEBUG")]
        public static void LeaveMethod(Cattegory cattegory, string message)
        {
            Debug.Unindent();
            Debug.WriteLine(message, cattegory.ToString());
        }

        [Conditional("DEBUG")]
        public static void LeaveMethod(Cattegory cattegory, string message, params object[] @params)
        {
            LeaveMethod(cattegory, string.Format(message, @params));
        }

        [Conditional("DEBUG")]
        public static void LeaveMethod(Cattegory cattegory, MethodBase method)
        {
            LeaveMethod(cattegory, "Leave: " + method.Name, cattegory);
        }
    }

#if LOGGER
    public class Logger : Stopwatch, IDisposable
    {
        public static Logger Scope(string title)
        {
            Trace.Indent();
            Trace.WriteLine(title);
            var res = new Logger {_Title = title };
            res.Start();
            return res;
        }

        private Logger()
        {
        }

        public void Dispose()
        {
            Stop();
            Trace.WriteLine("END - " + _Title + ": " + Elapsed);
            Trace.Unindent();
        }

        private string _Title;
    }
#endif
}

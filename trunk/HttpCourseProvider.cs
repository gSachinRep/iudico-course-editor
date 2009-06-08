using System;
using System.IO;
using System.Net;
using System.Threading;
using FireFly.CourseEditor.Common;
using FireFly.CourseEditor.GUI;

namespace FireFly.CourseEditor
{
    class HttpCourseProvider
    {
        public string PlayerLocation { get; set; }

        private void DoProcessRequest(HttpListenerContext context)
        {
            try
            {
                var response = context.Response;
                var url = context.Request.Url.LocalPath;
                url = url.Replace('/', '\\');
                if (url.EndsWith("\\"))
                {
                    url += "index.html";
                }

                var courseRequested = url.Contains("course");
                var location = courseRequested ? Course.Course.FullPath : PlayerLocation;
                if (courseRequested)
                {
                    url = url.Remove(0, "/course".Length);
                }

                if (!File.Exists(location + url))
                {
                    response.StatusCode = 404;
                    response.StatusDescription = "File not found.";
                }
                else
                {
                    try
                    {
                        using (var r = new FileStream(location + url, FileMode.Open, FileAccess.Read))
                        {
                            var buf = new byte[BufSize];
                            int c;
                            do
                            {
                                c = r.Read(buf, 0, BufSize);
                                response.OutputStream.Write(buf, 0, c);
                            }
                            while (c > 0);
                        }

                        response.StatusCode = 200;
                        response.StatusDescription = "Ok";
                    }
                    catch (Exception e)
                    {
                        response.StatusCode = 500;
                        response.StatusDescription = "Internal Server error";

                        ErrorDialog.ShowError(e.Message);
                    }
                }
            }
            catch (HttpListenerException e)
            {
                var ec = e.ErrorCode;
                if (ec != 64 && ec != 1236 && ec != 1229)
                {
                    throw;
                }
            }
            finally
            {
                context.Response.Close();
            }
        }

        public void Start()
        {
            if (m_Listener != null)
            {
                throw new FireFlyException("HttpListener already started.");
            }
            if (!File.Exists(PlayerLocation + "/index.html"))
            {
                ErrorDialog.ShowError("Can start course preview. Player not found.\n");
            }

            if (ConfigurePlayer())
            {
                m_Listener = new HttpListener();
                m_Listener.Prefixes.Add("http://localhost:" + Port + "/");
                m_Listener.IgnoreWriteExceptions = true;

                try
                {
                    m_Listener.Start();
                }
                catch (HttpListenerException e)
                {
                    m_Listener = null;
                    ErrorDialog.ShowError("Can't start HTTP Listener. Maybe you already run preview in another instance of this application.\n" + e.Message);
                }

                ThreadPool.QueueUserWorkItem(obj =>
                {
                    var l = (HttpListener)obj;
                    while (l != null && l.IsListening)
                    {
                        try
                        {
                            DoProcessRequest(l.GetContext());
                        }
                        catch (HttpListenerException e)
                        {
                            if (e.ErrorCode != 995)
                            {
                                ErrorDialog.ShowError(e.Message);
                            }
                        }
                    }
                }, m_Listener);
            }
        }

        private bool ConfigurePlayer()
        {
            string playerHtmlFile;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(new FileStream(PlayerLocation + "/index.html", FileMode.Open));
                playerHtmlFile = reader.ReadToEnd();
            }
            catch (IOException e)
            {
                ErrorDialog.ShowError("Can't read player configuration." + Environment.NewLine + "Details: " + e.Message);
                return false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            playerHtmlFile = playerHtmlFile.Replace("<!!resourcelocation!!>", "http://localhost:" + Port);
            playerHtmlFile = playerHtmlFile.Replace("<!!courselocation!!>", "http://localhost:" + Port + "/course/");

            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(new FileStream(PlayerLocation + "/index.html", FileMode.Create));
                writer.Write(playerHtmlFile);
            }
            catch (IOException e)
            {
                ErrorDialog.ShowError("Can't write player configuration." + Environment.NewLine + "Details: " + e.Message);
                return false;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
            return true;
        }

        public void Stop()
        {
            if (m_Listener != null)
            {
                m_Listener.Prefixes.Clear();
                m_Listener.Abort();
                m_Listener = null;
            }
            else
            {
                throw new FireFlyException("HttpListener already stopped.");
            }
        }

        public bool TryStopService()
        {
            try
            {
                Stop();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public const int Port = 13000;
        private HttpListener m_Listener;
        private const int BufSize = 1024 * 1024;
    }
}

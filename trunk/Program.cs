using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;

#if PRECOMPILATION
using System.CodeDom.Compiler;
#endif

namespace FireFly.CourseEditor
{
    using GUI;
    using Course;
    using Course.Manifest;                                              
    using Common;
#if !PRECOMPILATION
    using GUI.HtmlEditor;
#endif

    ///<summary>
    /// Static storage for singleton instances of Application Forms 
    ///</summary>
    public static class Forms
    {
        ///<summary>
        /// Singleton instance of <see cref="GUI.ManifestBrowser" /> class
        ///</summary>
        [NotNull]
        public static ManifestBrowser ManifestBrowser;

        ///<summary>
        /// Singleton instance of <see cref="GUI.PropertyEditor" /> class
        ///</summary>
        [NotNull]
        public static PropertyEditor PropertyEditor;

        ///<summary>
        /// Singleton instance of <see cref="GUI.CourseExplorer" /> class
        ///</summary>
        [NotNull]
        public static CourseExplorer CourseExplorer;
        
        ///<summary>
        /// Singleton instance of <see cref="GUI.CourseDesigner" /> class
        ///</summary>
        [NotNull]
        public static CourseDesigner CourseDesigner;

        ///<summary>
        /// Singleton instance of <see cref="GUI.MainForm" /> class - Main Form of Course Editor
        ///</summary>
        [NotNull]
        public static MainForm Main;
    }

    [StartupInitializable]
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern Boolean FreeConsole();

        private static void AllocOrAttachConsole()
        {
            if (!AttachConsole(-1))
            {
                AllocConsole();
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

#if !PRECOMPILATION

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] ps)
        {
            if (ps.Length <= 1)
            {
                SplashForm.ShowSplash(11);

                SplashForm.StepDone();
                InitializeUtils.Initialize(Assembly.GetExecutingAssembly());
                SplashForm.StepDone();

                ErrorDialog.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                SplashForm.StepDone();

                var mainForm = Forms.Main = new MainForm();
                SplashForm.StepDone();
                var docPanel = mainForm.DockingPanel;
                SplashForm.StepDone();
                Forms.ManifestBrowser = new ManifestBrowser(docPanel);
                SplashForm.StepDone();
                Forms.PropertyEditor = new PropertyEditor(docPanel);
                SplashForm.StepDone();
                Forms.CourseExplorer = new CourseExplorer(docPanel);
                SplashForm.StepDone();
                Forms.CourseDesigner = new CourseDesigner(docPanel);
                SplashForm.StepDone();

                if (ps.Length == 1)
                {
                    mainForm.OpenCourse(ps[0], true);
                }
                SplashForm.StepDone();

                Application.Run(mainForm);
            }
            else
            {
                InitializeUtils.Initialize(Assembly.GetExecutingAssembly());
                AllocOrAttachConsole();
                if (ps[0].Equals("--upgrade", StringComparison.InvariantCultureIgnoreCase))
                {
                    for (var i = 1; i < ps.Length; i++)
                    {
                        var dirName = Path.GetDirectoryName(ps[i]);
                        var fileMask = Path.GetFileName(ps[i]);
                        var files = Directory.GetFiles(dirName, fileMask);
                        foreach (var file in files)
                        {
                            try
                            {
                                Console.Write("Upgrading '{0}'... ", file);
                                if (Course.Course.OpenZipPackage(file))
                                {
                                    Course.Course.SaveToZipPackage(file);
                                }
                                else
                                {
                                    Console.WriteLine("ERROR ON OPENNING");
                                }
                                Console.WriteLine("[DONE]");  
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("[ERROR]");
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
                FreeConsole();
            }
        }

        public static void Initialize()
        {
#if CHECKERS
            if (ManifestType.Serializer != null || Answers.Serializer != null || HtmlTextBox.TextBoxInit != null || LastCoursesXml.Serializer != null)
            {
                throw new FireFlyException("Already initialized");
            }
#endif
#if LOGGER
            using (Logger.Scope("Initialize precompiled code"))
            {
#endif
#if !DISABLE_PRECOMPILATION
                var a = Assembly.Load("CourseEditor.xmls");
                var t = a.GetType("Microsoft.Xml.Serialization.GeneratedAssembly.ManifestTypeSerializer");
                ManifestType.Serializer = t.Create<XmlSerializer>();

                t = a.GetType("Microsoft.Xml.Serialization.GeneratedAssembly.AnswersSerializer");
                Answers.Serializer = t.Create<XmlSerializer>();

                t = a.GetType("Microsoft.Xml.Serialization.GeneratedAssembly.LastCoursesXmlSerializer");
                LastCoursesXml.Serializer = t.Create<XmlSerializer>();

                a = Assembly.Load("regex");
                t = a.GetType("TextBoxInitRegEx");
                HtmlTextBox.TextBoxInit = t.Create<Regex>();
#else
                Answers.Serializer = new XmlSerializer(typeof(Answers));
                ManifestType.Serializer = new XmlSerializer(typeof(ManifestType));
                LastCoursesXml.Serializer = new XmlSerializer(typeof(LastCoursesXml));
                HtmlTextBox.TextBoxInit = new Regex(@"textBoxInit\(document.all\['(?<id>\w+)'\],\s*'(?<emptyText>.+?)'\)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

#endif
#if LOGGER
            }
#endif
        }
#else
        private static void Main()
        {
            AllocOrAttachConsole();
            PrecompileXmlSerializers(typeof(ManifestType), typeof(Answers), typeof(LastCoursesXml));
            PrecompileRegEx();
            FreeConsole();
        }

        private static void PrecompileXmlSerializers(params Type[] types)
        {
            var xms = new XmlTypeMapping[types.Length];
            var ind = 0;
            var importer = new XmlReflectionImporter();
            foreach (var t in types)
            {
                xms[ind++] = importer.ImportTypeMapping(t);
                Console.WriteLine("> type {0}", t);
            }

            var outDir = Path.GetDirectoryName(Application.ExecutablePath);
            var cp = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = false,
                IncludeDebugInformation = false,
                OutputAssembly = Path.Combine(outDir, Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".xmls.dll")
            };
            cp.ReferencedAssemblies.Add(Application.ExecutablePath);

            if (File.Exists(cp.OutputAssembly))
            {
                Console.WriteLine("Deleting file {0}", cp.OutputAssembly);
                File.Delete(cp.OutputAssembly);
            }

            Console.WriteLine("Generating assembly {0}...", cp.OutputAssembly);
            XmlSerializer.GenerateSerializer(types, xms, cp);
        }

        private static void PrecompileRegEx()
        {
            var regexAsm = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "regex.dll");
            if (File.Exists(regexAsm))
            {
                Console.WriteLine("Deleting file {0}", regexAsm);
                File.Delete(regexAsm);
            }
            Console.WriteLine("Generating assembly {0}...", regexAsm);
            var rAsmShortName = Path.GetFileNameWithoutExtension((regexAsm));
            Regex.CompileToAssembly(new[] 
            { new RegexCompilationInfo(@"textBoxInit\(document.all\['(?<id>\w+)'\],\s*'(?<emptyText>.+?)'\)",
                      RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled, 
                      "TextBoxInitRegEx", 
                      string.Empty, true)},
                      new AssemblyName(rAsmShortName));
            File.Move(rAsmShortName + ".dll", regexAsm);

            Console.WriteLine("Done.");
        }
#endif
    }
}
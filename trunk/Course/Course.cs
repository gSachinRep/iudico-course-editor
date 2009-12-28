using FireFly.CourseEditor.GUI;
using FireFly.CourseEditor.GUI.HtmlEditor;
using FireFly.CourseEditor.Properties;

namespace FireFly.CourseEditor.Course
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Common;
    using Manifest;

    /// <summary>
    /// This class encapsulate all logic of working with course
    /// </summary>
    public static class Course
    {
        private static readonly Dictionary<string, int> __ContainedFileResource = new Dictionary<string, int>();

        /// <summary>
        /// Defines the name of file with answers in course package
        /// </summary>
        public const string ANSWERS_FILE_NAME = "answers.xml";

        /// <summary>
        /// Defines the name of file with answers in course package
        /// </summary>
        public const string LANGUAGES_NAMESPACE = "FireFly.CourseEditor.GUI.HtmlEditor.HighlightControl.";

        ///// <summary>
        /// Defines the name of file with answers in course package
        /// </summary>
        public const string MANIFEST_FILE_NAME = "imsmanifest.xml";


        ///<summary>
        /// Namespace of <see cref="Course"/> class
        ///</summary>
        public const string NAMESPACE = "FireFly.CourseEditor.Course.";

        /// <summary>
        /// Gets current state of course
        /// </summary>
        public static CourseStates State { get; private set; }

        /// <summary>
        /// Course's manifest
        /// </summary>
        [NotNull]
        public static ManifestType Manifest
        {
            [DebuggerStepThrough]
            get { return __Manifest; }
        }

        ///<summary>
        /// Current organization
        ///</summary>
        [NotNull]
        public static OrganizationType Organization
        {
            [DebuggerStepThrough]
            get
            {
                return __Organization;
            }
        }

        /// <summary>
        /// Gets or sets answers for course
        /// </summary>
        public static Answers Answers { get; private set; }

        /// <summary>
        /// Represents full path of folder that represents the opened course
        /// </summary>
        public static string FullPath
        {
            get { return __FullPath; }
            set { __FullPath = value; }
        }
        /// <summary>
        /// Check is course saved and can be closed
        /// </summary>
        /// <returns>If course can be closed return value is true</returns>
        private static bool CanClose()
        {
            if (CourseClosing != null)
            {
                var e = new CancelEventArgs();
                CourseClosing(e);
                return !e.Cancel;
            }
            return true;
        }

        /// <summary>
        /// Open course represented as folder.
        /// </summary>
        /// <param name="courseFileName">Path to directory contains the course's files</param>
        /// <returns>If this course was opened return true</returns>
        public static bool Open([NotNull]string courseFileName)
        {
#if DEBUG
            try
            {
                FFDebug.EnterMethod(Cattegory.Course, string.Format("Path:{0}", courseFileName));
#endif
                if ((State & CourseStates.Opened) > 0 && !Close())
                {
                    return false;
                }

                State |= CourseStates.Opening;

                __FullPath = courseFileName;
                __Manifest = null;
                __Organization = null;
                __ContainedFileResource.Clear();

                string manifestFile = MapPath(MANIFEST_FILE_NAME);
                if (!File.Exists(manifestFile))
                {
                    ErrorDialog.ShowError("File you are trying to open is not a FireFly-compatible course. If you are sure it has been create using FireFly Course Editor, please send it to support of the Editor", courseFileName);
                    return false;
                }
                else
                {
                    using (var f = new FileStream(manifestFile, FileMode.Open))
                    {
#if LOGGER
                        using (Logger.Scope("Deserializing manifest"))
                        {
#endif                   
                            __Manifest = (ManifestType) ManifestType.Serializer.Deserialize(f);
#if LOGGER
                        }
                        using (Logger.Scope("Resolving tree"))
                        {
#endif
                            __Manifest.ResolveTree(__Manifest);
#if LOGGER
                        }
#endif

                    }
                    __Organization = __Manifest.organizations.Organizations[0];
                    Answers = CourseEditor.Course.Answers.FromFile(MapPath(ANSWERS_FILE_NAME));
                    if (__Manifest.version > CourseUpgradeManager.LastVersion)
                    {
                        State = CourseStates.OpenFailed;
                        Close();
                        State = CourseStates.None;
                        ErrorDialog.ShowError
                            ("Cannot open course because it was created by highest version of FireFly Course Editor than this one. Please, update program");
                    }

                    State = CourseStates.Opened;

                    string updRes;
                    if (CourseUpgradeManager.UpgradeCourse(out updRes))
                    {
                        if (CourseOpened != null)
                        {
                            CourseOpened();
                        }
                        return true;
                    }
                    else
                    {
                        State = CourseStates.UpgradeFailed;
                        Close();
                        State = CourseStates.None;
                        ErrorDialog.ShowError
                            ("Failed to convert course to actual version. Please sent it to our support." + Environment.NewLine +
                             "Upgrading details: " + Environment.NewLine + updRes);
                        return false;
                    }
                }
#if DEBUG
            }
            finally
            {
                FFDebug.LeaveMethod(Cattegory.Course, MethodBase.GetCurrentMethod());
            }
#endif
        }

        ///<summary>
        /// Checks is specified resources included in the course package and add them otherwise
        ///</summary>
        ///<param name="namespace">Namespace of resources</param>
        ///<param name="destination">Relative path to resources in course package</param>
        ///<param name="fileNames">List of file to check</param>
        ///<exception cref="FireFlyException"></exception>
        public static void EnsureCourseContainsResources([NotNull]string @namespace, [NotNull] string destination,
            params string[] fileNames)
        {
            var filePath = MapPath(destination);
            CreateDir(filePath);

            foreach (var fileName in fileNames)
            {
                if (!__ContainedFileResource.ContainsKey(fileName))
                {
                    var s = Assembly.GetEntryAssembly().GetManifestResourceStream(@namespace + fileName);
                    Debug.Assert(s != null);
                    var bts = new byte[s.Length];
                    s.Read(bts, 0, (int)s.Length);
                    File.WriteAllBytes(Path.Combine(filePath, fileName), bts);
                    __ContainedFileResource.Add(fileName, 1);
                }
                else
                {
                    __ContainedFileResource[fileName]++;
                }
            }
        }

        ///<summary>
        /// Releases resources located by <see cref="EnsureCourseContainsResources"/> method
        ///</summary>
        ///<param name="destination">Relative path to resources in course package</param>
        ///<param name="fileNames">List of file to release</param>
        ///<exception cref="FireFlyException"></exception>
        public static void ReleaseCourseResource([NotNull]string destination, params string[] fileNames)
        {
            string filePath = MapPath(destination);

            foreach (var fn in fileNames)
            {
                int count;
                if (!__ContainedFileResource.TryGetValue(fn, out count))
                {
                    throw new FireFlyException("Resource file '{0}' was not registered", fn);
                }
                if (count == 1)
                {
                    __ContainedFileResource.Remove(fn);
                    File.Delete(Path.Combine(filePath, fn));
                }
                else
                {
                    __ContainedFileResource[fn]--;
                }
            }
            if (Directory.GetFiles(Path.GetDirectoryName(filePath), "*", SearchOption.AllDirectories).Length == 0)
            {
                Directory.Delete(filePath);
            }
        }

        /// <summary>
        /// Open zip package that contains course's files
        /// </summary>
        /// <param name="fileName">If course is opened return true</param>
        public static bool OpenZipPackage([NotNull]string fileName)
        {
            FFDebug.EnterMethod(Cattegory.Course, string.Format("fileName:'{0}'", fileName));
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Zipper.ExtractZipFile(fileName, path);
            bool res = Open(path);
            if (res)
            {
                var lc = Settings.Default.LastCoursesXml;
                if (!lc.Contains(fileName))
                {
                    lc.Add(fileName);
                    if (lc.Count > 9)
                    {
                        lc.RemoveAt(0);
                    }
                    Settings.Default.LastCoursesXml = lc;
                }
            }
            
            FFDebug.LeaveMethod(Cattegory.Course, MethodBase.GetCurrentMethod());

            return res;
        }

        /// <summary>
        /// Save current course to zip package
        /// </summary>
        /// <param name="fileName">File name of zip package. If file not exists it will be created</param>
        public static bool SaveToZipPackage([NotNull]string fileName)
        {
            FFDebug.EnterMethod(Cattegory.Course, string.Format("fileName:'{0}'", fileName));

            if (Save() != true)
                return false;

            Zipper.CreateZip(fileName, __FullPath);
            var lc = Settings.Default.LastCoursesXml;
            if (!lc.Contains(fileName))
            {
                lc.Add(fileName);
                if (lc.Count > 9)
                {
                    lc.RemoveAt(0);
                }
                Settings.Default.LastCoursesXml = lc;
            }
            FFDebug.LeaveMethod(Cattegory.Course, MethodBase.GetCurrentMethod());
            return true;
        }

        /// <summary>
        /// Maps relative path to absolute
        /// </summary>
        /// <param name="path">Path to map</param>
        /// <returns>Mapped absolute path</returns>
        [NotNull]
        public static string MapPath([NotNull]string path)
        {
            return Path.Combine(FullPath, path);
        }

        /// <summary>
        /// Creates directory in course package.
        /// </summary>
        /// <param name="dir">Name of new folder</param>
        public static void CreateDir([NotNull]string dir)
        {
            Directory.CreateDirectory(MapPath(dir));
        }

        /// <summary>
        /// Close opened course
        /// </summary>
        /// <returns>If course was closed return false</returns>
        public static bool Close()
        {
            var failed = (State & (CourseStates.UpgradeFailed | CourseStates.OpenFailed)) > 0;
            if ((State & CourseStates.Opened) == 0 && !failed)
            {
                return true;
            }
            if (CanClose())
            {
                __Manifest = null;
                __Organization = null;
                Answers = null;
                var tempFullPath = __FullPath;
                __FullPath = null;
                if (!failed)
                {
                    HtmlPageBase.ReleasePages();
                    if (CourseClosed != null)
                    {
                        CourseClosed();
                    }
                }
                try
                {
                    Directory.Delete(tempFullPath, true);
                }
                catch (Exception e)
                {
                    ErrorDialog.ShowError("Error on deleting temporary files: " + e.Message);
                }

                __ContainedFileResource.Clear();

                State &= ~CourseStates.Opened;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Save all changes of current course to folder that represents is
        /// </summary>
        public static bool Save()
        {
            FFDebug.EnterMethod(Cattegory.Course, State.ToString());
            State |= CourseStates.Saving;

#if CHECKERS
            if (__Manifest == null)
            {
                throw new FireFlyException("Course is not assigned");
            }
            if (__Manifest.organizations.Organizations.Count == 0)
            {
                throw new FireFlyException("No organization assigned");
            }
#endif
            if (CourseSaving != null)
            {
                CourseSaving();
            }
            HtmlPageBase.StorePages();

            var cv = new CourseValidator();
            if (!cv.Validate())
            {
                ErrorDialog.ShowError("THIS COURSE IS INVALID!!! It cannot be played. Errors:" + Environment.NewLine + cv.GetErrorMessages());
                return false;
            }

            using (var f = new FileStream(MapPath(MANIFEST_FILE_NAME), FileMode.Create))
            {
                ManifestType.Serializer.Serialize(f, __Manifest, ManifestNamespaces.SerializerNamespaces);
            }
            Answers.SaveToFile(MapPath(ANSWERS_FILE_NAME));
            State &= ~CourseStates.Saving;
            State &= ~CourseStates.Modified;

            if (CourseSaved != null)
            {
                CourseSaved();
            }

            FFDebug.LeaveMethod(Cattegory.Course, MethodBase.GetCurrentMethod());

            return true;
        }

        /// <summary>
        /// Creates new course and opens it.
        /// </summary>
        public static void CreateNew()
        {
            FFDebug.EnterMethod(Cattegory.Course, State.ToString());

            if ((State & CourseStates.Opened) > 0 && !Close())
            {
                return;
            }

            __FullPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(__FullPath);
            __Manifest = new ManifestType("NewCourse") { version = CourseUpgradeManager.LastVersion };
            __Manifest.ResolveTree(__Manifest);
            __Organization = __Manifest.organizations.Organizations[0];
            __Manifest.organizations.@default = __Organization.identifier;
            Answers = new Answers();
            Save();
            if (Open(__FullPath))
            {
                State = CourseStates.Opened;
            }

            FFDebug.LeaveMethod(Cattegory.Course, MethodBase.GetCurrentMethod());
        }

        internal static void NotifyChanged()
        {
            if ((State & CourseStates.Modified) == 0)
            {
                State |= CourseStates.Modified;         
                if (CourseChanged != null)
                {
                    CourseChanged();
                }
            }
        }

        /// <summary>
        /// Notify course about manifest was changed. 
        /// </summary>
        /// <param name="node">Node of manifest that was changed</param>
        /// <param name="aNode">Instance of additional node (see documentation for <see cref="ManifestChangedEventArgs"/>)</param>
        /// <param name="changeType">Type of changed that was made</param>
        internal static void NotifyManifestChanged([NotNull]IManifestNode node, [NotNull] IManifestNode[] aNode,
            ManifestChangeTypes changeType)
        {
            NotifyChanged();
            if (ManifestChanged != null && (State & CourseStates.Saving) == 0)
            {
                ManifestChanged(new ManifestChangedEventArgs(node, aNode, changeType));
            }
        }

        /// <summary>
        /// Notify course about manifest was changed. 
        /// </summary>
        /// <param name="node">Node of manifest that was changed</param>
        /// <param name="changeType">Type of occurred changes</param>
        internal static void NotifyManifestChanged([NotNull] IManifestNode node, ManifestChangeTypes changeType)
        {
            NotifyChanged();
            if (ManifestChanged != null && (State & CourseStates.Saving) == 0)
            {
                ManifestChanged(new ManifestChangedEventArgs(node, null, changeType));
            }
        }

        ///// <summary>
        ///// Occurs on manifest changing
        ///// </summary>
        //public static event ManifestChangingDelegate ManifestChanging;

        /// <summary>
        /// Occurs after manifest changed
        /// </summary>
        public static event Action<ManifestChangedEventArgs> ManifestChanged;

        /// <summary>
        /// Occurs when course opened
        /// </summary>
        public static event Action CourseOpened;

        /// <summary>
        /// Occurs on closing course
        /// </summary>
        public static event Action<CancelEventArgs> CourseClosing;

        /// <summary>
        /// Occurs when course was closed
        /// </summary>
        public static event Action CourseClosed;

        ///<summary>
        /// Occurs before course started to saving
        ///</summary>
        public static event Action CourseSaving;

        ///<summary>
        /// Raises when course is saved
        ///</summary>
        public static event Action CourseSaved;

        ///<summary>
        /// Raises when course change
        ///</summary>
        public static event Action CourseChanged;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string __FullPath;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ManifestType __Manifest;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static OrganizationType __Organization;
    }

    namespace Manifest
    {
        ///<summary>
        /// Type of change
        ///</summary>
        [Flags]
        public enum ManifestChangeTypes
        {
            Changed = 1,
            ChildrenAdded = 2,
            ChildrenRemoved = 4,
            ChildrenReordered = 8
        }

        ///<summary>
        ///
        ///</summary>
        public class ManifestChangingEventArgs : CancelEventArgs
        {
            /// <summary>
            /// </summary>
            /// <param name="node">Manifest node that will be changed</param>
            public ManifestChangingEventArgs([NotNull]IManifestNode node)
            {
                ManifestNode = node;
            }

            /// <summary>
            /// Represents the node of manifest that will be changed
            /// </summary>
            public IManifestNode ManifestNode { get; set; }
        }

        ///<summary>
        /// Arguments about Manifest changed events
        ///</summary>
        public class ManifestChangedEventArgs : EventArgs
        {
            ///<summary>
            ///  Additional nodes
            ///</summary>
            public readonly IManifestNode[] Nodes;

            ///<summary>
            ///  Node that was changed
            ///</summary>
            public readonly IManifestNode ChangedNode;

            ///<summary>
            ///  Type of change
            ///</summary>
            public readonly ManifestChangeTypes ChangeType;

            ///<summary>
            ///  Creates new instance of <c>ManifestChangeEventArgs</c>
            ///</summary>
            ///<param name="changedNode"></param>
            ///<param name="nodes"></param>
            ///<param name="changeType"></param>
            public ManifestChangedEventArgs([NotNull]IManifestNode changedNode, [NotNull]IManifestNode[] nodes, ManifestChangeTypes changeType)
            {
                ChangedNode = changedNode;
                ChangeType = changeType;
                Nodes = nodes;
            }
        }
    }

    ///<summary>
    /// Bit mask of states of <see cref="Course"/> 
    ///</summary>
    [Flags]
    public enum CourseStates
    {
        None = 0,
        Opened = 1,
        Modified = 2,
        Saving = 4,
        Opening = 8,
        UpgradeFailed = 16,
        OpenFailed = 32
    }
}
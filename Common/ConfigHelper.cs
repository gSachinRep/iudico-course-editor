namespace FireFly.CourseEditor.Common
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Course.Manifest;
    using Properties;

    /// <summary>
    /// This class provides help methods to interaction with configuration options;
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// Restore saved location to Window
        /// </summary>
        /// <param name="window">Window that location should be restored</param>
        public static void RestoreLocation([NotNull]Form window)
        {
            window.Location = (Point) Settings.Default[window.Name + "_Location"];
        }

        /// <summary>
        /// Restore full state of some window
        /// </summary>
        /// <param name="window">Window that settings should be restored</param>
        public static void RestoreWindowSettings([NotNull]Form window)
        {
            string objName = window.Name;
            window.Visible = (bool)Settings.Default[objName + "_Visible"];
            window.WindowState = (FormWindowState)Settings.Default[objName + "_WindowState"];
            RestoreLocation(window);
            window.Size = (Size)Settings.Default[objName + "_Size"];
        }

        /// <summary>
        /// Save location of some window to restored in a future
        /// </summary>
        /// <param name="window">Window that location should be saved</param>
        public static void SaveLocation([NotNull]Form window)
        {
            if (window.WindowState == FormWindowState.Normal)
                Settings.Default[window.Name + "_Location"] = window.Location;
            else
                Settings.Default[window.Name + "_Location"] = window.RestoreBounds.Location;
        }

        /// <summary>
        /// Save all settings of some window to restored in a future
        /// </summary>
        /// <param name="window">Window that settings should be saved</param>
        public static void SaveWindowSettings([NotNull]Form window)
        {
            string objName = window.Name;
            Settings.Default[objName + "_Visible"] = window.Visible;
            Settings.Default[objName + "_WindowState"] = window.WindowState;
            Settings.Default[objName + "_Size"] = window.Size;
            SaveLocation(window);
        }

        /// <summary>
        /// Gets the default title to created item related of it's type
        /// </summary>
        /// <param name="type">Type of item</param>
        /// <returns>Default title</returns>
        public static string GetDefaultItemTitle(PageType type)
        {
            switch (type)
            {
                case PageType.Theory:
                    return Settings.Default.DefaultTheoryItemTitle;
                case PageType.Question:
                    return Settings.Default.DefaultQuestionItemTitle;
                case PageType.Summary:
                    return Settings.Default.DefaultSummaryItemTitle;

                default:
                    throw new FireFlyException("Cannot get title for '{0}'", type);
            }
        }
    }
}
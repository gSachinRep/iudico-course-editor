using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;

namespace FireFly.CourseEditor.Common
{
    using Properties;

    [BindProperty("Settings", "Options_SaveRestoreWindowsState", Category = "Main",
      DisplayName = @"Save/Restore state of windows",
      Description = "Determine should the application save state of all windows on exit and restore it on startup")]
    [BindProperty("Settings", "Options_ShowToolBar", Category = "Main",
      DisplayName = @"Show tool-bar on main form",
      Description = "Determine is the tool bar on main form visible")]
    [BindProperty("Settings", "Options_EnableLMSEmulation", Category = "Main",
      DisplayName = "LMS emulator", Description = "Enables/disables integrated LMS emulator")]
    [BindProperty("Settings", "Options_PlainTextEnabled", Category = "Main",
      DisplayName = "Plain Text Enabled", Description = "Determine is the plain text tab visible in CourseDesigner")]
    public class FFConfig
    {
        static FFConfig()
        {                        
            __Settings = Settings.Default;
        }                                        

        [NotNull]
        public static readonly FFConfig Instance = new FFConfig();

        private static readonly Settings __Settings;

// ReSharper disable UnusedPrivateMember
        private static Settings Settings  // This method uses with BindProperty attributes
// ReSharper restore UnusedPrivateMember
        {
            get { return __Settings; }
        }

        ///<summary>
        /// Reset all properties to it's default values
        ///</summary>
        public void ResetDefaults()
        {
            // This method depends of settings dialog implementation !!!
            Type type = GetType();
            foreach (BindPropertyAttribute bp in type.GetCustomAttributes<BindPropertyAttribute>())
            {
                PropertyInfo sourceProperty = bp.GetSourcePropertyInfo(type);
                PropertyInfo bindedProperty = bp.GetBindedPropertyInfo(sourceProperty);
                string defaultValue = bindedProperty.GetCustomAttribute<DefaultSettingValueAttribute>().Value;
                object typedDefaultValue = ((IConvertible)defaultValue).ToType(bindedProperty.PropertyType, CultureInfo.CurrentCulture);
                object sourcePropertyValue = sourceProperty.GetValue(this, null);

                bindedProperty.SetValue(sourcePropertyValue, typedDefaultValue, null);
            }
        }
    }
}
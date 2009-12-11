using System;
using System.Diagnostics;
using System.Xml;
using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlStyleAttribute = System.Web.UI.HtmlTextWriterStyle;
using System.Drawing;
using System.Windows.Forms;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Common;

    ///<summary>
    /// Elements of instance of <see cref="HtmlControl"/> that marked to read-write to/from html code on parsing/writing 
    ///</summary>
    [Flags]
    public enum SerializeElems
    {
        None = 0,
        Position = 1,
        Size = 2,
        Font = 4,
        ALL = Position | Size | Font
    }

    ///<summary>
    /// Attribute to mark which properties of <see cref="HtmlControl"/> store in html code
    ///</summary>
    [AttributeUsage(AttributeTargets.Class)]
    [DebuggerDisplay("{SerializeElems}")]
    [BaseTypeRequiredAttribute(typeof(HtmlControl))]
    public class HtmlSerializeSettingsAttribute : Attribute
    {
        ///<summary>
        /// Elements to serialize
        ///</summary>
        public readonly SerializeElems SerializeElems;

        ///<summary>
        /// Creates new instance of <see cref="HtmlSerializeSettingsAttribute"
        ///</summary>
        ///<param name="serializeElems">Elements to serialize</param>
        public HtmlSerializeSettingsAttribute(SerializeElems serializeElems)
        {
            SerializeElems = serializeElems;
        }
    }

    ///<summary>
    /// Helper class to read-write common attributes from/to html code
    ///</summary>
    public static class HtmlSerializeHelper
    {
        //static HtmlSerializeHelper()
        //{
        //    var t = typeof(HtmlSerializeHelper);
        //    addSize = t.GetMethod("AddSizeAttributes", BindingFlags.Static);
        //    addPosition = t.GetMethod("AddPositionAttributes", BindingFlags.Static);
        //    addFont = t.GetMethod("AddFontAttributes", BindingFlags.Static);
        //    readSize = t.GetMethod("ReadSizeAttributes", BindingFlags.Static);
        //    readPosition = t.GetMethod("ReadPositionAttributes", BindingFlags.Static);
        //    readFont = t.GetMethod("ReadFontAttributes", BindingFlags.Static);
        //}

        //public static MethodInfo addSize, addPosition, addFont, readSize, readFont, readPosition;

        ///<summary>
        /// Adds size attributes of <see cref="c"> to <see cref="w" />
        ///</summary>
        ///<param name="c">Control which property should be stored</param>
        ///<param name="w">Writer should be used to store properties</param>
        public static void AddSizeAttributes([NotNull]Control c, [NotNull]HtmlWriter w)
        {
            w.AddStyleAttribute(HtmlStyleAttribute.Width, c.Width.ToString());
            w.AddStyleAttribute(HtmlStyleAttribute.Height, c.Height.ToString());
        }

        ///<summary>
        /// Adds position attributes of <see cref="c"> to <see cref="w" />
        ///</summary>
        ///<param name="c">Control which property should be stored</param>
        ///<param name="w">Writer should be used to store properties</param>
        public static void AddPositionAttributes([NotNull]Control c, [NotNull]HtmlWriter w)
        {
            w.AddStyleAttribute(HtmlStyleAttribute.Left, c.Left.ToString());
            w.AddStyleAttribute(HtmlStyleAttribute.Top, c.Top.ToString());
        }

        ///<summary>
        /// Adds font attributes to <see cref="w" />
        ///</summary>
        ///<param name="f">Font should be written to <see cref="w"></param>
        ///<param name="w">Writer should be used to store properties</param>
        public static void AddFontAttributes([NotNull]Font f, [NotNull]HtmlWriter w)
        {
            w.AddStyleAttribute(HtmlStyleAttribute.FontFamily, f.FontFamily.Name);
            w.AddStyleAttribute(HtmlStyleAttribute.FontSize, f.Size + (f.Unit == GraphicsUnit.Pixel ? "px" : "pt"));
            if (f.Bold)
            {
                w.AddStyleAttribute(HtmlStyleAttribute.FontWeight, "bold");
            }
            if (f.Italic)
            {
                w.AddStyleAttribute(HtmlStyleAttribute.FontStyle, "italic");
            }
        }

        ///<summary>
        /// Reads positions attributes from <see cref="styles" /> and update <see cref="c" />
        ///</summary>
        ///<param name="c">Control to update</param>
        ///<param name="styles">Instance of <see cref="HtmlStyleReader"/></param>
        public static void ReadPositionAttributes([NotNull]Control c, [NotNull]HtmlStyleReader styles)
        {
            c.Location = new Point(int.Parse(styles[HtmlStyleAttribute.Left]), int.Parse(styles[HtmlStyleAttribute.Top]));
        }

        ///<summary>
        /// Reads size attributes from <see cref="styles" /> and update <see cref="c" />
        ///</summary>
        ///<param name="c">Control to update</param>
        ///<param name="styles">Instance of <see cref="HtmlStyleReader"/></param>
        public static void ReadSizeAttributes([NotNull]Control c, [NotNull]HtmlStyleReader styles)
        {
            c.Size = new Size(int.Parse(styles[HtmlStyleAttribute.Width]), int.Parse(styles[HtmlStyleAttribute.Height]));
        }

        ///<summary>
        /// Reads font attributes from <see cref="styles" /> and update <see cref="c" />
        ///</summary>
        ///<param name="c">Control to update</param>
        ///<param name="styles">Instance of <see cref="HtmlStyleReader"/></param>
        public static void ReadFontAttributes([NotNull]Control c, [NotNull]HtmlStyleReader styles)
        {
            var size = styles[HtmlStyleAttribute.FontSize];
            var u = size.EndsWith("pt") ? GraphicsUnit.Point : GraphicsUnit.Pixel;
            var fs = FontStyle.Regular;
            if (styles[HtmlStyleAttribute.FontWeight] == "bold")
            {
                fs |= FontStyle.Bold;
            }
            if (styles[HtmlStyleAttribute.FontStyle] == "italic")
            {
                fs |= FontStyle.Italic;
            }
            size = size.Remove(size.Length - 2);
            c.Font = new Font(styles[HtmlStyleAttribute.FontFamily], float.Parse(size), fs, u);
        }
    }

    ///<summary>
    /// Typed helper to read-write common properties of controls
    ///</summary>
    ///<typeparam name="TControl">Type of control</typeparam>
    [StartupInitializable(typeof(HtmlButton), typeof(HighlightControl.HtmlHighlightedCode), typeof(HtmlCodeSnippet), typeof(HtmlComboBox), typeof(HtmlCompiledTest), typeof(HtmlLabel), typeof(HtmlSimpleQuestion), typeof(HtmlTextBox))]
    public static class HtmlSerializeHelper<TControl>
        where TControl : HtmlControl
    {
        private static bool __Size, __Font, __Position, __Initialized;

        ///<summary>
        /// Initializes class
        /// <seealso cref="StartupInitializableAttribute"/>
        ///</summary>
        public static void Initialize()
        {
            Debug.Assert(!__Initialized);
            SerializeElems se = typeof(TControl).GetCustomAttribute<HtmlSerializeSettingsAttribute>().SerializeElems;
            __Size = (se & SerializeElems.Size) > 0;
            __Font = (se & SerializeElems.Font) > 0;
            __Position = (se & SerializeElems.Position) > 0;
            __Initialized = true;
        }

        ///<summary>
        /// Writes attributes for root element of control
        ///</summary>
        ///<param name="w">Instance of <see cref="System.Web.UI.HtmlTextWriter"/> should be used</param>
        ///<param name="c">Instance of control which configuration should be written</param>
        public static void WriteRootElementAttributes([NotNull]HtmlWriter w, [NotNull] TControl c)
        {
            Debug.Assert(__Initialized);
            // TODO: Convert it to expression and then compile it
            if (__Font)
            {
                HtmlSerializeHelper.AddFontAttributes(c.Control.Font, w);
            }
            if (__Position)
            {
                HtmlSerializeHelper.AddPositionAttributes(c.Control, w);
            }
            if (__Size)
            {
                HtmlSerializeHelper.AddSizeAttributes(c.Control, w);
            }
        }

        ///<summary>
        ///  Reads attributes of root element of control
        ///</summary>
        ///<param name="node">Node of html</param>
        ///<param name="c">Control that should be retrieved based on <see cref="node"/></param>
        public static void ReadRootElementAttributes([NotNull]XmlNode node, [NotNull]TControl c)
        {
            Debug.Assert(__Initialized);
            // TODO: Convert it to expression and compile it
            var r = new HtmlStyleReader(node);
            if (__Font)
            {
                HtmlSerializeHelper.ReadFontAttributes(c.Control, r);
            }
            if (__Position)
            {
                HtmlSerializeHelper.ReadPositionAttributes(c.Control, r);
            }
            if (__Size)
            {
                HtmlSerializeHelper.ReadSizeAttributes(c.Control, r);
            }
        }
    }

    [HtmlSerializeSettings(SerializeElems.Position)]
    public partial class HtmlControl
    {
        /// <summary>
        /// Raises when control has been parsed from html
        /// </summary>
        public event Action Parsed;

        ///<summary>
        /// Writes Html code to <see cref="System.Web.UI.HtmlTextWriter"/>
        ///</summary>
        ///<param name="w"></param>
        public virtual void WriteHtml([NotNull]HtmlWriter w)
        {
            w.AddAttribute(HtmlAttribute.Id, Name);
            w.AddStyleAttribute(HtmlStyleAttribute.Position, "absolute");
        }

        /// <summary>
        /// Parses html node
        /// </summary>
        /// <param name="node">Html node</param>
        protected virtual void Parse([NotNull]XmlNode node)
        {
            Name = node.Attributes["id"].Value;
        }
    }
}

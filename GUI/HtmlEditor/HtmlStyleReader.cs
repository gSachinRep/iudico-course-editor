using HtmlStyleAttribute = System.Web.UI.HtmlTextWriterStyle;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class HtmlStyleReader : Dictionary<string, string>
    {
        /// <summary>
        /// Creates new instance of HtmlStyleReader based on styles styles parameter contains
        /// </summary>
        /// <param name="styles">String representation of styles</param>
        public HtmlStyleReader(string styles)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            string[] items = styles.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string i in items)
            {
                int ind = i.IndexOf(':');
                string name = i.Remove(ind).Trim();
                string value = i.Substring(ind + 1).Trim();
                Add(name, value);
            }
        }

        /// <summary>
        /// Creates new instance of HtmlStyleReader based on style attribute from specified node
        /// </summary>
        /// <param name="node">Html node contains style attribute</param>
        public HtmlStyleReader(XmlNode node)
            : this(node.Attributes["style"].Value)
        {
        }

        public new string this[string name]
        {
            get
            {
                if (ContainsKey(name))
                    return base[name];
                else
                    return string.Empty;
            }
        }

        public string this[HtmlStyleAttribute style]
        {
            get
            {
                switch (style)
                {
                    case HtmlStyleAttribute.Height:
                    case HtmlStyleAttribute.Left:
                    case HtmlStyleAttribute.Top:
                    case HtmlStyleAttribute.Width:
                    case HtmlStyleAttribute.Position:
                    case HtmlStyleAttribute.Color:
                    case HtmlStyleAttribute.Cursor:
                    case HtmlStyleAttribute.Direction:
                    case HtmlStyleAttribute.Display:
                    case HtmlStyleAttribute.Filter:
                    case HtmlStyleAttribute.Margin:
                    case HtmlStyleAttribute.Padding:
                    case HtmlStyleAttribute.Visibility:
                    case HtmlStyleAttribute.ZIndex:
                    case HtmlStyleAttribute.Overflow:
                        return this[style.ToString()];

                    case HtmlStyleAttribute.FontFamily:
                        return this["font-family"];
                    case HtmlStyleAttribute.FontSize:
                        return this["font-size"];
                    case HtmlStyleAttribute.FontStyle:
                        return this["font-style"];
                    case HtmlStyleAttribute.FontVariant:
                        return this["font-variant"];
                    case HtmlStyleAttribute.FontWeight:
                        return this["font-weight"];

                    case HtmlStyleAttribute.BackgroundColor:
                        return this["background-color"];
                    case HtmlStyleAttribute.BackgroundImage:
                        return this["background-image"];
                    case HtmlStyleAttribute.BorderCollapse:
                        return this["border-collapse"];
                    case HtmlStyleAttribute.BorderColor:
                        return this["border-color"];
                    case HtmlStyleAttribute.BorderStyle:
                        return this["border-style"];
                    case HtmlStyleAttribute.BorderWidth:
                        return this["border-width"];
                    case HtmlStyleAttribute.ListStyleImage:
                        return this["list-style-image"];
                    case HtmlStyleAttribute.ListStyleType:
                        return this["list-style-type"];
                    case HtmlStyleAttribute.MarginBottom:
                        return this["margin-bottom"];
                    case HtmlStyleAttribute.MarginLeft:
                        return this["margin-left"];
                    case HtmlStyleAttribute.MarginRight:
                        return this["margin-right"];
                    case HtmlStyleAttribute.MarginTop:
                        return this["margin-top"];
                    case HtmlStyleAttribute.OverflowX:
                        return this["overflow-x"];
                    case HtmlStyleAttribute.OverflowY:
                        return this["overflow-y"];
                    case HtmlStyleAttribute.PaddingBottom:
                        return this["padding-bottom"];
                    case HtmlStyleAttribute.PaddingLeft:
                        return this["padding-left"];
                    case HtmlStyleAttribute.PaddingRight:
                        return this["padding-right"];
                    case HtmlStyleAttribute.PaddingTop:
                        return this["padding-top"];
                    case HtmlStyleAttribute.TextAlign:
                        return this["text-align"];
                    case HtmlStyleAttribute.TextDecoration:
                        return this["text-decoration"];
                    case HtmlStyleAttribute.TextOverflow:
                        return this["text-overlow"];
                    case HtmlStyleAttribute.VerticalAlign:
                        return this["vertical-align"];
                    case HtmlStyleAttribute.WhiteSpace:
                        return this["white-space"];

                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}
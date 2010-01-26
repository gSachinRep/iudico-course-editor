using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.UI;
using HtmlHighlightedCode=FireFly.CourseEditor.GUI.HtmlEditor.HighlightControl.HtmlHighlightedCode;
using Control = System.Windows.Forms.Control;
using HtmlAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlStyleAttribute = System.Web.UI.HtmlTextWriterStyle;
using HtmlTag = System.Web.UI.HtmlTextWriterTag;

namespace FireFly.CourseEditor.GUI.HtmlEditor.AdvancedCompiledTestControl
{
  using Common;
  using Course;

  /// <summary>
  /// Represents control that allows user to add code to compile.Common case:
  /// [code added by lector]
  /// [code added by user]
  /// [code added by lector]
  /// </summary>
  [HtmlSerializeSettings(SerializeElems.ALL)]
  class HtmlAdvancedCompiledTest : HtmlCompiledTest//, IJavaScriptInitializable
  {
    /*
    private HtmlHighlightedCode.LANGUAGE? _LanguageToHighlight;

    ///<summary>
    /// Programming language to highlight
    ///</summary>
    public HtmlHighlightedCode.LANGUAGE? LanguageToHighlight
    {
      get { return _LanguageToHighlight; }
      set
      {
          if (_LanguageToHighlight != value)
          {
              if (_LanguageToHighlight != null)
              {
                  ReleaseFileResources();
              }
              _LanguageToHighlight = value;
              if (value != null)
              {
                  RemoveError(LANG_IS_NULL_ERROR);
                  GetFileResources();
              }
          }
          ReValidate();
      }
    }

    private const string HIGHLIGHT_JS_FILENAME = "highlight.js",
                         SAMPLE_CSS_FILENAME = "sample.css",
                         HIGHLIGHT_DIRECTORY = "HighlightLanguages", LANG_IS_NULL_ERROR = "'Language' property must be specified";
    */

    public override string GetScoTestInitializer()
    {
      string input = "[";
      string output = "[";

      for (int i = 0; i < TestCases.Count; ++i)
      {
        CompiledTestCase item = TestCases[i];
        input += item.Input;
        output += item.Output;

        if (i != TestCases.Count - 1)
        {
          input += ",";
          output += ",";
        }
      }
      input += "]";
      output += "]";

      string result = string.Format("new compiledTest('TextBoxBefore', 'TextBoxAfter', 'TextBoxUserCode', '{0}', '{1}', {2}, {3}, {4}, {5}",
          ServiceAddress, CompiledQuestion.GetLanguageString(Language), TimeLimit, MemoryLimit, input, output);


      result += ")";

      return result;
    }

    protected override Control CreateWindowControl()
    {
      return new AdvancedCompiledTest();
    }

    public override void WriteHtml(System.Web.UI.HtmlTextWriter w)
    {
      /*
      //set code language
      if (Language == CompiledQuestion.LANGUAGE.CPP)
      {
        LanguageToHighlight = HtmlHighlightedCode.LANGUAGE.Cpp;
      }
      else if (Language == CompiledQuestion.LANGUAGE.CS)
      {
        LanguageToHighlight = HtmlHighlightedCode.LANGUAGE.Cpp;
      }
      else if (Language == CompiledQuestion.LANGUAGE.Delphi)
      {
        LanguageToHighlight = HtmlHighlightedCode.LANGUAGE.Delphi;
      }
      else if (Language == CompiledQuestion.LANGUAGE.Java)
      {
        LanguageToHighlight = HtmlHighlightedCode.LANGUAGE.Java;
      }
       * */

      //<div>
      w.AddAttribute(HtmlAttribute.Id, Name);
      w.AddAttribute(HtmlAttribute.Name, "advancedCompiledTest");
      w.AddStyleAttribute(HtmlStyleAttribute.Position, "absolute");
      HtmlSerializeHelper<HtmlCompiledTest>.WriteRootElementAttributes(w, this);
      w.RenderBeginTag(HtmlTag.Div);

      //var ls = LanguageToHighlight.ToString().ToLower();

      //<span name="BeforeCode">//there will be lector code
      w.AddAttribute(HtmlAttribute.Id, "TextBoxBefore");
      w.AddStyleAttribute(HtmlStyleAttribute.Overflow, "scroll");
      w.AddStyleAttribute(HtmlStyleAttribute.Height, (Control as AdvancedCompiledTest).TextBoxBefore.Height.ToString());
      w.AddStyleAttribute(HtmlStyleAttribute.Width, (Control as AdvancedCompiledTest).TextBoxBefore.Width.ToString());
      w.RenderBeginTag(HtmlTag.Span);
      //w.AddAttribute(HtmlAttribute.Class, ls);
      //w.WriteFullBeginTag(string.Concat("pre><code class=\"", ls, "\""));
      w.WriteFullBeginTag(string.Concat("pre><code"));
      w.Write((Control as AdvancedCompiledTest).TextBoxBefore.Text.HttpEncode());
      w.WriteFullBeginTag("/code></pre");
      w.RenderEndTag();
      //</span>

      //<textarea>//there will be user code
      w.AddAttribute(HtmlAttribute.Id, "TextBoxUserCode");
      w.AddStyleAttribute(HtmlStyleAttribute.Width, (Control as AdvancedCompiledTest).TextBoxUserCode.Width.ToString());
      w.AddStyleAttribute(HtmlStyleAttribute.Height, (Control as AdvancedCompiledTest).TextBoxUserCode.Height.ToString());
      w.RenderBeginTag(HtmlTextWriterTag.Textarea);
      w.RenderEndTag();
      //</textarea>

      //<span name="AfterCode">//there will be lector code
      w.AddAttribute(HtmlAttribute.Id, "TextBoxAfter");
      w.AddStyleAttribute(HtmlStyleAttribute.Overflow, "scroll");
      w.AddStyleAttribute(HtmlStyleAttribute.Height, (Control as AdvancedCompiledTest).TextBoxAfter.Height.ToString());
      w.AddStyleAttribute(HtmlStyleAttribute.Width, (Control as AdvancedCompiledTest).TextBoxAfter.Width.ToString());
      w.RenderBeginTag(HtmlTag.Span);
      //w.AddAttribute(HtmlAttribute.Class, ls);
      //w.WriteFullBeginTag(string.Concat("pre><code class=\"", ls, "\""));
      w.WriteFullBeginTag(string.Concat("pre><code"));
      w.Write((Control as AdvancedCompiledTest).TextBoxAfter.Text.HttpEncode());
      w.WriteFullBeginTag("/code></pre");
      w.RenderEndTag();
      //</span>

      w.RenderEndTag();
      //</div>
    }

    protected override void Parse(XmlNode node)
    {
      base.Parse(node);
      Control.Text = "";
      (Control as AdvancedCompiledTest).TextBoxBefore.Text = node.ChildNodes[0].InnerText;//.HttpDecode();
      (Control as AdvancedCompiledTest).TextBoxAfter.Text = node.ChildNodes[2].InnerText;//.HttpDecode();
      (Control as AdvancedCompiledTest).AdvancedCompiledTest_Resize(null, null);
    }
    
    /*
    #region Java
    [CanBeNull]
    public string GetJavaScriptInitializer()
    {
      var page = GetParentPage();
      if (/*Control.Text.IsNotNull()((Control as AdvancedCompiledTest).TextBoxBefore.Text.IsNotNull() || (Control as AdvancedCompiledTest).TextBoxAfter.Text.IsNotNull()) 
          && LanguageToHighlight != null && ((page.InitializedLanguages & LanguageToHighlight) == 0))
      {
        page.InitializedLanguages |= LanguageToHighlight.Value;
        return string.Format("initHighlightingOnLoad('{0}');", (LanguageToHighlight.ToString().ToLower()));
      }
      return null;
    }

    private void ReleaseFileResources()
    {
      Course.ReleaseCourseResource(HIGHLIGHT_DIRECTORY, HIGHLIGHT_JS_FILENAME, SAMPLE_CSS_FILENAME, LanguageToHighlight.ToString().ToLower() + ".js");
      HtmlPageBase page = GetParentPage();
      page.Scripts.RemoveAllWithRoot(HIGHLIGHT_DIRECTORY, HIGHLIGHT_JS_FILENAME);
      page.Styles.RemoveAllWithRoot(HIGHLIGHT_DIRECTORY, SAMPLE_CSS_FILENAME);
    }

    private void GetFileResources()
    {
      HtmlPageBase page = GetParentPage();
      page.Scripts.IncludeAllWithRoot(HIGHLIGHT_DIRECTORY, HIGHLIGHT_JS_FILENAME);
      page.Styles.IncludeAllWithRoot(HIGHLIGHT_DIRECTORY, SAMPLE_CSS_FILENAME);
      Course.EnsureCourseContainsResources(Course.LANGUAGES_NAMESPACE, HIGHLIGHT_DIRECTORY, HIGHLIGHT_JS_FILENAME, SAMPLE_CSS_FILENAME, LanguageToHighlight.ToString().ToLower() + ".js");
    }
    #endregion
*/
  }
}

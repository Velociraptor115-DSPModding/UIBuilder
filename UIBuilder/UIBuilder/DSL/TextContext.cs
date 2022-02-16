using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;
public static partial class UIBuilderDSL
{
  public record TextContext : UIElementContextBase<TextContext>
  {
    public TextContext(GameObject uiElement) : base(uiElement)
    {
      text = uiElement.GetOrCreateComponent<Text>();
      text.font = UIBuilder.fontSAIRASB;
      text.alignment = TextAnchor.MiddleCenter;
      text.verticalOverflow = VerticalWrapMode.Overflow;
    }

    public readonly Text text;
    public override TextContext Context => this;

    public TextContext WithLocalizer(string value)
    {
      var localizer = uiElement.GetOrCreateComponent<Localizer>();
      localizer.stringKey = value;
      localizer.Refresh();
      
      return Context;
    }

    public TextContext WithFont(Font font)
    {
      text.font = font;
      return Context;
    }
    
    public TextContext WithFontSize(int fontSize)
    {
      text.fontSize = fontSize;
      return Context;
    }

    public TextContext WithText(string value)
    {
      text.text = value;
      return Context;
    }

    public TextContext WithAlignment(TextAnchor alignment)
    {
      text.alignment = alignment;
      return Context;
    }
  }
}
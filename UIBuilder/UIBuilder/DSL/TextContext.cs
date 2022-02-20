using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;
public static partial class UIBuilderDSL
{
  public record TextContext : GraphicContextBase<TextContext>
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
    protected override Graphic graphic => text;

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
    
    public TextContext WithFontSize(int fontSize, int? minSize = null, int? maxSize = null)
    {
      text.fontSize = fontSize;
      if (minSize is null && maxSize is null)
        return Context;

      text.resizeTextForBestFit = true;
      if (minSize.HasValue)
        text.resizeTextMinSize = minSize.Value;
      if (maxSize.HasValue)
        text.resizeTextMaxSize = maxSize.Value;
      
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

    public TextContext WithOverflow(HorizontalWrapMode? horizontal = null, VerticalWrapMode? vertical = null)
    {
      if (horizontal.HasValue)
        text.horizontalOverflow = horizontal.Value;
      if (vertical.HasValue)
        text.verticalOverflow = vertical.Value;
      return Context;
    }
  }
}
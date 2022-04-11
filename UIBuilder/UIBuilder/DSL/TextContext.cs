using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface ITextContext : IGraphicContext
{
  Text text { get; }
}

public record TextContext(Text text) : GraphicContext(text.gameObject), ITextContext
{
  public readonly Text text = text;
  public override Graphic graphic => text;
  Text ITextContext.text => text;
}

public static class TextContextExtensions
{
  public static TextContext Create(GameObject uiElement)
  {
    var text = uiElement.GetOrCreateComponent<Text>();
    text.font = UIBuilder.fontSAIRASB;
    text.alignment = TextAnchor.MiddleCenter;
    text.verticalOverflow = VerticalWrapMode.Overflow;
    return Select(text);
  }

  public static TextContext Select(Text text)
    => new TextContext(text);
}

public static class ITextContextExtensions
{
  public static T WithLocalizer<T>(this T Context, string value)
    where T : ITextContext
  {
    var localizer = Context.text.gameObject.GetOrCreateComponent<Localizer>();
    localizer.stringKey = value;
    localizer.Refresh();

    return Context;
  }

  public static T WithFont<T>(this T Context, Font font)
    where T : ITextContext
  {
    Context.text.font = font;
    return Context;
  }

  public static T WithFontSizeExact<T>(this T Context, int fontSize)
    where T : ITextContext
  {
    Context.text.fontSize = fontSize;
    Context.text.resizeTextForBestFit = false;
      return Context;
  }

  public static T WithFontSize<T>(this T Context, int fontSize, int? minSize = null, int? maxSize = null)
    where T : ITextContext
  {
    Context.text.fontSize = fontSize;
    Context.text.resizeTextForBestFit = true;
    Context.text.resizeTextMinSize = minSize ?? 10;
    Context.text.resizeTextMaxSize = maxSize ?? fontSize;
    return Context;
  }

  public static T WithText<T>(this T Context, string value)
    where T : ITextContext
  {
    Context.text.text = value;
    return Context;
  }

  public static T WithAlignment<T>(this T Context, TextAnchor alignment)
    where T : ITextContext
  {
    Context.text.alignment = alignment;
    return Context;
  }

  public static T WithOverflow<T>(this T Context, HorizontalWrapMode? horizontal = null, VerticalWrapMode? vertical = null)
    where T : ITextContext
  {
    if (horizontal.HasValue)
      Context.text.horizontalOverflow = horizontal.Value;
    if (vertical.HasValue)
      Context.text.verticalOverflow = vertical.Value;
    return Context;
  }
}
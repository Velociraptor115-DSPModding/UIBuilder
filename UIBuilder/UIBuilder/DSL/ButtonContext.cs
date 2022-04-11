using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface IButtonSelectableContext : ISelectableContext
{
  Button button { get; }
}

public record ButtonContext(Button button, Text text) : UIElementContext(button.gameObject), IButtonSelectableContext, ITextContext, ISelectableVisuals
{
  public Graphic visuals { get; set; }
  Text ITextContext.text => text;
  Graphic IGraphicContext.graphic => text;
  Button IButtonSelectableContext.button => button;
  Selectable ISelectableContext.selectable => button;
}

public static class ButtonContextExtensions
{
  public static ButtonContext Select(Button button, Text text = null, Graphic visuals = null)
    => new(button, text) { visuals = visuals };

  public static ButtonContext Create(GameObject buttonObj, GameObject textObj)
  {
    var button = buttonObj.GetOrCreateComponent<Button>();
    var text = textObj.GetOrCreateComponent<Text>();
    return Select(button, text);
  }

  public static ButtonContext Create(GameObject buttonObj)
    => Select(buttonObj.GetOrCreateComponent<Button>());

  public static ButtonContext Create(GameObject buttonObj, string buttonText)
  {
    var button = buttonObj.GetOrCreateComponent<Button>();
    return Create(button, buttonText);
  }
  
  public static ButtonContext Create(Button button, string buttonText)
  {
    var text =
      UIBuilderDSL.Create.Text("text")
        .WithOverflow(vertical: VerticalWrapMode.Truncate)
        .WithFontSize(16)
        .WithLocalizer(buttonText)
        .ChildOf(button.gameObject)
        .WithAnchor(Anchor.Stretch)
        .At(0, 0)
        ;

    return Select(button, text.text);
  }
}

public static class IButtonContextExtensions
{
  public static T AddClickListener<T>(this T Context, UnityAction onClickCallback)
    where T : IButtonSelectableContext
  {
    if (onClickCallback != null)
    Context.button.onClick.AddListener(onClickCallback);
    return Context;
  }
  
  public static T RemoveClickListener<T>(this T Context, UnityAction onClickCallback)
    where T : IButtonSelectableContext
  {
    if (onClickCallback != null)
    Context.button.onClick.RemoveListener(onClickCallback);
    return Context;
  }
  
  public static T ClearClickListeners<T>(this T Context)
    where T : IButtonSelectableContext
  {
    Context.button.onClick.RemoveAllListeners();
    return Context;
  }

  public static T SetClickListener<T>(this T Context, UnityAction onClickCallback)
    where T : IButtonSelectableContext
    => Context.ClearClickListeners().AddClickListener(onClickCallback);
}
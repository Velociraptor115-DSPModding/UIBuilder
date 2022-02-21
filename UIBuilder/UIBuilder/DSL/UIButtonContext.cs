using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public static class UIButtonContextExtensions
{
  public static T WithTransitions<T>(this T context, params UIButton.Transition[] transitions)
    where T: UIElementContext
  {
    var uiButton = context.uiElement.GetOrCreateComponent<UIButton>();
    uiButton.transitions = transitions;
    return context;
  }
  
  public static T WithHoverTips<T>(this T context, UIButton.TipSettings tips)
    where T: UIElementContext
  {
    var uiButton = context.uiElement.GetOrCreateComponent<UIButton>();
    uiButton.tips = tips;
    return context;
  }
  
  public static T WithInteractionAudios<T>(this T context, UIButton.AudioSettings audios)
    where T: UIElementContext
  {
    var uiButton = context.uiElement.GetOrCreateComponent<UIButton>();
    uiButton.audios = audios;
    return context;
  }

  public static UIButton.Transition CloneWithTarget(this UIButton.Transition original, Graphic target)
  {
    return new UIButton.Transition()
    {
      target = target,
      damp = original.damp,
      mouseoverSize = original.mouseoverSize,
      pressedSize = original.pressedSize,
      normalColor = original.normalColor,
      mouseoverColor = original.mouseoverColor,
      pressedColor = original.pressedColor,
      disabledColor = original.disabledColor,
      alphaOnly = original.alphaOnly,
      highlightSizeMultiplier = original.highlightSizeMultiplier,
      highlightColorMultiplier = original.highlightColorMultiplier,
      highlightAlphaMultiplier = original.highlightAlphaMultiplier,
      highlightColorOverride = original.highlightColorOverride
    };
  }
}
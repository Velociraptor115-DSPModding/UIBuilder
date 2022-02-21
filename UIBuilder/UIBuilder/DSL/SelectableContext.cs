using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface ISelectableContext
{
  Selectable selectable { get; }
}

public interface ISelectableVisuals
{
  Graphic visuals { get; set; }
}

public static class SelectableContextExtensions
{
  public static T WithTransition<T>(this T Context, Selectable.Transition? transition = null, ColorBlock? colors = null, SpriteState? spriteState = null)
    where T: ISelectableContext
  {
    if (transition.HasValue)
      Context.selectable.transition = transition.Value;
    if (colors.HasValue)
      Context.selectable.colors = colors.Value;
    if (spriteState.HasValue)
      Context.selectable.spriteState = spriteState.Value;

    return Context;
  }
  
  public static T BindInteractive<T>(this T Context, IOneWayDataBindSource<bool> binding)
    where T: UIElementContext, ISelectableContext
  {
    var selectable = Context.selectable;
    selectable.interactable = binding.Value;
    Context.WithComponent((DataBindValueChangedHandlerBool x) =>
    {
      x.Binding = binding;
      x.Handler = isOn => selectable.interactable = isOn;
    });
    return Context;
  }
  
  public static T WithVisuals<T, U>(this T Context, IProperties<U> visualProperties)
    where T: UIElementContext, ISelectableContext, ISelectableVisuals
    where U: Graphic
  {
    if (Context.visuals)
      visualProperties.Apply(Context.visuals as U);
    else
      Context.selectable.targetGraphic = Context.visuals =
        Context.selectable.gameObject.GetOrCreateComponentWithProperties(visualProperties);
    return Context;
  }
}
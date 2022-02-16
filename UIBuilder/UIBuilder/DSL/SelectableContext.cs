using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;
public static partial class UIBuilderDSL
{
  public abstract record SelectableContext<T>(GameObject uiElement) : UIElementContextBase<T>(uiElement)
    where T: SelectableContext<T>
  {
    protected abstract Selectable selectable { get; }

    public T WithTransition(Selectable.Transition? transition = null, ColorBlock? colors = null, SpriteState? spriteState = null)
    {
      if (transition.HasValue)
        selectable.transition = transition.Value;
      if (colors.HasValue)
        selectable.colors = colors.Value;
      if (spriteState.HasValue)
        selectable.spriteState = spriteState.Value;

      return Context;
    }
  }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public static partial class UIBuilderDSL
{
  public abstract record GraphicContextBase<T>(GameObject uiElement) : UIElementContextBase<T>(uiElement)
    where T: GraphicContextBase<T>
  {
    protected abstract Graphic graphic { get; }
  
    public T WithColor(Color color)
    {
      graphic.color = color;
      return Context;
    }
    
    public T WithMaterial(Material material)
    {
      graphic.material = material;
      return Context;
    }

    public T WithRaycastTarget(bool raycastTarget)
    {
      graphic.raycastTarget = raycastTarget;
      return Context;
    }
  }
}
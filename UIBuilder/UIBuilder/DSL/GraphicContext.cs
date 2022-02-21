using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface IGraphicContext
{
  Graphic graphic { get; }
}

public abstract record GraphicContext(GameObject uiElement) : UIElementContext(uiElement), IGraphicContext
{
  public abstract Graphic graphic { get; }
}

public static class IGraphicContextExtensions
{
  public static T WithColor<T>(this T Context, Color color)
    where T: IGraphicContext
  {
    Context.graphic.color = color;
    return Context;
  }
    
  public static T WithMaterial<T>(this T Context, Material material)
    where T: IGraphicContext
  {
    Context.graphic.material = material;
    return Context;
  }

  public static T WithRaycastTarget<T>(this T Context, bool raycastTarget)
    where T: IGraphicContext
  {
    Context.graphic.raycastTarget = raycastTarget;
    return Context;
  }
}
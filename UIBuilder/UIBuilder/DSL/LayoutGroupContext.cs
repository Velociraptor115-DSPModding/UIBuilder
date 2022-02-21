using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface ILayoutGroupContext
{
  LayoutGroup layoutGroup { get; }
}

public static class ILayoutGroupContextExtensions
{
  public static T WithPadding<T>(this T Context, RectOffset padding)
    where T: ILayoutGroupContext
  {
    Context.layoutGroup.padding = padding;
    return Context;
  }
  
  public static T WithChildAlignment<T>(this T Context, TextAnchor childAlignment)
    where T: ILayoutGroupContext
  {
    Context.layoutGroup.childAlignment = childAlignment;
    return Context;
  }
}

public interface IHorizontalOrVerticalLayoutGroupContext: ILayoutGroupContext
{
  new HorizontalOrVerticalLayoutGroup layoutGroup { get; }
}

public static class HorizontalOrVerticalLayoutGroupContextExtensions
{
  public static T WithSpacing<T>(this T Context, float spacing)
    where T: IHorizontalOrVerticalLayoutGroupContext
  {
    Context.layoutGroup.spacing = spacing;
    return Context;
  }
  
  public static T ForceExpand<T>(this T Context, bool width = true, bool height = true)
    where T: IHorizontalOrVerticalLayoutGroupContext
  {
    Context.layoutGroup.childForceExpandWidth = width;
    Context.layoutGroup.childForceExpandHeight = height;
    return Context;
  }
  
  public static T ChildControls<T>(this T Context, bool width = true, bool height = true)
    where T: IHorizontalOrVerticalLayoutGroupContext
  {
    Context.layoutGroup.childControlWidth = width;
    Context.layoutGroup.childControlHeight = height;
    return Context;
  }
}

public record HorizontalLayoutGroupContext(GameObject uiElement) :
  UIElementContext(uiElement), IHorizontalOrVerticalLayoutGroupContext
{
  public HorizontalLayoutGroup LayoutGroupComponent { get; } =
    uiElement.GetOrCreateComponent<HorizontalLayoutGroup>();

  HorizontalOrVerticalLayoutGroup IHorizontalOrVerticalLayoutGroupContext.layoutGroup => LayoutGroupComponent;
  LayoutGroup ILayoutGroupContext.layoutGroup => LayoutGroupComponent;
}

public record VerticalLayoutGroupContext(GameObject uiElement) :
  UIElementContext(uiElement), IHorizontalOrVerticalLayoutGroupContext
{
  public VerticalLayoutGroup LayoutGroupComponent { get; } =
    uiElement.GetOrCreateComponent<VerticalLayoutGroup>();
  
  HorizontalOrVerticalLayoutGroup IHorizontalOrVerticalLayoutGroupContext.layoutGroup => LayoutGroupComponent;
  LayoutGroup ILayoutGroupContext.layoutGroup => LayoutGroupComponent;
}

public interface IGridLayoutGroupContext: ILayoutGroupContext
{
  new GridLayoutGroup layoutGroup { get; }
}

public record GridLayoutGroupContext(GameObject uiElement) :
  UIElementContext(uiElement), IGridLayoutGroupContext
{
  public GridLayoutGroup LayoutGroupComponent { get; } =
    uiElement.GetOrCreateComponent<GridLayoutGroup>();

  GridLayoutGroup IGridLayoutGroupContext.layoutGroup => LayoutGroupComponent;
  LayoutGroup ILayoutGroupContext.layoutGroup => LayoutGroupComponent;
}

public static class GridLayoutGroupContextExtensions
{
  public static T WithSpacing<T>(this T Context, Vector2 spacing)
    where T : IGridLayoutGroupContext
  {
    Context.layoutGroup.spacing = spacing;
    return Context;
  }

  public static T WithCellSize<T>(this T Context, Vector2 cellSize)
    where T : IGridLayoutGroupContext
  {
    Context.layoutGroup.cellSize = cellSize;
    return Context;
  }

  public static T WithConstraint<T>(this T Context, GridLayoutGroup.Constraint constraint, int constraintCount)
    where T : IGridLayoutGroupContext
  {
    Context.layoutGroup.constraint = constraint;
    Context.layoutGroup.constraintCount = constraintCount;
    return Context;
  }

  public static T WithStart<T>(this T Context, GridLayoutGroup.Corner startCorner, GridLayoutGroup.Axis startAxis)
    where T : IGridLayoutGroupContext
  {
    Context.layoutGroup.startCorner = startCorner;
    Context.layoutGroup.startAxis = startAxis;
    return Context;
  }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;
public static partial class UIBuilderDSL
{
  public abstract record LayoutGroupContext<T, U>(GameObject uiElement) : UIElementContextBase<T>(uiElement)
    where T: LayoutGroupContext<T, U>
    where U: LayoutGroup
  {
    protected abstract U LayoutGroupComponent { get; }
    
    public T WithPadding(RectOffset padding)
    {
      LayoutGroupComponent.padding = padding;
      return Context;
    }
    
    public T WithChildAlignment(TextAnchor childAlignment)
    {
      LayoutGroupComponent.childAlignment = childAlignment;
      return Context;
    }
  }
  
  public abstract record HorizontalOrVerticalLayoutGroupContext<T, U>(GameObject uiElement) : LayoutGroupContext<T, U>(uiElement)
    where T: HorizontalOrVerticalLayoutGroupContext<T, U>
    where U: HorizontalOrVerticalLayoutGroup 
  {

    public T WithSpacing(float spacing)
    {
      LayoutGroupComponent.spacing = spacing;
      return Context;
    }
    
    public T ForceExpand(bool width = true, bool height = true)
    {
      LayoutGroupComponent.childForceExpandWidth = width;
      LayoutGroupComponent.childForceExpandHeight = height;
      return Context;
    }
    
    public T ChildControls(bool width = true, bool height = true)
    {
      LayoutGroupComponent.childControlWidth = width;
      LayoutGroupComponent.childControlHeight = height;
      return Context;
    }
  }

  public record HorizontalLayoutGroupContext(GameObject uiElement) :
    HorizontalOrVerticalLayoutGroupContext<HorizontalLayoutGroupContext, HorizontalLayoutGroup>(uiElement)
  {
    public override HorizontalLayoutGroupContext Context => this;
    protected override HorizontalLayoutGroup LayoutGroupComponent { get; } =
      uiElement.GetOrCreateComponent<HorizontalLayoutGroup>();
  }
  
  public record VerticalLayoutGroupContext(GameObject uiElement) :
    HorizontalOrVerticalLayoutGroupContext<VerticalLayoutGroupContext, VerticalLayoutGroup>(uiElement)
  {
    public override VerticalLayoutGroupContext Context => this;
    protected override VerticalLayoutGroup LayoutGroupComponent { get; } =
      uiElement.GetOrCreateComponent<VerticalLayoutGroup>();
  }
  
  public record GridLayoutGroupContext(GameObject uiElement) :
    LayoutGroupContext<GridLayoutGroupContext, GridLayoutGroup>(uiElement)
  {
    public override GridLayoutGroupContext Context => this;
    protected override GridLayoutGroup LayoutGroupComponent { get; } =
      uiElement.GetOrCreateComponent<GridLayoutGroup>();

    public GridLayoutGroupContext WithSpacing(Vector2 spacing)
    {
      LayoutGroupComponent.spacing = spacing;
      return Context;
    }
    
    public GridLayoutGroupContext WithCellSize(Vector2 cellSize)
    {
      LayoutGroupComponent.cellSize = cellSize;
      return Context;
    }
    
    public GridLayoutGroupContext WithConstraint(GridLayoutGroup.Constraint constraint, int constraintCount)
    {
      LayoutGroupComponent.constraint = constraint;
      LayoutGroupComponent.constraintCount = constraintCount;
      return Context;
    }
    
    public GridLayoutGroupContext WithStart(GridLayoutGroup.Corner startCorner, GridLayoutGroup.Axis startAxis)
    {
      LayoutGroupComponent.startCorner = startCorner;
      LayoutGroupComponent.startAxis = startAxis;
      return Context;
    }
  }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;

public static partial class UIBuilderDSL
{
  public abstract record WindowContext<T>(GameObject window) : UIElementContextBase<T>(window)
  {
    protected GameObject panelBg { get; set; }
    protected abstract TranslucentImage panelBgCloneImg { get; }
    protected abstract Image panelBgBorderCloneImg { get; }
    protected abstract Image panelBgDragTriggerCloneImg { get; }
    protected abstract Image shadowCloneImg { get; }

    internal WindowContext<T> WithPanelBg()
    {
      if (this.panelBg != null)
        return this;

      panelBg =
        Create.UIElement("panel-bg")
          .CloneComponentFrom(panelBgCloneImg)
          .ChildOf(uiElement).WithAnchor(Anchor.Stretch)
          .uiElement;

      return this;
    }

    internal WindowContext<T> WithDragSupport()
    {
      WithPanelBg();

      var dragTrigger = uiElement.SelectChild("drag-trigger");
      if (dragTrigger == null)
      {
        dragTrigger =
          Create.UIElement("drag-trigger")
            .CloneComponentFrom(panelBgDragTriggerCloneImg)
            .ChildOf(panelBg).WithAnchor(Anchor.Stretch)
            .uiElement;
      }
        
      return WithDragSupport(dragTrigger);
    }

    internal WindowContext<T> WithDragSupport(GameObject dragTriggerObj)
    {
      var dragComponent = uiElement.GetComponent<UIWindowDrag>();
      if (dragComponent != null)
        return this;

      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);

      dragComponent = uiElement.GetOrCreateComponent<UIWindowDrag>();
      dragComponent.dragTrigger = dragTriggerObj.GetOrCreateComponent<EventTrigger>();

      if (wasActive)
        uiElement.SetActive(true);

      return this;
    }

    public WindowContext<T> WithDragProperties(int dragThreshold = 4, float damping = 0.2f, float spring = 0.5f)
    {
      WithDragSupport();
      var dragComponent = uiElement.GetComponent<UIWindowDrag>();
      dragComponent.dragThreshold = dragThreshold;
      dragComponent.damping = damping;
      dragComponent.spring = spring;
      return this;
    }

    internal WindowContext<T> WithResizeSupport()
    {
      WithPanelBg();

      var resizeTrigger = uiElement.SelectChild("resize-trigger");
      if (resizeTrigger == null)
      {
        resizeTrigger =
          Create.UIElement("resize-trigger")
            .CloneComponentFrom(UIBuilder.plainWindowPanelBgDragTrigger)
            .ChildOf(panelBg).WithAnchor(Anchor.BottomRight).OfSize(20, 20)
            .uiElement;
      }

      return WithResizeSupport(resizeTrigger);
    }

    internal WindowContext<T> WithResizeSupport(GameObject resizeTriggerObj)
    {
      var resizeComponent = uiElement.GetComponent<UIWindowResize>();
      if (resizeComponent != null)
        return this;

      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);

      resizeComponent = uiElement.GetOrCreateComponent<UIWindowResize>();
      resizeComponent.resizeTrigger = resizeTriggerObj.GetOrCreateComponent<EventTrigger>(); ;

      if (wasActive)
        uiElement.SetActive(true);

      return this;
    }

    public WindowContext<T> WithResizeProperties(int resizeThreshold = 4, Vector2? minSizeOpt = null)
    {
      var minSize = minSizeOpt ?? new Vector2(300, 200);
      WithResizeSupport();
      var resizeComponent = uiElement.GetComponent<UIWindowResize>();
      resizeComponent.resizeThreshold = resizeThreshold;
      resizeComponent.minSize = minSize;
      return this;
    }

    public virtual WindowContext<T> WithShadow()
    {
      Create.UIElement("shadow")
        .CloneComponentFrom(shadowCloneImg)
        .ChildOf(uiElement).WithAnchor(Anchor.Stretch);
      return this;
    }

    public virtual WindowContext<T> WithBorder()
    {
      WithPanelBg();
      Create.UIElement("border")
        .CloneComponentFrom(panelBgBorderCloneImg)
        .ChildOf(panelBg).WithAnchor(Anchor.Stretch);
      return this;
    }
  }
}
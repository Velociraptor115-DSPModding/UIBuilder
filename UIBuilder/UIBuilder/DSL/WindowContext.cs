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
    protected GameObject panelBg { get; init; }
    protected abstract TranslucentImage panelBgCloneImg { get; }
    protected abstract Image panelBgBorderCloneImg { get; }
    protected abstract Image panelBgDragTriggerCloneImg { get; }
    protected abstract Image shadowCloneImg { get; }

    internal WindowContext<T> WithPanelBg()
    {
      if (this.panelBg != null)
        return this;

      var panelBg =
        New.UIElement("panel-bg")
          .CloneComponentFrom(panelBgCloneImg)
          .ChildOf(this.transform).WithAnchor(Anchor.Stretch);

      return this with { panelBg = panelBg.uiElement };
    }

    internal WindowContext<T> WithDragSupport()
    {
      var ctx = WithPanelBg();

      var dragTrigger = ctx.uiElement.SelectChild("drag-trigger");
      if (dragTrigger == null)
      {
        dragTrigger =
          New.UIElement("drag-trigger")
            .CloneComponentFrom(panelBgDragTriggerCloneImg)
            .ChildOf(ctx.panelBg).WithAnchor(Anchor.Stretch)
            .uiElement;
      }
        
      return ctx.WithDragSupport(dragTrigger);
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
      dragComponent.dragTrigger = dragTriggerObj.GetOrCreateComponent<EventTrigger>(); ;

      if (wasActive)
        uiElement.SetActive(true);

      return WithDragProperties();
    }

    public WindowContext<T> WithDragProperties(int dragThreshold = 4, float damping = 0.2f, float spring = 0.5f)
    {
      var ctx = this;
      var dragComponent = uiElement.GetComponent<UIWindowDrag>();
      if (dragComponent == null)
        ctx = WithDragSupport();
      dragComponent = uiElement.GetComponent<UIWindowDrag>();
      dragComponent.dragThreshold = dragThreshold;
      dragComponent.damping = damping;
      dragComponent.spring = spring;
      return ctx;
    }

    internal WindowContext<T> WithResizeSupport()
    {
      var ctx = WithPanelBg();

      var resizeTrigger = ctx.uiElement.SelectChild("resize-trigger");
      if (resizeTrigger == null)
      {
        resizeTrigger =
          New.UIElement("resize-trigger")
            .CloneComponentFrom(UIBuilder.plainWindowPanelBgDragTrigger)
            .ChildOf(ctx.panelBg).WithAnchor(Anchor.BottomRight).OfSize(20, 20)
            .uiElement;
      }

      return ctx.WithResizeSupport(resizeTrigger);
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

      return WithResizeProperties();
    }

    public WindowContext<T> WithResizeProperties(int resizeThreshold = 4, Vector2? minSizeOpt = null)
    {
      var minSize = minSizeOpt ?? new Vector2(300, 200);
      var ctx = this;
      var resizeComponent = uiElement.GetComponent<UIWindowResize>();
      if (resizeComponent == null)
        ctx = WithResizeSupport();
      resizeComponent = uiElement.GetComponent<UIWindowResize>();
      resizeComponent.resizeThreshold = resizeThreshold;
      resizeComponent.minSize = minSize;
      return ctx;
    }

    public virtual WindowContext<T> WithShadow()
    {
      New.UIElement("shadow")
        .CloneComponentFrom(shadowCloneImg)
        .ChildOf(uiElement).WithAnchor(Anchor.Stretch);
      return this;
    }

    public virtual WindowContext<T> WithBorder()
    {
      var ctx = WithPanelBg();
      New.UIElement("border")
        .CloneComponentFrom(panelBgBorderCloneImg)
        .ChildOf(ctx.panelBg).WithAnchor(Anchor.Stretch);
      return ctx;
    }
  }
}
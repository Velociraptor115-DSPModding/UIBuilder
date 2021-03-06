using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public static partial class UIBuilderDSL
{
  public abstract record WindowContext<T>(GameObject uiElement) : UIElementContext(uiElement)
    where T: WindowContext<T>
  {
    public bool scrollCapture { get; set; }
    public GameObject panelBg { get; protected set; }
    protected abstract TranslucentImageProperties panelBgCloneImgProperties { get; }
    protected abstract ImageProperties panelBgBorderCloneImgProperties { get; }
    protected abstract ImageProperties panelBgDragTriggerCloneImgProperties { get; }
    protected abstract ImageProperties shadowCloneImgProperties { get; }

    public T WithScrollCapture()
    {
      scrollCapture = true;
      return Context;
    }

    internal T WithPanelBg()
    {
      if (this.panelBg != null)
        return Context;

      panelBg =
        Create.UIElement("panel-bg")
          .WithComponent(out TranslucentImage _, panelBgCloneImgProperties)
          .ChildOf(uiElement).WithAnchor(Anchor.Stretch)
          .uiElement;

      return Context;
    }

    internal T WithDragSupport()
    {
      WithPanelBg();

      var dragTrigger = panelBg.SelectChild("drag-trigger");
      if (dragTrigger == null)
      {
        dragTrigger =
          Create.UIElement("drag-trigger")
            .WithComponent(out Image _, panelBgDragTriggerCloneImgProperties)
            .ChildOf(panelBg).WithAnchor(Anchor.Stretch)
            .uiElement;
      }
        
      return WithDragSupport(dragTrigger);
    }

    internal T WithDragSupport(GameObject dragTriggerObj)
    {
      var dragComponent = uiElement.GetComponent<UIWindowDrag>();
      if (dragComponent != null)
        return Context;

      using (DeactivatedScope)
      {
        dragComponent = uiElement.GetOrCreateComponent<UIWindowDrag>();
        dragComponent.dragTrigger = dragTriggerObj.GetOrCreateComponent<EventTrigger>(); 
      }

      return Context;
    }

    public T WithDragProperties(int dragThreshold = 4, float damping = 0.2f, float spring = 0.5f)
    {
      WithDragSupport();
      var dragComponent = uiElement.GetComponent<UIWindowDrag>();
      dragComponent.dragThreshold = dragThreshold;
      dragComponent.damping = damping;
      dragComponent.spring = spring;
      return Context;
    }

    internal T WithResizeSupport()
    {
      WithPanelBg();

      var resizeTrigger = panelBg.SelectChild("resize-trigger");
      if (resizeTrigger == null)
      {
        resizeTrigger =
          Create.UIElement("resize-trigger")
            .WithComponent(out Image _, UIBuilder.plainWindowPanelBgDragTriggerProperties)
            .ChildOf(panelBg).WithAnchor(Anchor.BottomRight).OfSize(20, 20)
            .uiElement;
      }

      return WithResizeSupport(resizeTrigger);
    }

    internal T WithResizeSupport(GameObject resizeTriggerObj)
    {
      var resizeComponent = uiElement.GetComponent<UIWindowResize>();
      if (resizeComponent != null)
        return Context;

      using (DeactivatedScope)
      {
        resizeComponent = uiElement.GetOrCreateComponent<UIWindowResize>();
        resizeComponent.resizeTrigger = resizeTriggerObj.GetOrCreateComponent<EventTrigger>(); ; 
      }

      return Context;
    }

    public T WithResizeProperties(int resizeThreshold = 4, Vector2? minSizeOpt = null)
    {
      var minSize = minSizeOpt ?? new Vector2(300, 200);
      WithResizeSupport();
      var resizeComponent = uiElement.GetComponent<UIWindowResize>();
      resizeComponent.resizeThreshold = resizeThreshold;
      resizeComponent.minSize = minSize;
      return Context;
    }

    public virtual T WithShadow()
    {
      var shadow =
        Create.UIElement("shadow")
          .WithComponent(out Image _, shadowCloneImgProperties)
          .ChildOf(uiElement).WithAnchor(Anchor.Stretch);
      shadow.transform.SetAsFirstSibling();
      return Context;
    }

    public abstract T WithBorder();

    public abstract T Context { get; }

    public abstract T WithTitle(string title);
    
    // TODO: Add WithContentBg
  }
}
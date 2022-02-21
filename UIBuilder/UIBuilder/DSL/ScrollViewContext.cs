using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public enum ScrollViewAxis
{
  VerticalOnly,
  HorizontalOnly,
  BothVerticalAndHorizontal
}

public record struct ScrollViewConfiguration(
  ScrollViewAxis axis = ScrollViewAxis.VerticalOnly
  , uint scrollBarWidth = 5
  , ScrollRectProperties scrollRectProperties = null
  , ImageProperties scrollBgImgProperties = null
  , ImageProperties scrollHandleImgProperties = null
);

public record ScrollViewContext(ScrollRect scrollRect) : UIElementContext(scrollRect.gameObject);

public static class ScrollViewContextExtensions
{
  public static ScrollViewContext Select(ScrollRect scrollRect)
    => new ScrollViewContext(scrollRect);
  
  public static ScrollViewContext Create(GameObject uiElement, ScrollViewConfiguration configuration)
  {
    using var _ = uiElement.DeactivatedScope();

    var scrollRectProperties = configuration.scrollRectProperties ?? UIBuilder.scrollRectProperties;
    var scrollBgImgProperties = configuration.scrollBgImgProperties ?? UIBuilder.scrollBgImgProperties;
    var scrollHandleImgProperties = configuration.scrollHandleImgProperties ?? UIBuilder.scrollHandleImgProperties;

    var scrollRect = uiElement.GetOrCreateComponentWithProperties(scrollRectProperties);

    var viewport =
      UIBuilderDSL.Create.UIElement("viewport")
        .ChildOf(uiElement).WithAnchor(Anchor.Stretch)
        .At(0, 0)
        .WithComponent(out Mask _, x => x.showMaskGraphic = false)
        .WithComponent(out Image _, x => x.color = Color.white);
    
    scrollRect.viewport = viewport.transform;

    var contentAnchor = configuration.axis switch
    {
      ScrollViewAxis.VerticalOnly => Anchor.TopStretch,
      ScrollViewAxis.HorizontalOnly => Anchor.StretchLeft,
      ScrollViewAxis.BothVerticalAndHorizontal => Anchor.TopLeft,
      _ => throw new ArgumentOutOfRangeException("configuration.axis", configuration.axis, null)
    };

    var content =
      UIBuilderDSL.Create.UIElement("content")
        .ChildOf(viewport).WithAnchor(contentAnchor)
        .WithPivot(0, 1)
        .At(0, 0);
    
    scrollRect.content = content.transform;
    
    var horizontalScroll = configuration.axis is ScrollViewAxis.HorizontalOnly or ScrollViewAxis.BothVerticalAndHorizontal;
    var verticalScroll = configuration.axis is ScrollViewAxis.VerticalOnly or ScrollViewAxis.BothVerticalAndHorizontal;
    
    scrollRect.vertical = verticalScroll;
    scrollRect.horizontal = horizontalScroll;

    if (verticalScroll)
    {
      var vScroll =
        UIBuilderDSL.Create.UIElement("v-bar")
          .ChildOf(uiElement).WithAnchor(Anchor.StretchRight).WithPivot(1, 1)
          .OfSize((int) configuration.scrollBarWidth, 0)
          .At(0, 0);

      var vSlidingArea =
        UIBuilderDSL.Create.UIElement("sliding-area")
          .ChildOf(vScroll).WithAnchor(Anchor.Stretch)
          .At(0, 0);

      var vSlidingAreaHandle =
        UIBuilderDSL.Create.UIElement("handle")
          .ChildOf(vSlidingArea).WithAnchor(Anchor.TopStretch)
          .OfSize(0, 0).At(0, 0);

      vScroll.WithComponent(out Image _, scrollBgImgProperties);
      vSlidingAreaHandle.WithComponent(out Image vSlidingAreaHandleImg, scrollHandleImgProperties);
      vScroll.WithComponent(out Scrollbar vScrollbar, x =>
      {
        x.targetGraphic = vSlidingAreaHandleImg;
        x.direction = Scrollbar.Direction.BottomToTop;
        x.handleRect = vSlidingAreaHandle.transform;
      });
      
      scrollRect.verticalScrollbar = vScrollbar;
      scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
    }
    
    if (horizontalScroll)
    {
      var hScroll =
        UIBuilderDSL.Create.UIElement("h-bar")
          .ChildOf(uiElement).WithAnchor(Anchor.BottomStretch).WithPivot(0, 0)
          .OfSize(0, (int) configuration.scrollBarWidth)
          .At(0, 0);

      var hSlidingArea =
        UIBuilderDSL.Create.UIElement("sliding-area")
          .ChildOf(hScroll).WithAnchor(Anchor.Stretch)
          .At(0, 0);

      var hSlidingAreaHandle =
        UIBuilderDSL.Create.UIElement("handle")
          .ChildOf(hSlidingArea).WithAnchor(Anchor.StretchLeft)
          .OfSize(0, 0).At(0, 0);

      hScroll.WithComponent(out Image _, scrollBgImgProperties);
      hSlidingAreaHandle.WithComponent(out Image hSlidingAreaHandleImg, scrollHandleImgProperties);
      hScroll.WithComponent(out Scrollbar hScrollbar, x =>
      {
        x.targetGraphic = hSlidingAreaHandleImg;
        x.direction = Scrollbar.Direction.LeftToRight;
        x.handleRect = hSlidingAreaHandle.transform;
      });
      
      scrollRect.horizontalScrollbar = hScrollbar;
      scrollRect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
    }

    return Select(scrollRect);
  }
}
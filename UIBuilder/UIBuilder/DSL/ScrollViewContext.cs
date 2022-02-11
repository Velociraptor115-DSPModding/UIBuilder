using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;
public static partial class UIBuilderDSL
{
  public record ScrollSupport(
    GameObject scrollViewObj
    , GameObject scrollContentObj
    , ScrollRect scrollRect
    , Scrollbar vScrollbar
    , Scrollbar hScrollbar
  );
  
  public record ScrollViewContext(GameObject uiElement) : UIElementContextBase<ScrollViewContext>(uiElement)
  {
    public override ScrollViewContext Context => this;

    public ScrollSupport scrollSupport { get; set; }
    public GameObject contentRoot => scrollSupport != null ? scrollSupport.scrollContentObj : uiElement;

    public ScrollViewContext WithScrollSupport(bool onlyVerticalScroll = true, uint scrollBarWidth = 5)
    {
      if (scrollSupport != null)
        return Context;

      var viewportObj =
        Create.UIElement("viewport")
          .ChildOf(uiElement).WithAnchor(Anchor.Stretch)
          .At(0, 0)
          .uiElement;

      var contentObj =
        Create.UIElement("content")
          .ChildOf(viewportObj).WithAnchor(onlyVerticalScroll ? Anchor.TopStretch : Anchor.TopLeft)
          .WithPivot(0, 1)
          .At(0, 0)
          .uiElement;
      
      var vScroll =
        Create.UIElement("v-bar")
          .ChildOf(uiElement).WithAnchor(Anchor.StretchRight).WithPivot(1, 1)
          .OfSize((int)scrollBarWidth, 0)
          .At(0, 0)
          .uiElement;

      var vSlidingArea =
        Create.UIElement("sliding-area")
          .ChildOf(vScroll).WithAnchor(Anchor.Stretch)
          .At(0, 0)
          .uiElement;
      
      var vSlidingAreaHandle =
        Create.UIElement("handle")
          .ChildOf(vSlidingArea).WithAnchor(Anchor.TopStretch)
          .OfSize(0, 0).At(0, 0)
          .uiElement;

      var hScroll =
        !onlyVerticalScroll
          ? Create.UIElement("h-bar")
              .ChildOf(uiElement).WithAnchor(Anchor.BottomStretch).WithPivot(0, 0)
              .OfSize(0, (int)scrollBarWidth)
              .At(0, 0)
              .uiElement
          : null;

      var hSlidingArea =
        !onlyVerticalScroll
          ? Create.UIElement("sliding-area")
              .ChildOf(hScroll).WithAnchor(Anchor.Stretch)
              .At(0, 0)
              .uiElement
          : null;

      var hSlidingAreaHandle =
        !onlyVerticalScroll
          ? Create.UIElement("handle")
              .ChildOf(hSlidingArea).WithAnchor(Anchor.StretchLeft)
              .OfSize(0, 0).At(0, 0)
              .uiElement
          : null;

      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);

      viewportObj.GetOrCreateComponent<RectMask2D>();
      var col = viewportObj.GetOrCreateComponent<Image>();
      col.color = Color.clear;

      vScroll.CloneComponentFrom(UIBuilder.scrollBgImg);
      vSlidingAreaHandle.CloneComponentFrom(UIBuilder.scrollHandleImg);
      var vScrollbar = vScroll.GetOrCreateComponent<Scrollbar>();
      (vScrollbar as Selectable).CopyFrom(UIBuilder.scrollSelectable);
      vScrollbar.direction = Scrollbar.Direction.BottomToTop;
      vScrollbar.handleRect = vSlidingAreaHandle.GetComponent<RectTransform>();

      var hScrollbar =
        !onlyVerticalScroll
          ? hScroll.GetOrCreateComponent<Scrollbar>()
          : null;

      if (!onlyVerticalScroll)
      {
        hScroll.CloneComponentFrom(UIBuilder.scrollBgImg);
        hSlidingAreaHandle.CloneComponentFrom(UIBuilder.scrollHandleImg);
        (hScrollbar as Selectable).CopyFrom(UIBuilder.scrollSelectable);
        hScrollbar.direction = Scrollbar.Direction.LeftToRight;
        hScrollbar.handleRect = hSlidingAreaHandle.GetComponent<RectTransform>();
      }

      var scrollRect = uiElement.GetOrCreateComponent<ScrollRect>();
      scrollRect.CopyFrom(UIBuilder.scrollRect);

      scrollRect.content = contentObj.GetComponent<RectTransform>();
      scrollRect.viewport = viewportObj.GetComponent<RectTransform>();
      scrollRect.vertical = true;
      scrollRect.horizontal = !onlyVerticalScroll;
      scrollRect.verticalScrollbar = vScrollbar;
      scrollRect.horizontalScrollbar = hScrollbar;
      scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
      scrollRect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;

      if (wasActive)
        uiElement.SetActive(true);

      scrollSupport =
        new ScrollSupport(
            uiElement
          , contentObj
          , scrollRect
          , vScrollbar
          , hScrollbar
        );

      return Context;
    }
  }
}
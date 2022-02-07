using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;
public static partial class UIBuilderDSL
{
  public static readonly CreateContext Create;

  public struct CreateContext
  {
    public readonly UIElementContext UIElement(string name)
    {
      var obj = new GameObject(name, typeof(RectTransform));
      obj.SetLayer((int)Layer.UI);
      return new UIElementContext(obj).OfSize(0, 0).At(0, 0).Context; 
    }

    public readonly PlainWindowContext PlainWindow(string name)
    {
      return (
        new PlainWindowContext(UIElement(name).uiElement)
          .WithBorder()
          .WithShadow()
          .WithDragProperties()
          .WithResizeProperties()
          .Context
      );
    }

    public readonly FancyWindowContext FancyWindow(string name)
    {
      return (
        new FancyWindowContext(UIElement(name).uiElement)
          .WithBorder()
          .WithShadow()
          .WithDragProperties()
          .WithResizeProperties()
          .Context
      );
    }

    public readonly ScrollViewContext ScrollView(string name, bool onlyVerticalScroll = true, uint scrollBarWidth = 5)
    {
      return new ScrollViewContext(UIElement(name).uiElement)
        .WithScrollSupport(onlyVerticalScroll, scrollBarWidth);
    }
  }
}
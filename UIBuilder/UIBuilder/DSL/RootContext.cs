using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;
public static partial class UIBuilderDSL
{
  public static readonly RootContext New;

  public struct RootContext
  {
    public UIElementContext UIElement(string name)
      => new UIElementContext(new GameObject(name, typeof(RectTransform)));

    public PlainWindowContext PlainWindow(string name)
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

    public FancyWindowContext FancyWindow(string name)
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
  }
}
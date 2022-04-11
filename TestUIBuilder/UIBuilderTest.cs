using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BepInEx.Configuration;
using Object = UnityEngine.Object;
using DysonSphereProgram.Modding.UI.Builder;

namespace DysonSphereProgram.Modding.Raptor.TestUIBuilder
{
  using UI.Builder;
  using static UI.Builder.UIBuilderDSL;

  public static class UIBuilderTest
  {
    static GameObject obj;
    static UIModWindowBase myWindow;

    public static float mySliderValue;
    public static void CreateUI()
    {
      if (obj != null)
        return;

      var windowsObj = GameObject.Find("UI Root/Overlay Canvas/In Game/Windows");

      obj =
        Create.FancyWindow("Test UI Builder #1")
          .ChildOf(windowsObj)
          .WithAnchor(Anchor.TopLeft)
          .OfSize(500, 300)
          .At(300, -180)
          .WithScrollCapture()
          .WithTitle("Example Window")
          .InitializeComponent(out myWindow)
          .uiElement
          ;
    }

    public static void DestroyUI()
    {
      if (obj == null)
        return;

      Object.Destroy(obj);
      obj = null;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BepInEx.Configuration;
using Object = UnityEngine.Object;

namespace DysonSphereProgram.Modding.UI
{
  public static class TestUIBuilder
  {
    static GameObject obj;
    static UIWindow myWindow;

    public static float mySliderValue;
    public static void Create()
    {
      if (obj != null)
        return;

      var windowsObj = GameObject.Find("UI Root/Overlay Canvas/In Game/Windows");

      obj =
        UIBuilderDSL.Create.FancyWindow("Test UI Builder #1")
          .ChildOf(windowsObj)
          .WithAnchor(Anchor.TopLeft)
          .OfSize(500, 300)
          .At(300, -180)
          .WithScrollSupport()
          .InitializeComponent(out myWindow)
          .uiElement
          ;
    }

    public static void Destroy()
    {
      if (obj == null)
        return;

      Object.Destroy(obj);
      obj = null;
    }
  }
}

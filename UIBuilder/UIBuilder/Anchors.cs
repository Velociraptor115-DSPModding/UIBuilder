using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection.Emit;

namespace DysonSphereProgram.Modding.UI.Builder
{
  public enum AnchorHorizontal
  {
    Left, Middle, Right, Stretch
  }

  public enum AnchorVertical
  {
    Top, Middle, Bottom, Stretch
  }

  public record Anchor(AnchorVertical vertical, AnchorHorizontal horizontal)
  {
    public static Anchor TopLeft = new Anchor(AnchorVertical.Top, AnchorHorizontal.Left);
    public static Anchor Top = new Anchor(AnchorVertical.Top, AnchorHorizontal.Middle);
    public static Anchor TopRight = new Anchor(AnchorVertical.Top, AnchorHorizontal.Right);
    
    public static Anchor Left = new Anchor(AnchorVertical.Middle, AnchorHorizontal.Left);
    public static Anchor Center = new Anchor(AnchorVertical.Middle, AnchorHorizontal.Middle);
    public static Anchor Right = new Anchor(AnchorVertical.Middle, AnchorHorizontal.Right);
    
    public static Anchor BottomLeft = new Anchor(AnchorVertical.Bottom, AnchorHorizontal.Left);
    public static Anchor Bottom = new Anchor(AnchorVertical.Bottom, AnchorHorizontal.Middle);
    public static Anchor BottomRight = new Anchor(AnchorVertical.Bottom, AnchorHorizontal.Right);
    
    public static Anchor Stretch = new Anchor(AnchorVertical.Stretch, AnchorHorizontal.Stretch);
    
    public static Anchor TopStretch = new Anchor(AnchorVertical.Top, AnchorHorizontal.Stretch);
    public static Anchor CenterStretch = new Anchor(AnchorVertical.Middle, AnchorHorizontal.Stretch);
    public static Anchor BottomStretch = new Anchor(AnchorVertical.Bottom, AnchorHorizontal.Stretch);

    public static Anchor StretchLeft = new Anchor(AnchorVertical.Stretch, AnchorHorizontal.Left);
    public static Anchor StretchCenter = new Anchor(AnchorVertical.Stretch, AnchorHorizontal.Middle);
    public static Anchor StretchRight = new Anchor(AnchorVertical.Stretch, AnchorHorizontal.Right);

    public (Vector2, Vector2) ToMinMaxAnchors()
    {
      var (horizontalMin, horizontalMax) =
        horizontal switch
        {
            AnchorHorizontal.Left => (0f, 0f)
          , AnchorHorizontal.Right => (1f, 1f)
          , AnchorHorizontal.Middle => (0.5f, 0.5f)
          , AnchorHorizontal.Stretch => (0f, 1f)
          , _ => throw new System.NotImplementedException()
        };

      var (verticalMin, verticalMax) =
        vertical switch
        {
            AnchorVertical.Bottom => (0f, 0f)
          , AnchorVertical.Top => (1f, 1f)
          , AnchorVertical.Middle => (0.5f, 0.5f)
          , AnchorVertical.Stretch => (0f, 1f)
          , _ => throw new System.NotImplementedException()
        };

      return (new Vector2(horizontalMin, verticalMin), new Vector2(horizontalMax, verticalMax));
    }
  }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public static class FitterContextExtensions
{
  public static T WithContentSizeFitter<T>(this T context, ContentSizeFitter.FitMode? horizontal = null, ContentSizeFitter.FitMode? vertical = null)
    where T: UIElementContext
  {
    var csf = context.uiElement.GetOrCreateComponent<ContentSizeFitter>();
    if (horizontal.HasValue)
      csf.horizontalFit = horizontal.Value;
    if (vertical.HasValue)
      csf.verticalFit = vertical.Value;
    return context;
  }
  
  public static T WithAspectRatioFitter<T>(this T context, float? aspectRatio = null, AspectRatioFitter.AspectMode? mode = null)
    where T: UIElementContext
  {
    var arf = context.uiElement.GetOrCreateComponent<AspectRatioFitter>();
    if (aspectRatio.HasValue)
      arf.aspectRatio = aspectRatio.Value;
    if (mode.HasValue)
      arf.aspectMode = mode.Value;
    return context;
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DysonSphereProgram.Modding.UI.Builder
{
  using static UIBuilderDSL;

  public class UIModWindowBase: ManualBehaviour,
    IInitializeFromContext<FancyWindowContext>, IInitializeFromContext<PlainWindowContext>,
    IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
  {
    public void InitializeFromContext(FancyWindowContext context) =>
      InitializeFromContext<FancyWindowContext>(context.WithCloseButton(_Close));
    public void InitializeFromContext(PlainWindowContext context) =>
      InitializeFromContext<PlainWindowContext>(context.WithCloseButton(_Close));
    private void InitializeFromContext<T>(WindowContext<T> context)
      where T: WindowContext<T>
    {
      toCaptureScroll = context.scrollCapture;
      base._Create();
      base._Init(null);
      base._Open();
    }

    public override void _OnClose()
    {
      if (toCaptureScroll)
        UIBuilder.inScrollView.Remove(this);
      base._OnClose();
    }

    private bool toCaptureScroll;

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (!toCaptureScroll)
        return;
      UIBuilder.inScrollView.Add(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
      if (!toCaptureScroll)
        return;
      UIBuilder.inScrollView.Remove(this);
    }
    
    private void OnApplicationFocus(bool focus)
    {
      if (!toCaptureScroll)
        return;
      if (!focus)
      {
        focusPointEnter = UIBuilder.inScrollView.Contains(this);
        UIBuilder.inScrollView.Remove(this);
      }
      if (focus)
      {
        if (focusPointEnter)
          UIBuilder.inScrollView.Add(this);
        else
          UIBuilder.inScrollView.Remove(this);
      }
    }
    
    private bool focusPointEnter;
  }
}

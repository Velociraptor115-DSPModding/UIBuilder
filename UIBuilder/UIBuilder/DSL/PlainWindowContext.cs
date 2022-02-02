using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;

public static partial class UIBuilderDSL
{
  public record PlainWindowContext(GameObject window) : WindowContext<PlainWindowContext>(window)
  {
    public override PlainWindowContext Context => this;
    protected override TranslucentImage panelBgCloneImg => UIBuilder.plainWindowPanelBg;
    protected override Image panelBgBorderCloneImg => UIBuilder.plainWindowPanelBgBorder;
    protected override Image panelBgDragTriggerCloneImg => UIBuilder.plainWindowPanelBgDragTrigger;
    protected override Image shadowCloneImg => UIBuilder.plainWindowShadowImg;

    public override WindowContext<PlainWindowContext> WithShadow()
    {
      base.WithShadow();
      new UIElementContext(uiElement.SelectChild("shadow")).OfSize(44, 44);
      return this;
    }
  }
}
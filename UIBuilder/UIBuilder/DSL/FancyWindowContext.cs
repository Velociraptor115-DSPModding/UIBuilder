using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;

public static partial class UIBuilderDSL
{
  public record FancyWindowContext(GameObject window) : WindowContext<FancyWindowContext>(window)
  {
    public override FancyWindowContext Context => this;
    protected override TranslucentImage panelBgCloneImg => UIBuilder.fancyWindowPanelBg;
    protected override Image panelBgBorderCloneImg => UIBuilder.fancyWindowPanelBgBorder;
    protected override Image panelBgDragTriggerCloneImg => UIBuilder.fancyWindowPanelBgDragTrigger;
    protected override Image shadowCloneImg => UIBuilder.fancyWindowShadowImg;
  }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DysonSphereProgram.Modding.UI.Builder;

public static partial class UIBuilderDSL
{
  public record PlainWindowContext(GameObject uiElement) : WindowContext<PlainWindowContext>(uiElement)
  {
    public override PlainWindowContext Context => this;
    protected override TranslucentImageProperties panelBgCloneImgProperties => UIBuilder.plainWindowPanelBgProperties;
    protected override ImageProperties panelBgBorderCloneImgProperties  => UIBuilder.plainWindowPanelBgBorderProperties;
    protected override ImageProperties panelBgDragTriggerCloneImgProperties  => UIBuilder.plainWindowPanelBgDragTriggerProperties;
    protected override ImageProperties shadowCloneImgProperties  => UIBuilder.plainWindowShadowImgProperties;

    public override PlainWindowContext WithShadow()
    {
      base.WithShadow();
      new UIElementContext(uiElement.SelectChild("shadow")).OfSize(44, 44);
      return Context;
    }

    public PlainWindowContext WithCloseButton(UnityAction closeCallback)
    {
      WithPanelBg();
      if (panelBg.SelectChild("x") != null)
        return Context;

      var closeBtnObj = 
        Create.UIElement("x")
          .WithComponent(out Image xImg, UIBuilder.plainWindowPanelBgXProperties)
          .WithComponent(out Button _, b => b.onClick.AddListener(closeCallback))
          .WithTransitions(UIBuilder.plainWindowPanelBgXTransition.WithTarget(xImg))
          .ChildOf(panelBg)
          .WithAnchor(Anchor.TopRight)
          .OfSize(21, 21)
          .At(-13, -13)
          .uiElement
          ;

      return Context;
    }

    public override PlainWindowContext WithTitle(string title)
    {
      WithPanelBg();
      Create.Text("title-text")
        .ChildOf(panelBg)
        .WithAnchor(Anchor.TopLeft)
        .At(15, -24)
        .WithLocalizer(title)
        .WithFont(UIBuilder.fontSAIRAB)
        .WithFontSize(18)
        .WithAlignment(TextAnchor.MiddleLeft)
        .WithMaterial(UIBuilder.materialWidgetTextAlpha5x)
        .WithColor(Color.white.AlphaMultiplied(0.6f))
        .WithComponent<ContentSizeFitter>(
          csf =>
          {
            csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
          });
      
      return Context;
    }
  }
}
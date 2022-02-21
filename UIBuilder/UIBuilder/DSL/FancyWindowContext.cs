using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public static partial class UIBuilderDSL
{
  public record FancyWindowContext(GameObject uiElement) : WindowContext<FancyWindowContext>(uiElement)
  {
    public override FancyWindowContext Context => this;
    protected override TranslucentImageProperties panelBgCloneImgProperties => UIBuilder.fancyWindowPanelBgProperties;
    protected override ImageProperties panelBgBorderCloneImgProperties => UIBuilder.fancyWindowPanelBgBorderProperties;
    protected override ImageProperties panelBgDragTriggerCloneImgProperties => UIBuilder.fancyWindowPanelBgDragTriggerProperties;
    protected override ImageProperties shadowCloneImgProperties => UIBuilder.fancyWindowShadowImgProperties;

    internal FancyWindowContext WithButtonBox(bool withSortBtn)
    {
      WithPanelBg();
      if (panelBg.SelectChild("btn-box") != null)
        return Context;

      var btnBoxObj = 
        Create.UIElement("btn-box")
          .WithComponent(out TranslucentImage _, UIBuilder.fancyWindowPanelBgBtnBoxProperties)
          .ChildOf(panelBg)
          .WithAnchor(Anchor.TopRight)
          .WithPivot(0.5f, 0.5f)
          .OfSize( withSortBtn ? 60 : 40, 24)
          .At(withSortBtn ? -60 : -50, -30);

      Create.UIElement("btn-border")
        .WithComponent(out Image _, UIBuilder.fancyWindowPanelBgBtnBoxBorderProperties)
        .ChildOf(btnBoxObj)
        .WithAnchor(Anchor.Stretch);

      return Context;
    }

    public FancyWindowContext WithCloseButton(UnityAction closeCallback)
    {
      using var _ = DeactivatedScope;
      
      WithButtonBox(false);
      var btnBoxObj = panelBg.SelectChild("btn-box");
      if (btnBoxObj.SelectChild("close-btn") != null)
        return Context;

      var closeBtnObj = 
        Create.UIElement("close-btn")
          .WithComponent(out Image closeBtnImg, UIBuilder.fancyWindowPanelBgBtnBoxHexBtnProperties)
          .ChildOf(btnBoxObj)
          .WithPivot(0.5f, 0.5f)
          .OfSize(38, 23)
          ;
      
      
      Create.UIElement("x")
        .WithComponent(out Image closeBtnXImg, UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnXProperties)
        .ChildOf(closeBtnObj)
        .WithPivot(0.5f, 0.5f)
        .OfSize(10, 10)
        // .At(-3, 0);
        ;
      
      Create.UIElement("col")
        .WithComponent(out Image _, UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnColProperties)
        .ChildOf(closeBtnObj)
        .WithPivot(0.5f, 0.5f)
        .OfSize(25, 19)
        // .At(-2, 0)
        ;

      closeBtnObj
        .WithComponent(out Button _, b => b.onClick.AddListener(closeCallback))
        .WithTransitions(
          UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnTransition.CloneWithTarget(closeBtnImg),
          UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnXTransition.CloneWithTarget(closeBtnXImg)
        );

      return Context;
    }

    public FancyWindowContext WithCloseAndSortButton(UnityAction closeCallback, UnityAction sortCallback)
    {
      WithButtonBox(true);
      var btnBoxObj = panelBg.SelectChild("btn-box");
      if (btnBoxObj.SelectChild("close-btn") != null)
        return Context;

      var closeBtnObj = 
        Create.UIElement("close-btn")
          .WithComponent(out Image closeBtnImg, UIBuilder.fancyWindowPanelBgBtnBoxHexBtnRProperties)
          .ChildOf(btnBoxObj)
          .WithPivot(0.5f, 0.5f)
          .OfSize(30, 24)
          .At(15, 0)
          ;
      
      var sortBtnObj = 
        Create.UIElement("sort-btn")
          .WithComponent(out Image sortBtnImg, UIBuilder.fancyWindowPanelBgBtnBoxHexBtnLProperties)
          .ChildOf(btnBoxObj)
          .WithPivot(0.5f, 0.5f)
          .OfSize(30, 24)
          .At(-15, 0)
          ;

      Create.UIElement("x")
        .WithComponent(out Image closeBtnXImg, UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnXProperties)
        .ChildOf(closeBtnObj)
        .WithPivot(0.5f, 0.5f)
        .OfSize(10, 10)
        .At(-3, 0)
        ;
      
      Create.UIElement("col")
        .WithComponent(out Image _, UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnColProperties)
        .ChildOf(closeBtnObj)
        .WithPivot(0.5f, 0.5f)
        .OfSize(25, 19)
        .At(-2, 0)
        ;
      
      Create.UIElement("x")
        .WithComponent(out Image sortBtnXImg, UIBuilder.fancyWindowPanelBgBtnBoxSortBtnXProperties)
        .ChildOf(sortBtnObj)
        .WithPivot(0.5f, 0.5f)
        .OfSize(10, 10)
        .At(3, 0)
        ;
      
      Create.UIElement("col")
        .WithComponent(out Image _, UIBuilder.fancyWindowPanelBgBtnBoxSortBtnColProperties)
        .ChildOf(sortBtnObj)
        .WithPivot(0.5f, 0.5f)
        .OfSize(25, 19)
        .At(2, 0)
        ;
      
      closeBtnObj
        .WithComponent(out Button _, b => b.onClick.AddListener(closeCallback))
        .WithTransitions(
            UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnTransition.CloneWithTarget(closeBtnImg),
            UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnXTransition.CloneWithTarget(closeBtnXImg)
          );
      
      sortBtnObj
        .WithComponent(out Button _, b => b.onClick.AddListener(sortCallback))
        .WithTransitions(
            UIBuilder.fancyWindowPanelBgBtnBoxSortBtnTransition.CloneWithTarget(sortBtnImg),
            UIBuilder.fancyWindowPanelBgBtnBoxSortBtnXTransition.CloneWithTarget(sortBtnXImg)
          );

      return Context;
    }

    public override FancyWindowContext WithTitle(string title)
    {
      WithPanelBg();
      Create.Text("title-text")
        .ChildOf(panelBg)
        .WithAnchor(Anchor.TopLeft)
        .At(50, -38)
        .WithLocalizer(title)
        .WithFont(UIBuilder.fontSAIRAB)
        .WithFontSize(18)
        .WithAlignment(TextAnchor.MiddleLeft)
        .WithContentSizeFitter(horizontal: ContentSizeFitter.FitMode.PreferredSize);

      return Context;
    }
  }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
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
    
    public UIButton closeUIButton { get; set; }
    public UIButton sortUIButton { get; set; }

    internal FancyWindowContext WithButtonBox(bool withSortBtn)
    {
      WithPanelBg();
      var panelBgObj = uiElement.SelectDescendant("panel-bg");
      if (panelBgObj.SelectChild("btn-box") != null)
        return this;

      var btnBoxObj = 
        Create.UIElement("btn-box")
          .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBox)
          .ChildOf(panelBgObj)
          .WithAnchor(Anchor.TopRight)
          .WithPivot(0.5f, 0.5f)
          .OfSize( withSortBtn ? 60 : 40, 24)
          .At(withSortBtn ? -60 : -50, -30);

      Create.UIElement("btn-border")
        .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxBorder)
        .ChildOf(btnBoxObj)
        .WithAnchor(Anchor.Stretch);

      return this;
    }

    internal FancyWindowContext WithCloseButton(GameObject closeBtnObj, System.Action closeCallback)
    {
      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);
      
      var unityButton = closeBtnObj.GetOrCreateComponent<Button>();
      closeUIButton = closeBtnObj.GetOrCreateComponent<UIButton>();
      closeUIButton.CopyFrom(UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnUIButton);

      if (closeUIButton.transitions.Length >= 2)
      {
        closeUIButton.transitions[0].target =
          closeBtnObj.GetComponent<Image>();
        closeUIButton.transitions[1].target =
          closeBtnObj.SelectDescendant("x").GetComponent<Image>();
      }

      if (wasActive)
        uiElement.SetActive(true);
      
      unityButton.onClick.AddListener(new UnityAction(closeCallback));

      return this;
    }
    
    internal FancyWindowContext WithSortButton(GameObject sortBtnObj, System.Action sortCallback)
    {
      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);
      
      var unityButton = sortBtnObj.GetOrCreateComponent<Button>();
      sortUIButton = sortBtnObj.GetOrCreateComponent<UIButton>();
      sortUIButton.CopyFrom(UIBuilder.fancyWindowPanelBgBtnBoxSortBtnUIButton);

      if (sortUIButton.transitions.Length >= 2)
      {
        sortUIButton.transitions[0].target =
          sortBtnObj.GetComponent<Image>();
        sortUIButton.transitions[1].target =
          sortBtnObj.SelectDescendant("x").GetComponent<Image>();
      }

      if (wasActive)
        uiElement.SetActive(true);
      
      unityButton.onClick.AddListener(new UnityAction(sortCallback));

      return this;
    }

    public FancyWindowContext WithCloseButton(System.Action closeCallback)
    {
      WithButtonBox(false);
      var btnBoxObj = uiElement.SelectDescendant("panel-bg", "btn-box");
      if (btnBoxObj.SelectChild("close-btn") != null)
        return this;

      var closeBtnObj = 
        Create.UIElement("close-btn")
          .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxHexBtn)
          .ChildOf(btnBoxObj)
          .OfSize(38, 23)
          ;
      
      
      Create.UIElement("x")
        .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnX)
        .ChildOf(closeBtnObj)
        .OfSize(10, 10)
        // .At(-3, 0);
        ;
      
      Create.UIElement("col")
        .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnCol)
        .ChildOf(closeBtnObj)
        .OfSize(25, 19)
        // .At(-2, 0)
        ;

      return WithCloseButton(closeBtnObj.uiElement, closeCallback);
    }

    public FancyWindowContext WithCloseAndSortButton(System.Action closeCallback, System.Action sortCallback)
    {
      
      WithButtonBox(true);
      var btnBoxObj = uiElement.SelectDescendant("panel-bg", "btn-box");
      if (btnBoxObj.SelectChild("close-btn") != null)
        return this;

      var closeBtnObj = 
        Create.UIElement("close-btn")
          .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxHexBtnR)
          .ChildOf(btnBoxObj)
          .OfSize(30, 24)
          .At(15, 0)
          ;
      
      var sortBtnObj = 
        Create.UIElement("sort-btn")
          .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxHexBtnL)
          .ChildOf(btnBoxObj)
          .OfSize(30, 24)
          .At(-15, 0)
          ;

      Create.UIElement("x")
        .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnX)
        .ChildOf(closeBtnObj)
        .OfSize(10, 10)
        .At(-3, 0)
        ;
      
      Create.UIElement("col")
        .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxCloseBtnCol)
        .ChildOf(closeBtnObj)
        .OfSize(25, 19)
        .At(-2, 0)
        ;
      
      Create.UIElement("x")
        .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxSortBtnX)
        .ChildOf(sortBtnObj)
        .OfSize(10, 10)
        .At(3, 0)
        ;
      
      Create.UIElement("col")
        .CloneComponentFrom(UIBuilder.fancyWindowPanelBgBtnBoxSortBtnCol)
        .ChildOf(sortBtnObj)
        .OfSize(25, 19)
        .At(2, 0)
        ;

      return
        WithCloseButton(closeBtnObj.uiElement, closeCallback)
          .WithSortButton(sortBtnObj.uiElement, sortCallback)
          ;
    }
  }
}
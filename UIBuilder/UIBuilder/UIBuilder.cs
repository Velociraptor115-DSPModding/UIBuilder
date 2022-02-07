using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using HarmonyLib;

namespace DysonSphereProgram.Modding.UI
{
  public static class UIBuilder
  {
    public static bool isReady = false;
    private static List<System.Action> readyCallbacks = new List<System.Action>();

    public static void QueueReadyCallback(System.Action callback)
    {
      if (isReady)
      {
        callback();
        return;
      }

      readyCallbacks.Add(callback);
    }

    private static void Ready()
    {
      isReady = true;
      foreach (var callback in readyCallbacks)
      {
        callback();
      }
      readyCallbacks.Clear();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIGame), nameof(UIGame._OnCreate))]
    static void UIGame___OnCreate()
    {
      Create();
    }

    //[HarmonyPostfix]
    //[HarmonyPatch(typeof(UIGame), nameof(UIGame._OnUpdate))]
    //static void UIGame___OnUpdate()
    //{
      
    //}

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(UIGame), nameof(UIGame._OnFree))]
    //static void UIGame___OnFree()
    //{
      
    //}

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIGame), nameof(UIGame._OnDestroy))]
    static void UIGame___OnDestroy()
    {
      Destroy();
    }


    public static Image plainWindowShadowImg;
    public static Image fancyWindowShadowImg;

    public static TranslucentImage plainWindowPanelBg;
    public static TranslucentImage fancyWindowPanelBg;
    public static Image plainWindowPanelBgBorder;
    public static Image fancyWindowPanelBgBorder;
    public static Image plainWindowPanelBgDragTrigger;
    public static Image fancyWindowPanelBgDragTrigger;

    public static TranslucentImage fancyWindowPanelBgBtnBox;
    public static Image fancyWindowPanelBgBtnBoxBorder;
    public static Image fancyWindowPanelBgBtnBoxHexBtn;
    public static Image fancyWindowPanelBgBtnBoxHexBtnR;
    public static Image fancyWindowPanelBgBtnBoxCloseBtnX;
    public static Image fancyWindowPanelBgBtnBoxCloseBtnCol;
    public static Image fancyWindowPanelBgBtnBoxHexBtnL;
    public static Image fancyWindowPanelBgBtnBoxSortBtnX;
    public static Image fancyWindowPanelBgBtnBoxSortBtnCol;
    public static UIButton fancyWindowPanelBgBtnBoxCloseBtnUIButton;
    public static UIButton fancyWindowPanelBgBtnBoxSortBtnUIButton;

    public static Image plainWindowPanelBgX;
    public static UIButton plainWindowPanelBgCloseUIButton;

    public static Image scrollBgImg;
    public static Image scrollHandleImg;
    public static Selectable scrollSelectable;
    public static ScrollRect scrollRect;

    public static void Create()
    {
      CaptureGameAssets();
      CreatePrefabs();
      Ready();
    }

    public static void Destroy()
    {
      DestroyPrefabs();
      ReleaseGameAssets();
    }

    public static void CaptureGameAssets()
    {
      {
        var obj = UIRoot.instance.uiGame.statWindow.gameObject;
        plainWindowShadowImg = obj.SelectDescendant("shadow").GetComponent<Image>();
        plainWindowPanelBg = obj.SelectDescendant("panel-bg").GetComponent<TranslucentImage>();
        plainWindowPanelBgBorder = obj.SelectDescendant("panel-bg", "border").GetComponent<Image>();
        plainWindowPanelBgDragTrigger = obj.SelectDescendant("panel-bg", "drag-trigger").GetComponent<Image>();
        
        plainWindowPanelBgX = obj.SelectDescendant("panel-bg", "x").GetComponent<Image>();
        plainWindowPanelBgCloseUIButton = obj.SelectDescendant("panel-bg", "x").GetComponent<UIButton>();
      }

      {
        var obj = UIRoot.instance.uiGame.assemblerWindow.gameObject;
        fancyWindowShadowImg = obj.SelectDescendant("shadow").GetComponent<Image>();
        fancyWindowPanelBg = obj.SelectDescendant("panel-bg").GetComponent<TranslucentImage>();
        fancyWindowPanelBgBorder = obj.SelectDescendant("panel-bg", "border").GetComponent<Image>();
        fancyWindowPanelBgBorder = obj.SelectDescendant("panel-bg", "border").GetComponent<Image>();
        fancyWindowPanelBgDragTrigger = obj.SelectDescendant("panel-bg", "drag-trigger").GetComponent<Image>();

        fancyWindowPanelBgBtnBox = obj.SelectDescendant("panel-bg", "btn-box").GetComponent<TranslucentImage>();
        fancyWindowPanelBgBtnBoxBorder = obj.SelectDescendant("panel-bg", "btn-box", "btn-border").GetComponent<Image>();
        fancyWindowPanelBgBtnBoxHexBtn = obj.SelectDescendant("panel-bg", "btn-box", "close-btn").GetComponent<Image>();
        
        obj = UIRoot.instance.uiGame.inventory.gameObject;
        fancyWindowPanelBgBtnBoxHexBtnR = obj.SelectDescendant("panel-bg", "btn-box", "close-btn").GetComponent<Image>();
        fancyWindowPanelBgBtnBoxCloseBtnUIButton = obj.SelectDescendant("panel-bg", "btn-box", "close-btn").GetComponent<UIButton>();
        fancyWindowPanelBgBtnBoxCloseBtnX = obj.SelectDescendant("panel-bg", "btn-box", "close-btn", "x").GetComponent<Image>();
        fancyWindowPanelBgBtnBoxCloseBtnCol = obj.SelectDescendant("panel-bg", "btn-box", "close-btn", "col").GetComponent<Image>();
        fancyWindowPanelBgBtnBoxHexBtnL = obj.SelectDescendant("panel-bg", "btn-box", "sort-btn").GetComponent<Image>();
        fancyWindowPanelBgBtnBoxSortBtnUIButton = obj.SelectDescendant("panel-bg", "btn-box", "sort-btn").GetComponent<UIButton>();
        fancyWindowPanelBgBtnBoxSortBtnX = obj.SelectDescendant("panel-bg", "btn-box", "sort-btn", "x").GetComponent<Image>();
        fancyWindowPanelBgBtnBoxSortBtnCol = obj.SelectDescendant("panel-bg", "btn-box", "sort-btn", "col").GetComponent<Image>();
      }

      {
        var obj = UIRoot.instance.uiGame.statWindow.achievementPanelUI.gameObject;
        scrollRect = obj.SelectDescendant("scroll-view").GetOrCreateComponent<ScrollRect>();
        scrollSelectable = obj.SelectDescendant("scroll-view", "v-bar").GetComponent<Scrollbar>();
        scrollBgImg = obj.SelectDescendant("scroll-view", "v-bar").GetComponent<Image>();
        scrollHandleImg = obj.SelectDescendant("scroll-view", "v-bar", "Sliding Area", "Handle").GetComponent<Image>();
      }
    }

    public static void ReleaseGameAssets()
    {
      plainWindowShadowImg = null;
      plainWindowPanelBg = null;
      plainWindowPanelBgBorder = null;
      plainWindowPanelBgDragTrigger = null;

      fancyWindowShadowImg = null;
      fancyWindowPanelBg = null;
      fancyWindowPanelBgBorder = null;
      fancyWindowPanelBgDragTrigger = null;
    }

    public static void CreatePrefabs()
    {

    }

    public static void DestroyPrefabs()
    {

    }
	}
}

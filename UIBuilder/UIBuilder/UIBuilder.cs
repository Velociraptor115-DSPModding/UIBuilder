using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using HarmonyLib;

namespace DysonSphereProgram.Modding.UI.Builder
{
  using static GraphicPropertiesBuilder;
  public static class UIBuilder
  {
    public static bool isReady = false;
    private static List<System.Action> readyCallbacks = new();

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


    public static readonly HashSet<object> inScrollView = new();
    [HarmonyPostfix]
    [HarmonyPatch(typeof(VFInput), nameof(VFInput.UpdateGameStates))]
    static void PatchInScrollView()
    {
      VFInput.inScrollView = VFInput.inScrollView || inScrollView.Count > 0;
    }
    
    public static ImageProperties plainWindowShadowImgProperties;
    public static ImageProperties fancyWindowShadowImgProperties;
    
    public static TranslucentImageProperties plainWindowPanelBgProperties;
    public static TranslucentImageProperties fancyWindowPanelBgProperties;
    public static ImageProperties plainWindowPanelBgBorderProperties;
    public static ImageProperties fancyWindowPanelBgBorderProperties;
    public static ImageProperties plainWindowPanelBgDragTriggerProperties;
    public static ImageProperties fancyWindowPanelBgDragTriggerProperties;
    
    public static TranslucentImageProperties fancyWindowPanelBgBtnBoxProperties;
    public static ImageProperties fancyWindowPanelBgBtnBoxBorderProperties;
    public static ImageProperties fancyWindowPanelBgBtnBoxHexBtnProperties;
    public static ImageProperties fancyWindowPanelBgBtnBoxHexBtnRProperties;
    public static ImageProperties fancyWindowPanelBgBtnBoxCloseBtnXProperties;
    public static ImageProperties fancyWindowPanelBgBtnBoxCloseBtnColProperties;
    public static ImageProperties fancyWindowPanelBgBtnBoxHexBtnLProperties;
    public static ImageProperties fancyWindowPanelBgBtnBoxSortBtnXProperties;
    public static ImageProperties fancyWindowPanelBgBtnBoxSortBtnColProperties;
    public static UIButton.Transition fancyWindowPanelBgBtnBoxCloseBtnTransition;
    public static UIButton.Transition fancyWindowPanelBgBtnBoxCloseBtnXTransition;
    public static UIButton.Transition fancyWindowPanelBgBtnBoxSortBtnTransition;
    public static UIButton.Transition fancyWindowPanelBgBtnBoxSortBtnXTransition;
    
    public static ImageProperties plainWindowPanelBgXProperties;
    public static UIButton.Transition plainWindowPanelBgXTransition;
    
    public static ImageProperties scrollBgImgProperties;
    public static ImageProperties scrollHandleImgProperties;
    public static ScrollRectProperties scrollRectProperties;
    
    public static ImageProperties buttonImgProperties;
    public static SelectableProperties buttonSelectableProperties;

    public static Font fontSAIRASB;
    public static Font fontSAIRAB;
    public static Font fontSAIRAL;
    public static Font fontSAIRAR;
    public static Font fontSAIRAM;
    public static Font fontSAIRAT;
    public static Font fontDIN;
    public static Font fontVERDANAB;
    public static Font fontSTXIHEI;
    public static Font fontAGENCYB;
    public static Font fontFZXDXJW;
    public static Font fontKNUCK;

    public static UIComboBox comboBoxComponent;
    
    public const string stringFullPlus = "＋";
    public const string stringFullMinus = "－";

    public static Sprite spriteBorder1;
    public static Sprite spriteBorder2;
    public static Sprite spriteRectP1;
    public static Sprite spriteRectP2;

    public static Sprite spriteWindowBorder1;
    public static Sprite spriteWindowBorder2;
    public static Sprite spriteWindowBorder3;
    public static Sprite spriteWindowBorder4;
    public static Sprite spriteWindowBorder5;
    public static Sprite spriteWindowContent1;
    public static Sprite spriteWindowContent2;
    public static Sprite spriteWindowContent3;
    public static Sprite spriteWindowContent4;
    public static Sprite spriteWindowContent5;
    public static Sprite spriteWindowTrsl1;
    public static Sprite spriteWindowTrsl1Shadow;
    public static Sprite spritePanel1;
    public static Sprite spritePanel2;
    public static Sprite spritePanel3;
    public static Sprite spritePanel4;
    public static Sprite spritePanel5;
    public static Sprite spritePanel6;

    public static Sprite spriteLitterPanelCC;
    public static Sprite spriteBoxBlur;
    public static Sprite spriteXIcon;
    public static Sprite spriteRefreshIcon;
    public static Sprite spriteBar4px;
    public static Sprite spriteRound256;
    public static Sprite spriteHexBg;
    public static Sprite spriteHexBorder;
    public static Sprite spriteHexBorderUp;
    public static Sprite spriteHexBtnLBg;
    public static Sprite spriteHexBtnRBg;

    public static Material materialTrslColored;
    public static Material materialTrslLine;
    public static Material materialTrslPanel0;
    public static Material materialTrslPanel1;
    public static Material materialTrslPanel2;
    public static Material materialTrslPanel3;
    public static Material materialTrslPanel4;
    public static Material materialTrslPanelClear;
    public static Material materialWidgetAdd5x;
    public static Material materialWidgetAlpha5x;
    public static Material materialWidgetAlpha5xAudio;
    public static Material materialWidgetAlpha5xNosharp;
    public static Material materialWidgetTextAlpha5x;
    public static Material materialWidgetTextAlpha5xThick;

    public static readonly Color windowShadowColor = new(0, 0, 0, 0.3137f);
    public static readonly Color borderColor = new(0.6557f, 0.9145f, 1f, 1f);
    public static readonly Color plainWindowPanelBgColor = new(0.0829f, 0.112f, 0.1412f, 1);
    public static readonly Color plainWindowPanelBgXColor = new(1, 1, 1, 0.0706f);
    public static readonly Color plainWindowPanelBgXTransitionColor = new(0.6549f, 0.9137f, 1f, 1f);
    public static readonly Color fancyWindowPanelBgColor = new(0f, 0.1744f, 0.2157f, 1f);
    public static readonly Color fancyWindowPanelBgBtnBoxColor = new(0f, 0.1744f, 0.2157f, 1f);
    public static readonly Color fancyWindowPanelBgBtnBoxHexBtnColor = new(0.3804f, 0.8431f, 1f, 0.0262f);
    public static readonly Color fancyWindowPanelBgBtnBoxCloseBtnColor = new(0.3821f, 0.8455f, 1f, 1f);
    public static readonly Color fancyWindowPanelBgBtnBoxHexBtnBgColor = new(0, 0, 0, 0.0824f);
    public static readonly Color scrollBgColor = new(0f, 0f, 0f, 0.7686f);

    public static readonly Color buttonBlueColor = new(0.2972f, 0.6886f, 1f, 0.8471f);

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
        materialTrslColored            = Resources.Load<Material>("ui/materials/trsl-colored");
        materialTrslLine               = Resources.Load<Material>("ui/materials/trsl-line");
        materialTrslPanel0             = Resources.Load<Material>("ui/materials/trsl-panel-0");
        materialTrslPanel1             = Resources.Load<Material>("ui/materials/trsl-panel-1");
        materialTrslPanel2             = Resources.Load<Material>("ui/materials/trsl-panel-2");
        materialTrslPanel3             = Resources.Load<Material>("ui/materials/trsl-panel-3");
        materialTrslPanel4             = Resources.Load<Material>("ui/materials/trsl-panel-4");
        materialTrslPanelClear         = Resources.Load<Material>("ui/materials/trsl-panel-clear");
        materialWidgetAdd5x            = Resources.Load<Material>("ui/materials/widget-add-5x");
        materialWidgetAlpha5x          = Resources.Load<Material>("ui/materials/widget-alpha-5x");
        materialWidgetAlpha5xAudio     = Resources.Load<Material>("ui/materials/widget-alpha-5x-audio");
        materialWidgetAlpha5xNosharp   = Resources.Load<Material>("ui/materials/widget-alpha-5x-nosharp");
        materialWidgetTextAlpha5x      = Resources.Load<Material>("ui/materials/widget-text-alpha-5x");
        materialWidgetTextAlpha5xThick = Resources.Load<Material>("ui/materials/widget-text-alpha-5x-thick");
      }

      {
        spriteBorder1 = Resources.Load<Sprite>("textures/border1");
        spriteBorder2 = Resources.Load<Sprite>("textures/border2");
        spriteRectP1  = Resources.Load<Sprite>("textures/rect_p1");
        spriteRectP2  = Resources.Load<Sprite>("textures/rect_p2");
        
        spriteWindowBorder1     = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-border-1");
        spriteWindowBorder2     = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-border-2");
        spriteWindowBorder3     = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-border-3");
        spriteWindowBorder4     = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-border-4");
        spriteWindowBorder5     = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-border-5");
        spriteWindowContent1    = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-content-1");
        spriteWindowContent2    = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-content-2");
        spriteWindowContent3    = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-content-3");
        spriteWindowContent4    = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-content-4");
        spriteWindowContent5    = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-content-5");
        spriteWindowTrsl1       = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-trsl-1");
        spriteWindowTrsl1Shadow = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/window-trsl-1-sd");
        
        spritePanel1 = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/panel-1");
        spritePanel2 = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/panel-2");
        spritePanel3 = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/panel-3");
        spritePanel4 = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/panel-4");
        spritePanel5 = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/panel-5");
        spritePanel6 = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/panel-6");

        spriteLitterPanelCC = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/litter-panel-cc");
        spriteBoxBlur       = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/box-blur");
        spriteXIcon         = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/x-icon");
        spriteRefreshIcon   = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/refresh-icon");
        spriteBar4px        = Resources.Load<Sprite>("ui/textures/sprites/bar-4px");
        spriteRound256      = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/round-256");
        spriteHexBg         = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/hex-bg");
        spriteHexBorder     = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/hex-border");
        spriteHexBorderUp   = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/hex-border-up");
        spriteHexBtnLBg     = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/hex-btn-l-bg");
        spriteHexBtnRBg     = Resources.Load<Sprite>("ui/textures/sprites/sci-fi/hex-btn-r-bg");
      }
      
      {
        fontSAIRASB  = Resources.Load<Font>("ui/fonts/SAIRASB");
        fontSAIRAB   = Resources.Load<Font>("ui/fonts/SAIRAB");
        fontSAIRAL   = Resources.Load<Font>("ui/fonts/SAIRAL");
        fontSAIRAR   = Resources.Load<Font>("ui/fonts/SAIRAR");
        fontSAIRAM   = Resources.Load<Font>("ui/fonts/SAIRAM");
        fontSAIRAT   = Resources.Load<Font>("ui/fonts/SAIRAT");
        fontDIN      = Resources.Load<Font>("ui/fonts/DIN");
        fontVERDANAB = Resources.Load<Font>("ui/fonts/VERDANAB");
        fontSTXIHEI  = Resources.Load<Font>("ui/fonts/STXIHEI");
        fontAGENCYB  = Resources.Load<Font>("ui/fonts/AGENCYB");
        fontFZXDXJW  = Resources.Load<Font>("ui/fonts/FZXDXJW");
        fontKNUCK    = Resources.Load<Font>("ui/fonts/KNUCK");
      }

      var noRaycastImage = ImagePropertiesDefault.WithRaycast(false);
      var colliderImage = ImagePropertiesDefault.WithColor(Color.clear);
      var windowPanelBgImg =
        TranslucentImagePropertiesDefault
          .WithRaycast(false)
          with
          {
            vibrancy = 1.2f, brightness = 0.3f, flatten = 0.005f, spriteBlending = 0.56f
            , updating = false, lateUpdating = false,
          };

      {
        plainWindowShadowImgProperties =
          noRaycastImage
            .WithSlicedSprite(spriteBoxBlur)
            .WithColor(windowShadowColor);
        
        plainWindowPanelBgProperties =
          windowPanelBgImg
            .WithSlicedSprite(spriteRectP1)
            .WithMaterial(materialTrslPanel0)
            .WithColor(plainWindowPanelBgColor);

        plainWindowPanelBgBorderProperties =
          noRaycastImage
            .WithSlicedSprite(spriteWindowBorder5)
            .WithColor(borderColor.AlphaMultiplied(0.1176f));

        plainWindowPanelBgDragTriggerProperties = colliderImage;

        plainWindowPanelBgXProperties =
          ImagePropertiesDefault
            .WithSprite(spriteXIcon)
            .WithMaterial(materialWidgetAlpha5x)
            .WithColor(plainWindowPanelBgXColor);
        
        plainWindowPanelBgXTransition = new()
        {
          damp = 0.3f,
          mouseoverColor = plainWindowPanelBgXTransitionColor.AlphaMultiplied(0.3765f),
          mouseoverSize = 1f,
          pressedColor = plainWindowPanelBgXTransitionColor.AlphaMultiplied(0.3765f),
          pressedSize = 1
        };
      }

      {
        fancyWindowShadowImgProperties =
          noRaycastImage
            .WithSlicedSprite(spriteWindowTrsl1Shadow)
            .WithColor(windowShadowColor);

        fancyWindowPanelBgProperties = new()
        {
          vibrancy = 1.1f,
          brightness = -0.5f,
          flatten = 0.005f,
          spriteBlending = 0.7f,

          sprite = spriteWindowTrsl1,
          type = Image.Type.Sliced,
          
          material = materialTrslPanel3,
          color = fancyWindowPanelBgColor,
          raycastTarget = false
        };
        
        fancyWindowPanelBgBorderProperties = new()
        {
          sprite = spriteWindowBorder1,
          type = Image.Type.Sliced,
          
          material = materialWidgetAlpha5x,
          color = borderColor.AlphaMultiplied(0.2941f),
          raycastTarget = false
        };

        fancyWindowPanelBgDragTriggerProperties = colliderImage;
        
        fancyWindowPanelBgBtnBoxProperties = new()
        {
          vibrancy = 1.2f,
          brightness = 0.3f,
          flatten = 0.005f,
          spriteBlending = 0.7f,
          
          sprite = spriteHexBg,
          type = Image.Type.Sliced,
          
          material = materialTrslPanel0,
          color = fancyWindowPanelBgBtnBoxColor,
          raycastTarget = false
        };
        
        fancyWindowPanelBgBtnBoxBorderProperties = new()
        {
          sprite = spriteHexBorderUp,
          type = Image.Type.Sliced,
          
          material = materialWidgetAlpha5x,
          color = borderColor.AlphaMultiplied(0.2941f),
          raycastTarget = false
        };
        
        fancyWindowPanelBgBtnBoxHexBtnProperties = new()
        {
          sprite = spriteHexBg,
          type = Image.Type.Sliced,
          
          material = materialWidgetAlpha5x,
          color = fancyWindowPanelBgBtnBoxHexBtnColor,
          raycastTarget = false
        };
        
        fancyWindowPanelBgBtnBoxHexBtnRProperties = new()
        {
          sprite = spriteHexBtnRBg,
          type = Image.Type.Sliced,
          
          material = materialWidgetAlpha5x,
          color = fancyWindowPanelBgBtnBoxHexBtnBgColor,
          raycastTarget = false
        };
        
        fancyWindowPanelBgBtnBoxCloseBtnTransition = new()
        {
          damp = 0.3f,
          highlightAlphaMultiplier = 1f,
          highlightColorMultiplier = 1f,
          highlightSizeMultiplier = 1f,
          mouseoverColor = fancyWindowPanelBgBtnBoxCloseBtnColor.AlphaMultiplied(0.0471f),
          mouseoverSize = 1f,
          pressedColor = fancyWindowPanelBgBtnBoxCloseBtnColor.AlphaMultiplied(0.0196f),
          pressedSize = 1
        };
        fancyWindowPanelBgBtnBoxCloseBtnXTransition = new()
        {
          damp = 0.3f,
          disabledColor = Color.black.AlphaMultiplied(0.2157f),
          highlightAlphaMultiplier = 1f,
          highlightColorMultiplier = 1f,
          highlightSizeMultiplier = 1f,
          mouseoverColor = borderColor.AlphaMultiplied(0.8118f),
          mouseoverSize = 1f,
          pressedColor = fancyWindowPanelBgBtnBoxCloseBtnColor.AlphaMultiplied(0.502f),
          pressedSize = 1
        };
        fancyWindowPanelBgBtnBoxCloseBtnXProperties = new()
        {
          sprite = spriteXIcon,

          material = materialWidgetAlpha5x,
          color = borderColor.AlphaMultiplied(0.3765f),
          raycastTarget = false
        };
        
        fancyWindowPanelBgBtnBoxCloseBtnColProperties = new()
        {
          material = materialWidgetAlpha5x, // probably unnecessary
          color = Color.clear,
        };
        
        fancyWindowPanelBgBtnBoxHexBtnLProperties = new()
        {
          sprite = spriteHexBtnLBg,
          type = Image.Type.Sliced,
          
          material = materialWidgetAlpha5x,
          color = fancyWindowPanelBgBtnBoxHexBtnBgColor,
          raycastTarget = false
        };
        
        fancyWindowPanelBgBtnBoxSortBtnTransition = fancyWindowPanelBgBtnBoxCloseBtnTransition.WithTarget(null);
        fancyWindowPanelBgBtnBoxSortBtnXTransition = fancyWindowPanelBgBtnBoxCloseBtnXTransition.WithTarget(null);
        fancyWindowPanelBgBtnBoxSortBtnXProperties = new()
        {
          sprite = spriteRefreshIcon,

          material = materialWidgetAlpha5x,
          color = borderColor.AlphaMultiplied(0.3765f),
          raycastTarget = false
        };
        
        fancyWindowPanelBgBtnBoxSortBtnColProperties = new()
        {
          material = materialWidgetAlpha5x, // probably unnecessary
          color = Color.clear,
        };
      }

      {
        scrollRectProperties = new()
        {
          movementType = ScrollRect.MovementType.Clamped,
          decelerationRate = 0,
          scrollSensitivity = 100
        };
        
        scrollBgImgProperties = new()
        {
          // type = Image.Type.Sliced,
          color = scrollBgColor 
        };
        
        scrollHandleImgProperties = new()
        {
          // type = Image.Type.Sliced,
          color = borderColor
        };
      }

      {
        buttonImgProperties = new()
        {
          sprite = spriteLitterPanelCC,
          type = Image.Type.Sliced,
          
          color = buttonBlueColor,
        };
        
        buttonSelectableProperties = new()
        {
          colors = ColorBlock.defaultColorBlock with
          {
            normalColor = Color.white.AlphaMultiplied(0.75f),
            highlightedColor = Color.white,
            pressedColor = Color.white.AlphaMultiplied(0.3725f),
            disabledColor = Color.black
          }
        };
      }

      {
        comboBoxComponent = UIRoot.instance.uiGame.monitorWindow.speakerWarningCombo;
      }
    }

    public static void ReleaseGameAssets()
    {
      
    }

    public static void CreatePrefabs()
    {

    }

    public static void DestroyPrefabs()
    {

    }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;
public static partial class UIBuilderDSL
{
  public record struct SliderHandleConfiguration(
    float handleLength = 50
    , float handleToGutterWidthRatio = 0.5f
    , RectTransform handleObj = null
  );

  public record struct SliderConfiguration(
    float minValue = 0
    , float maxValue = 10
    , bool wholeNumbers = false
    , Slider.Direction direction = Slider.Direction.LeftToRight
    , RectTransform backgroundObj = null
    , RectTransform fillObj = null
    , float borderPadding = 6
    , SliderHandleConfiguration? handle = null
  );

  public record SliderContext(GameObject uiElement) : UIElementContextBase<SliderContext>(uiElement)
  {
    public override SliderContext Context => this;
    
    public SliderContext WithSliderConfig(SliderConfiguration configuration)
    {
      using var _ = DeactivatedScope;
      
      var slider = uiElement.GetOrCreateComponent<Slider>();

      slider.direction = configuration.direction;
      slider.minValue = configuration.minValue;
      slider.maxValue = configuration.maxValue;
      slider.wholeNumbers = configuration.wholeNumbers;

      var effectiveHandleToGutterWidthRatio =
        Mathf.Clamp(configuration.handle?.handleToGutterWidthRatio ?? 0f, 0f, 1f);
      var gutterAnchorOffset = (1f - effectiveHandleToGutterWidthRatio) / 2f;

      var minGutterAnchor = slider.axis switch
      {
        Slider.Axis.Horizontal => new Vector2(0, 0.5f - gutterAnchorOffset),
        Slider.Axis.Vertical => new Vector2(0.5f - gutterAnchorOffset, 0),
        _ => throw new ArgumentOutOfRangeException()
      };
      
      var maxGutterAnchor = slider.axis switch
      {
        Slider.Axis.Horizontal => new Vector2(1, 0.5f + gutterAnchorOffset),
        Slider.Axis.Vertical => new Vector2(0.5f + gutterAnchorOffset, 1),
        _ => throw new ArgumentOutOfRangeException()
      };

      var backgroundAreaObj =
        Create.UIElement("background-area")
          .ChildOf(uiElement)
          .WithMinMaxAnchor(minGutterAnchor, maxGutterAnchor)
          .At(0, 0)
          .uiElement;

      static RectTransform CreateDefaultBackgroundObj()
      {
        var backgroundObj = Create.UIElement("background");

        using (backgroundObj.DeactivatedScope)
        {
          var backgroundImg = backgroundObj.uiElement.GetOrCreateComponent<Image>();
          backgroundImg.color = Color.black.AlphaMultiplied(0.8f);
        }
        
        return backgroundObj.transform;
      }

      var backgroundObj =
        Select.UIElement((configuration.backgroundObj ? configuration.backgroundObj : CreateDefaultBackgroundObj()).gameObject)
          .ChildOf(backgroundAreaObj)
          .WithAnchor(Anchor.Stretch).At(0, 0)
          .uiElement;
      
      var fillAreaObj =
        Create.UIElement("fill-area")
          .ChildOf(uiElement)
          .WithAnchor(Anchor.Stretch).At(0, 0)
          .OfSize(-configuration.borderPadding, -configuration.borderPadding)
          .uiElement;
      
      static RectTransform CreateDefaultFillObj()
      {
        var fillObj = Create.UIElement("fill");

        using (fillObj.DeactivatedScope)
        {
          var fillImg = fillObj.uiElement.GetOrCreateComponent<Image>();
          fillImg.color = Color.white.AlphaMultiplied(0.8f);
        }
        
        return fillObj.transform;
      }

      var fill =
        Select.UIElement((configuration.fillObj ? configuration.fillObj : CreateDefaultFillObj()).gameObject)
          .ChildOf(fillAreaObj)
          .WithAnchor(Anchor.Stretch).At(0, 0)
          .uiElement;

      slider.fillRect = fill.GetComponent<RectTransform>();

      if (configuration.handle.HasValue)
      {
        var handleConfiguration = configuration.handle.Value;
        var handleSize = handleConfiguration.handleLength;
      
        var handleAreaObj =
          Create.UIElement("handle-area")
            .ChildOf(uiElement)
            .WithAnchor(Anchor.Stretch).At(0, 0)
            .WithOffsetMinX(handleSize / 2f)
            .WithOffsetMaxX(-handleSize / 2f)
            .uiElement;
        
        static RectTransform CreateDefaultHandleObj()
        {
          var handleObj = Create.UIElement("handle");

          using (handleObj.DeactivatedScope)
          {
            var handleImg = handleObj.uiElement.GetOrCreateComponent<Image>();
            handleImg.color = Color.black.AlphaMultiplied(0.45f);
          }
        
          return handleObj.transform;
        }
      
        var handle =
          Select.UIElement((handleConfiguration.handleObj ? handleConfiguration.handleObj : CreateDefaultHandleObj()).gameObject)
            .ChildOf(handleAreaObj)
            .WithAnchor(Anchor.StretchCenter).At(0, 0)
            .OfSize(handleSize, 0)
            .uiElement;

        slider.handleRect = handle.GetComponent<RectTransform>();
      }

      return Context;
    }

    public SliderContext Bind(IDataBindSource<float> binding)
    {
      using var _ = DeactivatedScope;
      
      var bindingController = uiElement.GetOrCreateComponent<DataBindSlider>();
      bindingController.Binding = binding;
      
      return Context;
    }
  }
}
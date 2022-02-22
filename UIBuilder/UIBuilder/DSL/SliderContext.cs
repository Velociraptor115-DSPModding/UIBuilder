using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface ISliderSelectableContext : ISelectableContext
{
  Slider slider { get; }
}

public record struct SliderHandleConfiguration(
  float handleLength = 50
  , float handleToGutterWidthRatio = 0.5f
  , Image handle = null
);

public record struct SliderConfiguration(
  float minValue = 0
  , float maxValue = 10
  , bool wholeNumbers = false
  , Slider.Direction direction = Slider.Direction.LeftToRight
  , Image background = null
  , Image fill = null
  , float borderPadding = 6
  , SliderHandleConfiguration? handle = null
);

public record SliderContext(Slider slider, Image fill, Image handle = null) : UIElementContext(slider.gameObject), ISliderSelectableContext
{
  public Selectable selectable => slider;
}

public static class SliderContextExtensions
{
  public static SliderContext Select(Slider slider, Image fill, Image handle = null)
    => new SliderContext(slider, fill, handle);

  public static SliderContext Create(GameObject uiElement, SliderConfiguration configuration)
  {
    using var _ = uiElement.DeactivatedScope();
    
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
      UIBuilderDSL.Create.UIElement("background-area")
        .ChildOf(uiElement)
        .WithMinMaxAnchor(minGutterAnchor, maxGutterAnchor)
        .At(0, 0)
        .uiElement;

    static Image CreateDefaultBackground()
    {
      var backgroundObj =
        UIBuilderDSL.Create.UIElement("background")
          .WithComponent(out Image backgroundImg, x => x.color = Color.black.AlphaMultiplied(0.8f));
      return backgroundImg;
    }

    var background = configuration.background ? configuration.background : CreateDefaultBackground();

    var backgroundObj =
      UIBuilderDSL.Select.UIElement(background.gameObject)
        .ChildOf(backgroundAreaObj)
        .WithAnchor(Anchor.Stretch).At(0, 0);
    
    var fillAreaObj =
      UIBuilderDSL.Create.UIElement("fill-area")
        .ChildOf(backgroundAreaObj)
        .WithAnchor(Anchor.Stretch).At(0, 0)
        .OfSize(-configuration.borderPadding, -configuration.borderPadding)
        .uiElement;
    
    static Image CreateDefaultFill()
    {
      var fillObj =
        UIBuilderDSL.Create.UIElement("fill")
          .WithComponent(out Image fillImg, x => x.color = Color.white.AlphaMultiplied(0.8f));

      return fillImg;
    }

    var fill = configuration.fill ? configuration.fill : CreateDefaultFill();

    var fillObj =
      UIBuilderDSL.Select.UIElement(fill.gameObject)
        .ChildOf(fillAreaObj)
        .WithAnchor(Anchor.Stretch).At(0, 0);

    slider.fillRect = fillObj.transform;

    if (!configuration.handle.HasValue)
      return Select(slider, fill);
    
    var handleConfiguration = configuration.handle.Value;
    var handleSize = handleConfiguration.handleLength;
    
    var handleAreaObj =
      UIBuilderDSL.Create.UIElement("handle-area")
        .ChildOf(uiElement)
        .WithAnchor(Anchor.Stretch).At(0, 0)
        .WithOffsetMinX(handleSize / 2f)
        .WithOffsetMaxX(-handleSize / 2f)
        .uiElement;
      
    static Image CreateDefaultHandle()
    {
      var handleObj =
        UIBuilderDSL.Create.UIElement("handle")
          .WithComponent(out Image handleImg, x => x.color = Color.black.AlphaMultiplied(0.45f));

      return handleImg;
    }

    var handle = handleConfiguration.handle ? handleConfiguration.handle : CreateDefaultHandle();
    
    var handleObj =
      UIBuilderDSL.Select.UIElement(handle.gameObject)
        .ChildOf(handleAreaObj)
        .WithAnchor(Anchor.StretchCenter).At(0, 0)
        .OfSize(handleSize, 0);

    slider.handleRect = handleObj.transform;

    return Select(slider, fill, handle);
  }

  public static T Bind<T>(this T Context, IDataBindSource<float> binding)
    where T: SliderContext
  {
    using var _ = Context.DeactivatedScope;
    
    var bindingController = Context.slider.gameObject.GetOrCreateComponent<DataBindSlider>();
    bindingController.Binding = binding;
    
    return Context;
  }
}
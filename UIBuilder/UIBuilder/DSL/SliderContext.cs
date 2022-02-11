using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;
public static partial class UIBuilderDSL
{
  public record SliderContext(GameObject uiElement) : UIElementContextBase<SliderContext>(uiElement)
  {
    public override SliderContext Context => this;

    public SliderContext WithSliderSupport(float minValue, float maxValue, bool wholeNumbers)
    {
      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);

      var fillAreaObj =
        Create.UIElement("fill-area")
          .ChildOf(uiElement)
          .WithMinMaxAnchor(new Vector2(0, 0.25f), new Vector2(1, 0.75f))
          // .WithAnchor(Anchor.Stretch)
          .At(0, 0)
          .uiElement;
      
      var backgroundObj =
        Create.UIElement("background")
          .ChildOf(fillAreaObj)
          .WithAnchor(Anchor.Stretch)
          .At(0, 0)
          .uiElement;

      var fill =
        Create.UIElement("fill")
          .ChildOf(fillAreaObj)
          .WithAnchor(Anchor.Stretch).At(0, 0)
          .uiElement;

      var handleSize = 30;
      
      var handleAreaObj =
        Create.UIElement("handle-area")
          .ChildOf(uiElement)
          .WithAnchor(Anchor.Stretch).At(0, 0)
          .WithOffsetMinX(handleSize / 2f)
          .WithOffsetMaxX(-handleSize / 2f)
          .uiElement;
      
      var handle =
        Create.UIElement("handle")
          .ChildOf(handleAreaObj)
          .WithAnchor(Anchor.StretchCenter).At(0, 0)
          .OfSize(handleSize, 0)
          .uiElement;

      var backgroundImg = backgroundObj.GetOrCreateComponent<Image>();
      backgroundImg.color = Color.black.AlphaMultiplied(0.45f);
      var fillImg = fill.GetOrCreateComponent<Image>();
      fillImg.color = Color.black.AlphaMultiplied(0.45f);

      var handleImg = handle.GetOrCreateComponent<Image>();
      handleImg.color = Color.black.AlphaMultiplied(0.45f);

      var slider = uiElement.GetOrCreateComponent<Slider>();

      slider.direction = Slider.Direction.LeftToRight;
      slider.minValue = minValue;
      slider.maxValue = maxValue;
      slider.wholeNumbers = wholeNumbers;

      slider.fillRect = fill.GetComponent<RectTransform>();

      slider.handleRect = handle.GetComponent<RectTransform>();


      if (wasActive)
        uiElement.SetActive(true);

      return Context;
    }

    public SliderContext Bind(IDataBindSource<float> binding)
    {
      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);
      
      var bindingController = uiElement.GetOrCreateComponent<DataBindSlider>();
      bindingController.Binding = binding;
      
      if (wasActive)
        uiElement.SetActive(true);
      return Context;
    }
  }
}
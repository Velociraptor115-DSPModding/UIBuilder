using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;
public static partial class UIBuilderDSL
{
  public record ToggleButtonContext(GameObject uiElement) : UIElementContextBase<ToggleButtonContext>(uiElement)
  {
    public override ToggleButtonContext Context => this;

    public ToggleButtonContext WithButtonSupport(string buttonText, ToggleGroup toggleGroup = null)
    {
      using var _ = DeactivatedScope;
      
      var textObj =
        Create.Text("text")
          .WithFontSize(20)
          .WithLocalizer(buttonText)
          .ChildOf(uiElement)
          .WithAnchor(Anchor.Stretch)
          .At(0, 0)
          .uiElement;

      WithComponent(out Image buttonImg, UIBuilder.buttonImgProperties);

      var toggle = uiElement.GetOrCreateComponent<Toggle>();
      (toggle as Selectable).CopyFrom(UIBuilder.buttonSelectableProperties);
      
      toggle.targetGraphic = buttonImg;

      if (toggleGroup != null)
        toggle.group = toggleGroup;

      return Context;
    }
    
    public ToggleButtonContext Bind(IDataBindSource<System.Enum> binding, System.Enum linkedValue)
    {
      using var _ = DeactivatedScope;
      
      var bindingController = uiElement.GetOrCreateComponent<DataBindToggleEnum>();
      bindingController.linkedValue = linkedValue;
      bindingController.Binding = binding;

      return Context;
    }
    
    public ToggleButtonContext WithOnOffVisuals(ColorBlock onState, ColorBlock offState)
    {
      using var _ = DeactivatedScope;

      var toggle = uiElement.GetOrCreateComponent<Toggle>();
      var valueChangedHandler = uiElement.GetOrCreateComponent<ToggleValueChangedHandler>();
      valueChangedHandler.toggle = toggle;
      valueChangedHandler.Handler =
        CreateToggleOnOffVisualsDelegate(toggle, onState, offState);
      
      return Context;
    }

    private static System.Action<bool> CreateToggleOnOffVisualsDelegate(Toggle component, ColorBlock onState, ColorBlock offState)
    {
      return isOn => component.colors = isOn ? onState : offState;
    }
  }
}
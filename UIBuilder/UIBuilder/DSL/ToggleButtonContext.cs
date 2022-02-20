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
          .WithOverflow(vertical: VerticalWrapMode.Truncate)
          .WithFontSize(20, 10, 20)
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
    
    public ToggleButtonContext BindInteractive(IOneWayDataBindSource<bool> binding)
    {
      var toggle = uiElement.GetOrCreateComponent<Toggle>();
      toggle.interactable = binding.Value;
      WithComponent<DataBindValueChangedHandlerBool>(x =>
      {
        x.Binding = binding;
        x.Handler = isOn => toggle.interactable = isOn;
      });
      
      return Context;
    }
    
    public ToggleButtonContext Bind(IDataBindSource<bool> binding)
    {
      using var _ = DeactivatedScope;
      
      var bindingController = uiElement.GetOrCreateComponent<DataBindToggleBool>();
      bindingController.Binding = binding;

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
    
    public ToggleButtonContext WithOnOffVisualsAndText(ColorBlock onState, ColorBlock offState, string onText, string offText)
    {
      using var _ = DeactivatedScope;

      var toggle = uiElement.GetOrCreateComponent<Toggle>();
      var valueChangedHandler = uiElement.GetOrCreateComponent<ToggleValueChangedHandler>();
      valueChangedHandler.toggle = toggle;
      valueChangedHandler.Handler =
        CreateToggleOnOffVisualsAndTextDelegate(toggle, onState, offState, text, onText, offText);
      
      return Context;
    }

    private static System.Action<bool> CreateToggleOnOffVisualsDelegate(Toggle component, ColorBlock onState, ColorBlock offState)
    {
      return isOn => component.colors = isOn ? onState : offState;
    }
    
    private static System.Action<bool> CreateToggleOnOffVisualsAndTextDelegate(
      Toggle toggleComponent, ColorBlock onState, ColorBlock offState,
      Text textComponent, string onText, string offText)
    {
      return isOn =>
      {
        toggleComponent.colors = isOn ? onState : offState;
        textComponent.text = isOn ? onText : offText;
      };
    }
  }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface IToggleSelectableContext : ISelectableContext
{
  Toggle toggle { get; }
}

public record ToggleContext(Toggle toggle, Image image) : UIElementContext(toggle.gameObject), IToggleSelectableContext, ISelectableVisuals
{
  public Graphic visuals { get; set; }
  Toggle IToggleSelectableContext.toggle => toggle;
  Selectable ISelectableContext.selectable => toggle;
}

public static class ToggleContextExtensions
{
  public static ToggleContext Select(Toggle toggle, Image image = null)
    => new(toggle, image) { visuals = image };

  public static ToggleContext Create(GameObject toggleObj)
    => Select(toggleObj.GetOrCreateComponent<Toggle>(), toggleObj.GetOrCreateComponent<Image>());

  public static T WithOnOffVisuals<T>(this T Context, ColorBlock onState, ColorBlock offState)
    where T: ToggleContext
  {
    using var _ = Context.DeactivatedScope;

    var toggle = Context.toggle;
    var valueChangedHandler = Context.uiElement.GetOrCreateComponent<ToggleValueChangedHandler>();
    valueChangedHandler.toggle = toggle;
    valueChangedHandler.Handler =
      CreateToggleOnOffVisualsDelegate(toggle, onState, offState);
    
    return Context;
  }
  
  public static T WithOnOffVisualsAndSprites<T>(this T Context, ColorBlock onState, ColorBlock offState, Sprite onSprite, Sprite offSprite)
    where T: ToggleContext
  {
    using var _ = Context.DeactivatedScope;

    var toggle = Context.toggle;
    var valueChangedHandler = Context.uiElement.GetOrCreateComponent<ToggleValueChangedHandler>();
    valueChangedHandler.toggle = toggle;
    valueChangedHandler.Handler =
      CreateToggleOnOffVisualsAndSpritesDelegate(toggle, onState, offState, Context.image, onSprite, offSprite);

    return Context;
  }
  
  private static System.Action<bool> CreateToggleOnOffVisualsDelegate(Toggle component, ColorBlock onState, ColorBlock offState)
  {
    return isOn => component.colors = isOn ? onState : offState;
  }
  
  private static System.Action<bool> CreateToggleOnOffVisualsAndSpritesDelegate(Toggle toggleComponent, ColorBlock onState, ColorBlock offState, Image imageComponent, Sprite onSprite, Sprite offSprite)
  {
    return isOn =>
    {
      toggleComponent.colors = isOn ? onState : offState;
      imageComponent.sprite = isOn ? onSprite : offSprite;
    };
  }
}

public static class IToggleContextExtensions
{
  public static T WithToggleGroup<T>(this T Context, ToggleGroup group)
    where T : IToggleSelectableContext
  {
    Context.toggle.group = group;
    return Context;
  }
  
  public static T Bind<T>(this T Context, IDataBindSource<bool> binding)
    where T: UIElementContext, IToggleSelectableContext
  {
    using var _ = Context.DeactivatedScope;
    
    var bindingController = Context.uiElement.GetOrCreateComponent<DataBindToggleBool>();
    bindingController.Binding = binding;

    return Context;
  }
  
  public static T Bind<T>(this T Context, IDataBindSource<System.Enum> binding, System.Enum linkedValue)
    where T: UIElementContext, IToggleSelectableContext
  {
    using var _ = Context.DeactivatedScope;
    
    var bindingController = Context.uiElement.GetOrCreateComponent<DataBindToggleEnum>();
    bindingController.linkedValue = linkedValue;
    bindingController.Binding = binding;

    return Context;
  }
}
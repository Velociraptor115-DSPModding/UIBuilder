using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public record ToggleButtonContext(Toggle toggle, Text text) : UIElementContext(toggle.gameObject), IToggleSelectableContext, ITextContext, ISelectableVisuals
{
  public Graphic visuals { get; set; }
  Text ITextContext.text => text;
  Graphic IGraphicContext.graphic => text;
  Toggle IToggleSelectableContext.toggle => toggle;
  Selectable ISelectableContext.selectable => toggle;
}

public static class ToggleButtonContextExtensions
{
  public static ToggleButtonContext Select(Toggle toggle, Text text = null, Graphic visuals = null)
    => new(toggle, text) { visuals = visuals };

  public static ToggleButtonContext Create(GameObject toggleObj, GameObject textObj)
  {
    var toggle = toggleObj.GetOrCreateComponent<Toggle>();
    var text = textObj.GetOrCreateComponent<Text>();
    return Select(toggle, text);
  }

  public static ToggleButtonContext Create(GameObject toggleObj)
    => Select(toggleObj.GetOrCreateComponent<Toggle>());

  public static ToggleButtonContext Create(GameObject toggleObj, string buttonText)
  {
    var toggle = toggleObj.GetOrCreateComponent<Toggle>();
    return Create(toggle, buttonText);
  }
  
  public static ToggleButtonContext Create(Toggle toggle, string buttonText)
  {
    var text =
      UIBuilderDSL.Create.Text("text")
        .WithOverflow(vertical: VerticalWrapMode.Truncate)
        .WithFont(UIBuilder.fontSAIRASB)
        .WithFontSize(16)
        .WithLocalizer(buttonText)
        .ChildOf(toggle.gameObject)
        .WithAnchor(Anchor.Stretch)
        .At(0, 0)
        ;

    return Select(toggle, text.text);
  }
  
  public static T WithOnOffVisuals<T>(this T Context, ColorBlock onState, ColorBlock offState)
    where T: ToggleButtonContext
  {
    using var _ = Context.DeactivatedScope;

    var toggle = Context.uiElement.GetOrCreateComponent<Toggle>();
    var valueChangedHandler = Context.uiElement.GetOrCreateComponent<ToggleValueChangedHandler>();
    valueChangedHandler.toggle = toggle;
    valueChangedHandler.Handler =
      CreateToggleOnOffVisualsDelegate(toggle, onState, offState);
    
    return Context;
  }
  
  public static T WithOnOffVisualsAndText<T>(this T Context, ColorBlock onState, ColorBlock offState, string onText, string offText)
    where T: ToggleButtonContext
  {
    using var _ = Context.DeactivatedScope;

    var toggle = Context.uiElement.GetOrCreateComponent<Toggle>();
    var valueChangedHandler = Context.uiElement.GetOrCreateComponent<ToggleValueChangedHandler>();
    valueChangedHandler.toggle = toggle;
    valueChangedHandler.Handler =
      CreateToggleOnOffVisualsAndTextDelegate(toggle, onState, offState, Context.text, onText, offText);
    
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
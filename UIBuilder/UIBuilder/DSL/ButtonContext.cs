using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;
public static partial class UIBuilderDSL
{
  public record ButtonContext(GameObject uiElement) : UIElementContextBase<ButtonContext>(uiElement)
  {
    public override ButtonContext Context => this;
    public TextContext text { get; private set; }

    public ButtonContext WithButtonSupport(string buttonText, UnityAction onClickCallback)
    {
      using var _ = DeactivatedScope;

      text =
        Create.Text("text")
          .WithOverflow(vertical: VerticalWrapMode.Truncate)
          .WithFontSize(20, 10, 20)
          .WithLocalizer(buttonText)
          .ChildOf(uiElement)
          .WithAnchor(Anchor.Stretch)
          .At(0, 0)
          ;

      WithComponent(out Image buttonImg, UIBuilder.buttonImgProperties);

      var button = uiElement.GetOrCreateComponent<Button>();
      (button as Selectable).CopyFrom(UIBuilder.buttonSelectableProperties);
      
      button.targetGraphic = buttonImg;
      
      button.onClick.AddListener(onClickCallback);

      return Context;
    }

    public ButtonContext BindInteractive(IOneWayDataBindSource<bool> binding)
    {
      var button = uiElement.GetOrCreateComponent<Button>();
      button.interactable = binding.Value;
      WithComponent<DataBindValueChangedHandlerBool>(x =>
      {
        x.Binding = binding;
        x.Handler = isOn => button.interactable = isOn;
      });
      return Context;
    }
  }
}
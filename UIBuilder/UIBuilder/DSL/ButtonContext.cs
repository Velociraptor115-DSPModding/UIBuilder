using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;
public static partial class UIBuilderDSL
{
  public record ButtonContext(GameObject uiElement) : UIElementContextBase<ButtonContext>(uiElement)
  {
    public override ButtonContext Context => this;

    public ButtonContext WithButtonSupport(string buttonText, UnityAction onClickCallback)
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

      var button = uiElement.GetOrCreateComponent<Button>();
      (button as Selectable).CopyFrom(UIBuilder.buttonSelectableProperties);
      
      button.targetGraphic = buttonImg;
      
      button.onClick.AddListener(onClickCallback);

      return Context;
    }
  }
}
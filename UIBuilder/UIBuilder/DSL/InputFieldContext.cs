using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;
public static partial class UIBuilderDSL
{
  public record InputFieldContext : UIElementContextBase<InputFieldContext>
  {
    public InputFieldContext(GameObject uiElement) : base(uiElement)
    {
      text =
        Create.Text("text")
          .ChildOf(uiElement)
          .WithAnchor(Anchor.Stretch)
          .text;

      var wasActive = uiElement.activeSelf;
      if (wasActive)
        uiElement.SetActive(false);

      inputField = uiElement.GetOrCreateComponent<InputField>();
      inputField.textComponent = text;

      var fieldGraphic = uiElement.CloneComponentFrom(UIBuilder.buttonImg);
      (inputField as Selectable).CopyFrom(UIBuilder.buttonSelectable);

      inputField.targetGraphic = fieldGraphic;
      
      if (wasActive)
        uiElement.SetActive(true);
    }

    public InputFieldContext WithContentType(InputField.ContentType contentType)
    {
      inputField.contentType = contentType;
      return Context;
    }
    
    public InputFieldContext WithFontSize(int fontSize)
    {
      text.fontSize = fontSize;
      return Context;
    }
    
    public InputFieldContext Bind(IDataBindSource<string> binding)
    {
      using var _ = Deactivated;
      
      var bindingController = uiElement.GetOrCreateComponent<DataBindInputField>();
      bindingController.Binding = binding;
      
      return Context;
    }

    public readonly Text text;
    public readonly InputField inputField;
    public override InputFieldContext Context => this;
  }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface IInputFieldSelectableContext : ISelectableContext
{
  InputField inputField { get; }
}

public record InputFieldContext(InputField inputField, Text text) : UIElementContext(inputField.gameObject), IInputFieldSelectableContext, ITextContext, ISelectableVisuals
{
  public Graphic visuals { get; set; }
  Selectable ISelectableContext.selectable => inputField;
  Graphic IGraphicContext.graphic => text;
}

public static class InputFieldContextExtensions
{
  public static InputFieldContext Select(InputField inputField, Text text= null, Graphic visuals = null)
    => new(inputField, text) { visuals = visuals };

  public static InputFieldContext Create(GameObject inputFieldObj)
  {
    using var _ = inputFieldObj.DeactivatedScope();
    
    var inputField = inputFieldObj.GetOrCreateComponent<InputField>();
    var text =
      UIBuilderDSL.Create.Text("text")
        .ChildOf(inputFieldObj)
        .WithAnchor(Anchor.Stretch)
        .text;
    
    inputField.textComponent = text;

    return Select(inputField, text);
  }
  public static T Bind<T>(this T Context, IDataBindSource<string> binding)
    where T: InputFieldContext
  {
    using var _ = Context.DeactivatedScope;
    
    var bindingController = Context.inputField.gameObject.GetOrCreateComponent<DataBindInputField>();
    bindingController.Binding = binding;
    
    return Context;
  }
}

public static class IInputFieldContextExtensions
{
  public static T WithContentType<T>(this T Context, InputField.ContentType contentType)
  where T: IInputFieldSelectableContext
  {
    Context.inputField.contentType = contentType;
    return Context;
  }
}
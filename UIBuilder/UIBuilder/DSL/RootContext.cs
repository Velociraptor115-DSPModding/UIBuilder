using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;
public static partial class UIBuilderDSL
{
  public static readonly SelectContext Select;
  public static readonly CreateContext Create;

  public struct SelectContext
  {
    public readonly UIElementContext UIElement(RectTransform transform)
      => UIElementExtensions.Select(transform.gameObject);
    public readonly UIElementContext UIElement(GameObject obj)
      => UIElementExtensions.Select(obj);
    public readonly PlainWindowContext PlainWindow(GameObject obj) => new PlainWindowContext(obj);
    public readonly FancyWindowContext FancyWindow(GameObject obj) => new FancyWindowContext(obj);
    public readonly ScrollViewContext ScrollView(ScrollRect scrollRect)
      => ScrollViewContextExtensions.Select(scrollRect);
    public readonly ButtonContext Button(Button button, Text text = null, Graphic visuals = null)
      => ButtonContextExtensions.Select(button, text, visuals);
    public readonly ToggleButtonContext ToggleButton(Toggle toggle, Text text = null, Graphic visuals = null)
      => ToggleButtonContextExtensions.Select(toggle, text, visuals);
    public readonly TextContext Text(Text text)
      => TextContextExtensions.Select(text);
    public readonly SliderContext Slider(Slider slider, Image fill, Image handle = null)
      => SliderContextExtensions.Select(slider, fill, handle);
    public readonly InputFieldContext InputField(InputField inputField, Text text = null)
      => InputFieldContextExtensions.Select(inputField, text);
    public readonly ComboBoxContext ComboBox(GameObject obj) => new ComboBoxContext(obj);
    public readonly HorizontalLayoutGroupContext HorizontalLayoutGroup(GameObject obj) => new HorizontalLayoutGroupContext(obj);
    public readonly VerticalLayoutGroupContext VerticalLayoutGroup(GameObject obj) => new VerticalLayoutGroupContext(obj);
    public readonly GridLayoutGroupContext GridLayoutGroup(GameObject obj) => new GridLayoutGroupContext(obj);
  }

  public struct CreateContext
  {
    public readonly UIElementContext UIElement(string name)
      => UIElementExtensions.Create(name);

    public readonly PlainWindowContext PlainWindow(string name)
    {
      return Select.PlainWindow(UIElement(name).uiElement)
        .WithBorder()
        .WithShadow()
        .WithDragProperties()
        .WithResizeProperties()
        ;
    }

    public readonly FancyWindowContext FancyWindow(string name)
    {
      return Select.FancyWindow(UIElement(name).uiElement)
        .WithBorder()
        .WithShadow()
        .WithDragProperties()
        .WithResizeProperties()
        ;
    }

    public readonly ScrollViewContext ScrollView(string name, ScrollViewConfiguration configuration)
      => ScrollViewContextExtensions.Create(UIElement(name).uiElement, configuration);

    public readonly ButtonContext Button(string name, string buttonText, UnityAction onClickCallback)
      => ButtonContextExtensions.Create(UIElement(name).uiElement, buttonText)
          .AddClickListener(onClickCallback);

    public readonly ToggleButtonContext ToggleButton(string name, string buttonText)
      => ToggleButtonContextExtensions.Create(UIElement(name).uiElement, buttonText);

    public readonly TextContext Text(string name)
      => TextContextExtensions.Create(UIElement(name).uiElement);

    public readonly SliderContext Slider(string name, SliderConfiguration configuration)
      => SliderContextExtensions.Create(UIElement(name).uiElement, configuration);

    public readonly InputFieldContext InputField(string name)
      => InputFieldContextExtensions.Create(UIElement(name).uiElement);
    
    public readonly ComboBoxContext ComboBox(string name)
    {
      var obj = Object.Instantiate(UIBuilder.comboBoxComponent, null, false);
      return Select.ComboBox(obj.gameObject);
    }
    
    public readonly HorizontalLayoutGroupContext HorizontalLayoutGroup(string name)
      => Select.HorizontalLayoutGroup(UIElement(name).uiElement);
    public readonly VerticalLayoutGroupContext VerticalLayoutGroup(string name)
      => Select.VerticalLayoutGroup(UIElement(name).uiElement);
    public readonly GridLayoutGroupContext GridLayoutGroup(string name)
      => Select.GridLayoutGroup(UIElement(name).uiElement);
  }
}
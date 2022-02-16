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
    public readonly UIElementContext UIElement(GameObject obj) => new UIElementContext(obj);
    public readonly PlainWindowContext PlainWindow(GameObject obj) => new PlainWindowContext(obj);
    public readonly FancyWindowContext FancyWindow(GameObject obj) => new FancyWindowContext(obj);
    public readonly ScrollViewContext ScrollView(GameObject obj) => new ScrollViewContext(obj);
    public readonly ButtonContext Button(GameObject obj) => new ButtonContext(obj);
    public readonly TextContext Text(GameObject obj) => new TextContext(obj);
    public readonly SliderContext Slider(GameObject obj) => new SliderContext(obj);
    public readonly InputFieldContext InputField(GameObject obj) => new InputFieldContext(obj);
    public readonly ComboBoxContext ComboBox(GameObject obj) => new ComboBoxContext(obj);
    public readonly HorizontalLayoutGroupContext HorizontalLayoutGroup(GameObject obj) => new HorizontalLayoutGroupContext(obj);
    public readonly VerticalLayoutGroupContext VerticalLayoutGroup(GameObject obj) => new VerticalLayoutGroupContext(obj);
    public readonly GridLayoutGroupContext GridLayoutGroup(GameObject obj) => new GridLayoutGroupContext(obj);
  }

  public struct CreateContext
  {
    public readonly UIElementContext UIElement(string name)
    {
      var obj = new GameObject(name, typeof(RectTransform));
      obj.SetLayer((int)Layer.UI);
      return Select.UIElement(obj).OfSize(0, 0).WithPivot(0, 1).At(0, 0).Context; 
    }

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

    public readonly ScrollViewContext ScrollView(string name, bool onlyVerticalScroll = true, uint scrollBarWidth = 5)
    {
      return Select.ScrollView(UIElement(name).uiElement)
        .WithScrollSupport(onlyVerticalScroll, scrollBarWidth);
    }

    public readonly ButtonContext Button(string name, string buttonText, UnityAction onClickCallback)
    {
      return Select.Button(UIElement(name).uiElement)
        .WithButtonSupport(buttonText, onClickCallback);
    }
    
    public readonly TextContext Text(string name)
    {
      return Select.Text(UIElement(name).uiElement);
    }
    
    public readonly SliderContext Slider(string name, SliderConfiguration configuration)
    {
      return Select.Slider(UIElement(name).uiElement)
        .WithSliderConfig(configuration);
    }
    
    public readonly InputFieldContext InputField(string name)
    {
      return Select.InputField(UIElement(name).uiElement);
    }
    
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
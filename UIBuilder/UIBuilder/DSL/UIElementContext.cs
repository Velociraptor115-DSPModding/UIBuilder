using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;

public interface IInitializeFromContext<T>
{
  void InitializeFromContext(T context);
}

public record UIElementContext(GameObject uiElement)
{
  public readonly RectTransform transform = uiElement.transform as RectTransform;

  public TemporaryDeactivationContext DeactivatedScope => uiElement.DeactivatedScope();
}

public readonly ref struct TemporaryDeactivationContext
{
  private readonly bool wasActive;
  private readonly GameObject obj;

  public TemporaryDeactivationContext(GameObject obj)
  {
    this.wasActive = obj.activeSelf;
    this.obj = obj;
    obj.SetActive(false);
  }

  public void Dispose()
  {
    if (wasActive)
      obj.SetActive(true);
  }
}

public static class TemporaryDeactivationContextExtensions
{
  public static TemporaryDeactivationContext DeactivatedScope(this GameObject uiElement) => new(uiElement);
}

public static class UIElementExtensions
{
  public static UIElementContext Create(string name)
  {
    var obj = new GameObject(name, typeof(RectTransform));
    obj.SetLayer((int)Layer.UI);
    return Select(obj).OfSize(0, 0).WithPivot(0, 1).At(0, 0);
  }

  public static UIElementContext Select(GameObject obj)
    => new UIElementContext(obj);
  
  public static T Deactivate<T>(this T Context)
    where T: UIElementContext
  {
    Context.uiElement.SetActive(false);
    return Context;
  }

  public static T Activate<T>(this T Context)
    where T: UIElementContext
  {
    Context.uiElement.SetActive(true);
    return Context;
  }

  public static T OfSize<T>(this T Context, float width, float height)
    where T: UIElementContext
    => Context.OfSize(new Vector2(width, height));
  public static T OfSize<T>(this T Context, int width, int height)
    where T: UIElementContext
    => Context.OfSize(new Vector2(width, height));
  public static T OfSize<T>(this T Context, Vector2 size)
    where T: UIElementContext
  {
    Context.transform.sizeDelta = size;
    return Context;
  }

  public static T WithLayoutSize<T>(
    this T Context
    , float minWidth, float minHeight
    , float preferredWidth = -1f, float preferredHeight = -1f
    , float flexibleWidth = -1f, float flexibleHeight = -1f
    , int layoutPriority = 1)
    where T: UIElementContext
  {
    var layoutElement = Context.uiElement.GetOrCreateComponent<LayoutElement>();
    layoutElement.minWidth = minWidth;
    layoutElement.minHeight = minHeight;
    layoutElement.preferredWidth = preferredWidth;
    layoutElement.preferredHeight = preferredHeight;
    layoutElement.flexibleWidth = flexibleWidth;
    layoutElement.flexibleHeight = flexibleHeight;
    layoutElement.layoutPriority = layoutPriority;
    return Context;
  }

  public static T WithMinMaxAnchor<T>(this T Context, Vector2 anchorMin, Vector2 anchorMax)
    where T: UIElementContext
  {
    Context.transform.anchorMin = anchorMin;
    Context.transform.anchorMax = anchorMax;
    return Context;
  }

  public static T WithPivot<T>(this T Context, float x, float y)
    where T: UIElementContext
    => Context.WithPivot(new Vector2(x, y));
  public static T WithPivot<T>(this T Context, Vector2 pivot)
    where T: UIElementContext
  {
    Context.transform.pivot = pivot;
    return Context;
  }

  public static T WithAnchor<T>(this T Context, Anchor anchor)
    where T: UIElementContext
  {
    var (anchorMin, anchorMax) = anchor.ToMinMaxAnchors();
    var newSize = Context.transform.sizeDelta;
    newSize.x = anchor.horizontal == AnchorHorizontal.Stretch ? 0 : newSize.x;
    newSize.y = anchor.vertical == AnchorVertical.Stretch ? 0 : newSize.y;

    var pivot = (anchorMin + anchorMax) / 2f;

    return Context.WithMinMaxAnchor(anchorMin, anchorMax).WithPivot(pivot).OfSize(newSize);
  }
  
  public static T WithOffsetMinX<T>(this T Context, float value)
    where T: UIElementContext
    => Context.WithOffsetMin(new Vector2(value, Context.transform.offsetMin.y));
  public static T WithOffsetMinY<T>(this T Context, float value)
    where T: UIElementContext
    => Context.WithOffsetMin(new Vector2(Context.transform.offsetMin.x, value));
  public static T WithOffsetMin<T>(this T Context, float x, float y)
    where T: UIElementContext
    => Context.WithOffsetMin(new Vector2(x, y));
  public static T WithOffsetMin<T>(this T Context, Vector2 offsetMin)
    where T: UIElementContext
  {
    Context.transform.offsetMin = offsetMin;
    return Context;
  }
  
  public static T WithOffsetMaxX<T>(this T Context, float value)
   where T: UIElementContext
   => Context.WithOffsetMax(new Vector2(value, Context.transform.offsetMax.y));
  public static T WithOffsetMaxY<T>(this T Context, float value)
   where T: UIElementContext
   => Context.WithOffsetMax(new Vector2(Context.transform.offsetMax.x, value));
  public static T WithOffsetMax<T>(this T Context, float x, float y)
   where T: UIElementContext
   => Context.WithOffsetMax(new Vector2(x, y));
  public static T WithOffsetMax<T>(this T Context, Vector2 offsetMax)
    where T: UIElementContext

  {
    Context.transform.offsetMax = offsetMax;
    return Context;
  }

  public static T At<T>(this T Context, int x, int y)
    where T: UIElementContext
    => Context.At(new Vector2(x, y));
  public static T At<T>(this T Context, Vector2 position)
    where T: UIElementContext
  {
    Context.transform.anchoredPosition = position;
    return Context;
  }

  public static T ChildOf<T>(this T Context, UIElementContext parent)
    where T: UIElementContext
    => Context.ChildOf(parent.transform);
  public static T ChildOf<T>(this T Context, GameObject parent)
    where T: UIElementContext
    => Context.ChildOf(parent.transform);
  public static T ChildOf<T>(this T Context, Transform parent)
    where T: UIElementContext
  {
    Context.transform.SetParent(parent, false);
    Context.transform.localPosition = Vector3.zero;
    Context.transform.localScale = Vector3.one;
    return Context;
  }

  public static T AddChildren<T>(this T Context, params UIElementContext[] children)
    where T: UIElementContext
    => Context.AddChildren(children.Select(x => x.transform));
  public static T AddChildren<T>(this T Context, IEnumerable<RectTransform> children)
    where T: UIElementContext
  {
    foreach (var child in children)
    {
      child.SetParent(Context.transform, false);
      child.localPosition = Vector3.zero;
      child.localScale = Vector3.one;
    }
    return Context;
  }
  
  public static T WithComponent<T, U>(this T Context, out U component)
    where T: UIElementContext
    where U : Component
    => Context.WithComponent(out component, (System.Action<U>) null);
  public static T WithComponent<T, U>(this T Context, System.Action<U> initializer)
    where T: UIElementContext
    where U : Component
    => Context.WithComponent(out _, initializer);
  public static T WithComponent<T, U>(this T Context, out U component, System.Action<U> initializer)
    where T: UIElementContext
    where U : Component
  {
    component = Context.uiElement.GetOrCreateComponent<U>();
    initializer?.Invoke(component);
    return Context;
  }
  public static T WithComponent<T, U>(this T Context, out U component, params IProperties<U>[] properties)
    where T: UIElementContext
    where U : Component
  {
    component = Context.uiElement.GetOrCreateComponentWithProperties(properties);
    return Context;
  }

  public static T CloneComponentFrom<T, U>(this T Context, U componentToClone)
    where T: UIElementContext
    where U : Component
  {
    Context.uiElement.CloneComponentFrom(componentToClone);
    return Context;
  }

  public static T InitializeComponent<T, U>(this T Context, out U component)
    where T: UIElementContext
    where U : Component, IInitializeFromContext<T>
  {
    component = Context.uiElement.GetOrCreateComponent<U>();
    component.InitializeFromContext(Context);
    return Context;
  }
}
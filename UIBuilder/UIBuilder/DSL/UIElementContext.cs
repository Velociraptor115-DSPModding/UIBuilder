using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;

public interface IInitializeFromContext<T>
{
  void InitializeFromContext(T context);
}

public static partial class UIBuilderDSL
{
  public record UIElementContext(GameObject uiElement) : UIElementContextBase<UIElementContext>(uiElement)
  {
    public override UIElementContext Context => this;
  }

  public abstract record UIElementContextBase<T>(GameObject uiElement)
    where T: UIElementContextBase<T>
  {
    public readonly RectTransform transform = uiElement.transform as RectTransform;

    public T Deactivate()
    {
      uiElement.SetActive(false);
      return Context;
    }

    public T Activate()
    {
      uiElement.SetActive(true);
      return Context;
    }

    public T OfSize(int width, int height) => OfSize(new Vector2(width, height));
    public T OfSize(Vector2 size)
    {
      transform.sizeDelta = size;
      return Context;
    }

    public T WithLayoutSize(
        float minWidth, float minHeight
      , float preferredWidth = -1f, float preferredHeight = -1f
      , float flexibleWidth = -1f, float flexibleHeight = -1f
      , int layoutPriority = 1)
    {
      var layoutElement = uiElement.GetOrCreateComponent<LayoutElement>();
      layoutElement.minWidth = minWidth;
      layoutElement.minHeight = minHeight;
      layoutElement.preferredWidth = preferredWidth;
      layoutElement.preferredHeight = preferredHeight;
      layoutElement.flexibleWidth = flexibleWidth;
      layoutElement.flexibleHeight = flexibleHeight;
      layoutElement.layoutPriority = layoutPriority;
      return Context;
    }

    public T WithMinMaxAnchor(Vector2 anchorMin, Vector2 anchorMax)
    {
      transform.anchorMin = anchorMin;
      transform.anchorMax = anchorMax;
      return Context;
    }

    public T WithPivot(float x, float y) => WithPivot(new Vector2(x, y));
    public T WithPivot(Vector2 pivot)
    {
      transform.pivot = pivot;
      return Context;
    }

    public T WithAnchor(Anchor anchor)
    {
      var (anchorMin, anchorMax) = anchor.ToMinMaxAnchors();
      var newSize = transform.sizeDelta;
      newSize.x = anchor.horizontal == AnchorHorizontal.Stretch ? 0 : newSize.x;
      newSize.y = anchor.vertical == AnchorVertical.Stretch ? 0 : newSize.y;

      var pivot = (anchorMin + anchorMax) / 2f;

      return WithMinMaxAnchor(anchorMin, anchorMax).WithPivot(pivot).OfSize(newSize);
    }
    
    public T WithOffsetMinX(float value) => WithOffsetMin(new Vector2(value, transform.offsetMin.y));
    public T WithOffsetMinY(float value) => WithOffsetMin(new Vector2(transform.offsetMin.x, value));
    public T WithOffsetMin(float x, float y) => WithOffsetMin(new Vector2(x, y));
    public T WithOffsetMin(Vector2 offsetMin)
    {
      transform.offsetMin = offsetMin;
      return Context;
    }
    
    public T WithOffsetMaxX(float value) => WithOffsetMax(new Vector2(value, transform.offsetMax.y));
    public T WithOffsetMaxY(float value) => WithOffsetMax(new Vector2(transform.offsetMax.x, value));
    public T WithOffsetMax(float x, float y) => WithOffsetMax(new Vector2(x, y));
    public T WithOffsetMax(Vector2 offsetMax)
    {
      transform.offsetMax = offsetMax;
      return Context;
    }

    public T At(int x, int y) => At(new Vector2(x, y));
    public T At(Vector2 position)
    {
      transform.anchoredPosition = position;
      return Context;
    }

    public T ChildOf<U>(UIElementContextBase<U> parent)
      where U: UIElementContextBase<U>
      => ChildOf(parent.transform);
    public T ChildOf(GameObject parent) => ChildOf(parent.transform);
    public T ChildOf(Transform parent)
    {
      transform.SetParent(parent, false);
      transform.localPosition = Vector3.zero;
      transform.localScale = Vector3.one;
      return Context;
    }

    public T WithChildren(params UIElementContextBase<T>[] children) => WithChildren(children.AsEnumerable());
    public T WithChildren(IEnumerable<UIElementContextBase<T>> children)
    {
      foreach (var child in children)
        child.ChildOf(transform);
      return Context;
    }

    public T CloneComponentFrom<U>(U componentToClone) where U : Component
    {
      uiElement.CloneComponentFrom(componentToClone);
      return Context;
    }

    public T InitializeComponent<U>(out U component)
      where U : Component, IInitializeFromContext<T>
    {
      component = uiElement.GetOrCreateComponent<U>();
      component.InitializeFromContext(Context);
      return Context;
    }
    public TemporaryDeactivationContext DeactivatedScope => new(uiElement);

    public abstract T Context { get; }
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
}
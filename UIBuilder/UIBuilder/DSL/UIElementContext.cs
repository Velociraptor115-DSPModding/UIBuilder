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
  {
    public readonly RectTransform transform = uiElement.transform as RectTransform;

    public UIElementContextBase<T> Deactivate()
    {
      uiElement.SetActive(false);
      return this;
    }

    public UIElementContextBase<T> Activate()
    {
      uiElement.SetActive(true);
      return this;
    }

    public UIElementContextBase<T> OfSize(int width, int height) => OfSize(new Vector2(width, height));
    public UIElementContextBase<T> OfSize(Vector2 size)
    {
      transform.sizeDelta = size;
      return this;
    }

    public UIElementContextBase<T> WithMinMaxAnchor(Vector2 anchorMin, Vector2 anchorMax)
    {
      transform.anchorMin = anchorMin;
      transform.anchorMax = anchorMax;
      return this;
    }

    public UIElementContextBase<T> WithPivot(float x, float y) => WithPivot(new Vector2(x, y));
    public UIElementContextBase<T> WithPivot(Vector2 pivot)
    {
      transform.pivot = pivot;
      return this;
    }

    public UIElementContextBase<T> WithAnchor(Anchor anchor)
    {
      var (anchorMin, anchorMax) = anchor.ToMinMaxAnchors();
      var newSize = transform.sizeDelta;
      newSize.x = anchor.horizontal == AnchorHorizontal.Stretch ? 0 : newSize.x;
      newSize.y = anchor.vertical == AnchorVertical.Stretch ? 0 : newSize.y;

      var pivot = (anchorMin + anchorMax) / 2f;

      return WithMinMaxAnchor(anchorMin, anchorMax).WithPivot(pivot).OfSize(newSize);
    }

    public UIElementContextBase<T> At(int x, int y) => At(new Vector2(x, y));
    public UIElementContextBase<T> At(Vector2 position)
    {
      transform.anchoredPosition = position;
      return this;
    }

    public UIElementContextBase<T> ChildOf(UIElementContextBase<T> parent) => ChildOf(parent.transform);
    public UIElementContextBase<T> ChildOf(GameObject parent) => ChildOf(parent.transform);
    public UIElementContextBase<T> ChildOf(Transform parent)
    {
      transform.SetParent(parent);
      transform.localPosition = Vector3.zero;
      transform.localScale = Vector3.one;
      return this;
    }

    public UIElementContextBase<T> WithChildren(params UIElementContextBase<T>[] children) => WithChildren(children.AsEnumerable());
    public UIElementContextBase<T> WithChildren(IEnumerable<UIElementContextBase<T>> children)
    {
      foreach (var child in children)
        child.ChildOf(transform);
      return this;
    }

    public UIElementContextBase<T> CloneComponentFrom<U>(U componentToClone) where U : Component
    {
      uiElement.CloneComponentFrom(componentToClone);
      return this;
    }

    public UIElementContextBase<T> InitializeComponent<U>(out U component)
      where U : Component, IInitializeFromContext<T>
    {
      component = uiElement.GetOrCreateComponent<U>();
      component.InitializeFromContext(Context);
      return this;
    }

    public abstract T Context { get; }
  }
}
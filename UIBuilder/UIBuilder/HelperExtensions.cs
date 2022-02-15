using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DysonSphereProgram.Modding.UI
{
  public static class GameObjectExtensions
  {
    public static GameObject DestroyChildren(this GameObject gameObject, params string[] names)
    {
      var x = gameObject;
      foreach (var name in names)
        x = x.DestroyChild(name);
      return x;
    }
    public static GameObject DestroyChild(this GameObject gameObject, string name)
    {
      if (gameObject == null)
        return null;
      var child = gameObject.transform.Find(name)?.gameObject;
      if (child != null)
        Object.Destroy(child);
      return gameObject;
    }

    public static GameObject DestroyComponent<T>(this GameObject gameObject) where T : Component
    {
      if (gameObject == null)
        return null;
      var component = gameObject.GetComponent<T>();
      if (component != null)
        Object.Destroy(component);
      return gameObject;
    }

    public static T GetOrCreateComponent<T>(this GameObject gameObject) where T : Component
    {
      if (gameObject == null)
        return null;
      var component = gameObject.GetComponent<T>();
      if (component == null)
        component = gameObject.AddComponent<T>();
      return component;
    }
    
    public static T GetOrCreateComponentWithProperties<T>(this GameObject gameObject, params IProperties<T>[] componentProperties)
      where T : Component
    {
      if (gameObject == null)
        return null;
      var component = gameObject.GetOrCreateComponent<T>();
      foreach (var x in componentProperties)
        component.CopyFrom(x);
      return component;
    }

    public static T CloneComponentFrom<T>(this GameObject gameObject, T componentToClone) where T : Component
    {
      if (gameObject == null)
        return null;
      var component = gameObject.GetOrCreateComponent<T>();
      component.CopyFrom(componentToClone);
      return component;
    }

    public static GameObject SelectChild(this GameObject gameObject, string name)
    {
      if (gameObject == null)
        return null;
      return gameObject.transform.Find(name)?.gameObject;
    }

    public static GameObject SelectDescendant(this GameObject gameObject, params string[] names)
    {
      var x = gameObject;
      foreach (var name in names)
        x = x.SelectChild(name);
      return x;
    }
  }

  public static class MonoBehaviourExtensions
  {
    public static void InvokeNextFrame(this MonoBehaviour instance, System.Action executable)
    {
      try
      {
        instance.StartCoroutine(_InvokeNextFrame(executable));
      }
      catch
      {
        Plugin.Log.LogError("Couldn't invoke in next frame");
      }
    }

    private static IEnumerator _InvokeNextFrame(System.Action executable)
    {
      yield return null;
      executable();
    }
  }

  public interface ISupportCopyFrom<T> where T : Component
  {
    void CopyFrom(T source);
  }

  public static class CopyFromExtensions
  {
    public static void CopyFrom<T>(this T destination, T source) where T: Component
    {
      switch (destination, source)
      {
        case (TranslucentImage d, TranslucentImage s):
          d.CopyFrom(s);
          break;
        case (Image d, Image s):
          d.CopyFrom(s);
          break;
        case (MaskableGraphic d, MaskableGraphic s):
          d.CopyFrom(s);
          break;
        case (Graphic d, Graphic s):
          d.CopyFrom(s);
          break;
        case (UIButton d, UIButton s):
          d.CopyFrom(s);
          break;
        case (ScrollRect d, ScrollRect s):
          d.CopyFrom(s);
          break;
        case (Scrollbar d, Scrollbar s):
          d.CopyFrom(s);
          break;
        case (ISupportCopyFrom<T> d, T s):
          d.CopyFrom(s);
          break;
      }
    }
    
    public static void CopyFrom<T>(this T destination, IProperties<T> source) where T: Component
    {
      source.Apply(destination);
    }

    public static void CopyFrom(this TranslucentImage destination, TranslucentImage source)
    {
      (destination as Image).CopyFrom(source as Image);
      destination.vibrancy = source.vibrancy;
      destination.brightness = source.brightness;
      destination.flatten = source.flatten;
      destination.updating = source.updating;
      destination.lateUpdating = source.lateUpdating;
      destination.spriteBlending = source.spriteBlending;
    }

    public static void CopyFrom(this Image destination, Image source)
    {
      (destination as MaskableGraphic).CopyFrom(source as MaskableGraphic);
      destination.sprite = source.sprite;
      destination.overrideSprite = source.overrideSprite;
      destination.type = source.type;
      destination.preserveAspect = source.preserveAspect;
      
      destination.fillMethod = source.fillMethod;
      destination.fillCenter = source.fillCenter;
      destination.fillAmount = source.fillAmount;
      destination.fillClockwise = source.fillClockwise;
      destination.fillOrigin = source.fillOrigin;

      destination.alphaHitTestMinimumThreshold = source.alphaHitTestMinimumThreshold;
      destination.useSpriteMesh = source.useSpriteMesh;
    }

    public static void CopyFrom(this MaskableGraphic destination, MaskableGraphic source)
    {
      (destination as Graphic).CopyFrom(source as Graphic);
      destination.maskable = source.maskable;
    }

    public static void CopyFrom(this Graphic destination, Graphic source)
    {
      destination.color = source.color;
      destination.raycastTarget = source.raycastTarget;
      destination.material = source.material;
    }

    public static void CopyFrom(this UIButton destination, UIButton source)
    {
      destination.data = source.data;
      destination.audios = source.audios;
      destination.tips = source.tips;
      if (source.transitions != null)
      {
        var count = source.transitions.Length;
        destination.transitions = new UIButton.Transition[count];
        for (int i = 0; i < count; i++)
        {
          destination.transitions[i] = new UIButton.Transition();
          destination.transitions[i].CopyFrom(source.transitions[i]);
        }
      }

      destination.tipTitleFormatString = source.tipTitleFormatString;
      destination.tipTextFormatString = source.tipTextFormatString;
    }

    public static void CopyFrom(this UIButton.Transition destination, UIButton.Transition source)
    {
      destination.damp = source.damp;
      destination.mouseoverSize = source.mouseoverSize;
      destination.pressedSize = source.pressedSize;
      destination.normalColor = source.normalColor;
      destination.mouseoverColor = source.mouseoverColor;
      destination.pressedColor = source.pressedColor;
      destination.disabledColor = source.disabledColor;
      destination.alphaOnly = source.alphaOnly;
      destination.highlightSizeMultiplier = source.highlightSizeMultiplier;
      destination.highlightColorMultiplier = source.highlightColorMultiplier;
      destination.highlightAlphaMultiplier = source.highlightAlphaMultiplier;
      destination.highlightColorOverride = source.highlightColorOverride;
    }

    public static void CopyFrom(this ScrollRect destination, ScrollRect source)
    {
      destination.movementType = source.movementType;
      destination.elasticity = source.elasticity;
      destination.inertia = source.inertia;
      destination.velocity = source.velocity;
      destination.decelerationRate = source.decelerationRate;
      destination.scrollSensitivity = source.scrollSensitivity;
      destination.horizontalScrollbarSpacing = source.horizontalScrollbarSpacing;
      destination.verticalScrollbarSpacing = source.verticalScrollbarSpacing;
    }
    
    public static void CopyFrom(this Scrollbar destination, Scrollbar source)
    {
      (destination as Selectable).CopyFrom(source as Selectable);
      destination.direction = source.direction;
      // destination.value = source.value;
      // destination.size = source.size;
      destination.numberOfSteps = source.numberOfSteps;
    }
    
    public static void CopyFrom(this Selectable destination, Selectable source)
    {
      // destination.navigation = source.navigation;
      destination.transition = source.transition;
      destination.colors = source.colors;
      destination.interactable = source.interactable;
      // destination.spriteState = source.spriteState;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using DysonSphereProgram.Modding.UI.Builder;

namespace DysonSphereProgram.Modding.Raptor.TestUIBuilder
{
  public record EntityPathMap<T>(T entity, string[] paths);
  public static class UIAnalysis
  {
    public static string GetFullPath(this GameObject current) => current.transform.GetFullPath();
    public static string GetFullPath(this Transform current)
    {
      if (current.parent == null)
        return "/" + current.name;
      return current.parent.GetFullPath() + "/" + current.name;
    }

    public static void Execute()
    {
      // ListUIButtonsWithInput();
      // ListByUsage<Image, Sprite>(false, x => x.sprite, x => x.overrideSprite);
      // ListByUsage<Graphic, Material>(false, x => x.material);
      // ListByUsage<Graphic, Texture>(false, x => x.mainTexture);
      // ListByUsage<Text, Font>(false, x => x.font);
      // ListByUsage<Graphic, Shader>(false, x => x.material.shader);
      // ListUsageByField<Graphic, bool>(true, x => x.raycastTarget);
      // ListByUsage<RawImage, Texture>(true, x => x.texture);
      // Plugin.Log.LogWarning("updating");
      // ListUsageByField<TranslucentImage, bool>(true, x => x.updating);
      // Plugin.Log.LogWarning("lateUpdating");
      // ListUsageByField<TranslucentImage, bool>(true, x => x.lateUpdating);
    }
    
    private static void ListByUsage<TComponent, TEntity>(bool all, params Func<TComponent, TEntity>[] getters)
      where TComponent: Component
      where TEntity: Object
    {
      var obj = UIRoot.instance.uiGame.gameObject;
      var components = obj.GetComponentsInChildren<TComponent>(true);

      var entitiesDict = new Dictionary<TEntity, HashSet<string>>();

      void AddEntity(TEntity entities, string path)
      {
        if (!entities)
          return;
        if (!entitiesDict.ContainsKey(entities))
          entitiesDict[entities] = new HashSet<string>();
        entitiesDict[entities].Add(path);
      }

      foreach (var component in components)
      {
        var path = component.gameObject.GetFullPath();
        foreach (var getter in getters)
        {
          AddEntity(getter(component), path); 
        }
      }

      var entities = new List<EntityPathMap<TEntity>>();

      foreach (var entityKvp in entitiesDict)
      {
        var stringArr = entityKvp.Value.OrderBy(x => x.Length).ToArray();
        // Array.Sort(stringArr);
        entities.Add(new EntityPathMap<TEntity>(entityKvp.Key, stringArr));
      }

      var entitiesSorted = entities.OrderByDescending(x => x.paths.Length);

      foreach (var entity in entitiesSorted)
      {
        Plugin.Log.LogDebug($"{entity.entity.name}: {entity.paths.Length}");
        if (all)
        {
          foreach (var path in entity.paths)
          {
            Plugin.Log.LogDebug("- " + path);
          }
        }
        else
        {
          Plugin.Log.LogDebug("- " + entity.paths[0]); 
        }
      }
    }
    
    private static void ListUsageByField<TComponent, TField>(bool all, params Func<TComponent, TField>[] getters)
      where TComponent: Component
    {
      var obj = UIRoot.instance.uiGame.gameObject;
      var components = obj.GetComponentsInChildren<TComponent>(true);

      var entitiesDict = new Dictionary<TField, HashSet<string>>();

      foreach (var component in components)
      {
        var path = component.gameObject.GetFullPath();
        foreach (var getter in getters)
        {
          var fieldVal = getter(component);
          if (!entitiesDict.ContainsKey(fieldVal))
            entitiesDict[fieldVal] = new HashSet<string>();
          entitiesDict[fieldVal].Add(path);
        }
      }

      var entities = new List<EntityPathMap<TField>>();

      foreach (var entityKvp in entitiesDict)
      {
        var stringArr = entityKvp.Value.OrderBy(x => x.Length).ToArray();
        // Array.Sort(stringArr);
        entities.Add(new EntityPathMap<TField>(entityKvp.Key, stringArr));
      }

      var entitiesSorted = entities.OrderByDescending(x => x.paths.Length);

      foreach (var entity in entitiesSorted)
      {
        Plugin.Log.LogDebug($"{entity.entity}: {entity.paths.Length}");
        if (all)
        {
          foreach (var path in entity.paths)
          {
            Plugin.Log.LogDebug("- " + path);
          }
        }
        else
        {
          Plugin.Log.LogDebug("- " + entity.paths[0]); 
        }
      }
    }

    private static void ListUIButtonsWithInput()
    {
      var obj = UIRoot.instance.uiGame.gameObject;
      var uiButtons = obj.GetComponentsInChildren<UIButton>(true);

      foreach (var uiButton in uiButtons)
      {
        if (uiButton.input != null)
          Plugin.Log.LogDebug(uiButton.gameObject.GetFullPath());
      }
    }
  }
}
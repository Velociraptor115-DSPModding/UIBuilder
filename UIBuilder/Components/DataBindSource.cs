using System;
using BepInEx.Configuration;

namespace DysonSphereProgram.Modding.UI
{
  public interface IDataBindSource<TVisual>
  {
    TVisual BoundValue { get; set; }
  }
  
  public class DataBindTransform<TLogical, TVisual>
  {
    public Func<TLogical, TVisual> LogicalToVisualTransform;
    public Func<TVisual, TLogical> VisualToLogicalTransform;

    public static DataBindTransform<U, U> Identity<U>()
    {
      return new DataBindTransform<U, U>
      {
        LogicalToVisualTransform = x => x, VisualToLogicalTransform = x => x
      };
    }
  }

  public abstract class DataBindSourceTransformBase<TLogical, TVisual> : IDataBindSource<TVisual>
  {
    private readonly DataBindTransform<TLogical, TVisual> transform;

    protected DataBindSourceTransformBase(DataBindTransform<TLogical, TVisual> transform)
    {
      this.transform = transform;
    }

    public TVisual BoundValue
    {
      get => transform.LogicalToVisualTransform(Get());
      set => Set(transform.VisualToLogicalTransform(value));
    }

    protected abstract TLogical Get();
    protected abstract void Set(TLogical value);
  }

  public class ConfigEntryDataBindSource<T> : IDataBindSource<T>
  {
    private readonly ConfigEntry<T> entry;

    public ConfigEntryDataBindSource(ConfigEntry<T> entry)
    {
      this.entry = entry;
    }

    public T BoundValue
    {
      get => entry.Value;
      set => entry.Value = value;
    }

    public static implicit operator ConfigEntryDataBindSource<T>(ConfigEntry<T> entry) =>
      new ConfigEntryDataBindSource<T>(entry);
  }
  
  public class ConfigEntryDataBindSource<TLogical, TVisual> : DataBindSourceTransformBase<TLogical, TVisual>
  {
    private readonly ConfigEntry<TLogical> entry;

    public ConfigEntryDataBindSource(ConfigEntry<TLogical> entry, DataBindTransform<TLogical, TVisual> transform) :
      base(transform)
    {
      this.entry = entry;
    }

    protected override TLogical Get() => entry.Value;
    protected override void Set(TLogical value) => entry.Value = value;
  }

  public class MemberDataBindSource<U, T> : IDataBindSource<T>
  {
    private readonly U obj;
    private readonly Func<U, T> getter;
    private readonly Action<U, T> setter;

    public MemberDataBindSource(U obj, Func<U, T> getter, Action<U, T> setter)
    {
      this.obj = obj;
      this.getter = getter;
      this.setter = setter;
    }

    public T BoundValue
    {
      get => getter(obj);
      set => setter(obj, value);
    }
  }
  
  public class MemberDataBindSource<U, TLogical, TVisual> : DataBindSourceTransformBase<TLogical, TVisual>
  {
    private readonly U obj;
    private readonly Func<U, TLogical> getter;
    private readonly Action<U, TLogical> setter;

    public MemberDataBindSource(U obj, Func<U, TLogical> getter, Action<U, TLogical> setter
      , DataBindTransform<TLogical, TVisual> transform) : base(transform)
    {
      this.obj = obj;
      this.getter = getter;
      this.setter = setter;
    }

    protected override TLogical Get() => getter(obj);
    protected override void Set(TLogical value) => setter(obj, value);
  }
  
  public class DelegateDataBindSource<T> : IDataBindSource<T>
  {
    private readonly Func<T> getter;
    private readonly Action<T> setter;

    public DelegateDataBindSource(Func<T> getter, Action<T> setter)
    {
      this.getter = getter;
      this.setter = setter;
    }

    public T BoundValue
    {
      get => getter();
      set => setter(value);
    }
  }
  
  public class DelegateDataBindSource<TLogical, TVisual> : DataBindSourceTransformBase<TLogical, TVisual>
  {
    private readonly Func<TLogical> getter;
    private readonly Action<TLogical> setter;

    public DelegateDataBindSource(Func<TLogical> getter, Action<TLogical> setter
      , DataBindTransform<TLogical, TVisual> transform) : base(transform)
    {
      this.getter = getter;
      this.setter = setter;
    }

    protected override TLogical Get() => getter();
    protected override void Set(TLogical value) => setter(value);
  }
}

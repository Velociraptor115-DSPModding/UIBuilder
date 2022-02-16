using System;
using BepInEx.Configuration;

namespace DysonSphereProgram.Modding.UI.Builder
{
  public interface IOneWayDataBindSource
  {
    object Value { get; }
  }
  
  public interface IOneWayDataBindSource<out TVisual>
  {
    TVisual Value { get; }
  }
  
  public interface IDataBindSource<TVisual>
  {
    TVisual Value { get; set; }
  }
  
  public class DataBindTransform<TLogical, TVisual>
  {
    public readonly Func<TLogical, TVisual> LogicalToVisualTransform;
    public readonly Func<TVisual, TLogical> VisualToLogicalTransform;

    public DataBindTransform(Func<TLogical, TVisual> forward, Func<TVisual, TLogical> reverse)
    {
      LogicalToVisualTransform = forward;
      VisualToLogicalTransform = reverse;
    }
  }

  public static class DataBindTransform
  {
    public static DataBindTransform<TLogical, TVisual> From<TLogical, TVisual>
      (Func<TLogical, TVisual> forward, Func<TVisual, TLogical> reverse)
      => new(forward, reverse);

    public static DataBindTransform<T, T> Identity<T>()
      => From<T, T>(x => x, x => x);
  }

  public abstract class DataBindSourceBase<TLogical, TVisual> : IDataBindSource<TVisual>, IOneWayDataBindSource<TVisual>, IOneWayDataBindSource
  {
    protected readonly DataBindTransform<TLogical, TVisual> transform;

    protected DataBindSourceBase(DataBindTransform<TLogical, TVisual> transform)
    {
      this.transform = transform;
    }

    object IOneWayDataBindSource.Value => Value;
    public virtual TVisual Value
    {
      get => transform.LogicalToVisualTransform(Get());
      set => Set(transform.VisualToLogicalTransform(value));
    }

    protected abstract TLogical Get();
    protected abstract void Set(TLogical value);
    
    public DataBindSourceBase<TLogical, UVisual> WithTransform<UVisual>(
      Func<TLogical, UVisual> forward,
      Func<UVisual, TLogical> reverse
    )
      => WithTransform(DataBindTransform.From(forward, reverse));
    
    public abstract DataBindSourceBase<TLogical, UVisual> WithTransform<UVisual>(
      DataBindTransform<TLogical, UVisual> newTransform);
  }

  public class ConfigEntryDataBindSource<TLogical, TVisual> : DataBindSourceBase<TLogical, TVisual>
  {
    private readonly ConfigEntry<TLogical> entry;

    public ConfigEntryDataBindSource(ConfigEntry<TLogical> entry, DataBindTransform<TLogical, TVisual> transform) :
      base(transform)
    {
      this.entry = entry;
    }

    protected override TLogical Get() => entry.Value;
    protected override void Set(TLogical value) => entry.Value = value;
    public override DataBindSourceBase<TLogical, UVisual> WithTransform<UVisual>(
      DataBindTransform<TLogical, UVisual> newTransform)
      => new ConfigEntryDataBindSource<TLogical, UVisual>(entry, newTransform);
  }
  
  public class ConfigEntryDataBindSource<T> : ConfigEntryDataBindSource<T, T>
  {
    public static implicit operator ConfigEntryDataBindSource<T>(ConfigEntry<T> entry) =>
      new (entry);
    public ConfigEntryDataBindSource(ConfigEntry<T> entry) : base(entry, DataBindTransform.Identity<T>()) { }
  }

  public class MemberDataBindSource<U, TLogical, TVisual> : DataBindSourceBase<TLogical, TVisual>
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
    public override DataBindSourceBase<TLogical, UVisual> WithTransform<UVisual>(
      DataBindTransform<TLogical, UVisual> newTransform)
      => new MemberDataBindSource<U, TLogical, UVisual>(obj, getter, setter, newTransform);
  }
  
  public class MemberDataBindSource<U, T> : MemberDataBindSource<U, T, T>
  {
    public MemberDataBindSource(U obj, Func<U, T> getter, Action<U, T> setter)
      : base(obj, getter, setter, DataBindTransform.Identity<T>()) { }
  }

  public class DelegateDataBindSource<TLogical, TVisual> : DataBindSourceBase<TLogical, TVisual>
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
    public override DataBindSourceBase<TLogical, UVisual> WithTransform<UVisual>(
      DataBindTransform<TLogical, UVisual> newTransform)
      => new DelegateDataBindSource<TLogical, UVisual>(getter, setter, newTransform);
  }
  
  public class DelegateDataBindSource<T> : DelegateDataBindSource<T, T>
  {
    public DelegateDataBindSource(Func<T> getter, Action<T> setter)
      : base(getter, setter, DataBindTransform.Identity<T>()) { }
  }
}

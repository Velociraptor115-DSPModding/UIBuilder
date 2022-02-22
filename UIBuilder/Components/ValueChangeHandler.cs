using System;
using UnityEngine;
using UnityEngine.UI;

namespace DysonSphereProgram.Modding.UI.Builder
{
  public abstract class ValueChangeHandler<T> : MonoBehaviour
  {
    public Action<T> Handler;
    protected abstract T CurrentValue { get; }
    protected T prevValue;

    private void OnEnable()
    {
      Handler?.Invoke(CurrentValue);
    }
    private void LateUpdate()
    {
      T newValue;
      if ((newValue = CurrentValue).Equals(prevValue))
        return;
      
      Handler?.Invoke(newValue);
      prevValue = newValue;
    }
  }
  
  public class DataBindValueChangeHandler : ValueChangeHandler<object>
  {
    public IOneWayDataBindSource Binding;
    protected override object CurrentValue => Binding.Value;
  }
  
  public abstract class DataBindValueChangeHandler<T> : ValueChangeHandler<T>
  {
    public IOneWayDataBindSource<T> Binding;
    protected override T CurrentValue => Binding.Value;
  }

  public class DataBindValueChangedHandlerBool : DataBindValueChangeHandler<bool> {}
  public class DataBindValueChangedHandlerInt : DataBindValueChangeHandler<int> {}
  public class DataBindValueChangedHandlerLong : DataBindValueChangeHandler<long> {}
  public class DataBindValueChangedHandlerFloat : DataBindValueChangeHandler<float> {}
  public class DataBindValueChangedHandlerDouble : DataBindValueChangeHandler<double> {}

  public class ToggleValueChangedHandler : ValueChangeHandler<bool>
  {
    public Toggle toggle;
    
    private void Start()
    {
      if (!toggle)
        toggle = GetComponent<Toggle>();
    }
    
    protected override bool CurrentValue => toggle.isOn;
  }
  
  public class SliderValueChangedHandler : ValueChangeHandler<float>
  {
    public Slider slider;
    
    private void Start()
    {
      if (!slider)
        slider = GetComponent<Slider>();
    }
    
    protected override float CurrentValue => slider.value;
  }
}

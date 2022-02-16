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
}

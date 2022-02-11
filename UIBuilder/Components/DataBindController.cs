using System;
using Bulldozer;
using UnityEngine;
using UnityEngine.UI;

namespace DysonSphereProgram.Modding.UI
{
  public abstract class DataBindController<T> : MonoBehaviour
  {
    public IDataBindSource<T> Binding;
    private void LateUpdate()
    {
      if (ChangedFromUI)
        UIToBinding();
      ChangedFromUI = false;
    }

    private void FixedUpdate()
    {
      BindingToUI();
    }

    protected bool ChangedFromUI;
    protected abstract void BindingToUI();
    protected abstract void UIToBinding();
  }

  public class DataBindSliderInt : DataBindController<int>
  {
    private Slider slider;
    
    private void Start()
    {
      slider = GetComponent<Slider>();
      slider.onValueChanged.AddListener(ValueChangedHandler);
    }

    private void ValueChangedHandler(float _)
    {
      ChangedFromUI = true;
    }
    protected override void BindingToUI()
    {
      slider.value = Binding.BoundValue;
    }
    
    protected override void UIToBinding()
    {
      Binding.BoundValue = (int)slider.value;
    }
  }

  public class DataBindSliderFloat : DataBindController<float>
  {
    private Slider slider;
    
    private void Start()
    {
      slider = GetComponent<Slider>();
      slider.onValueChanged.AddListener(ValueChangedHandler);
    }
    
    private void ValueChangedHandler(float _)
    {
      ChangedFromUI = true;
    }

    protected override void BindingToUI()
    {
      slider.value = Binding.BoundValue;
    }
    
    protected override void UIToBinding()
    {
      Binding.BoundValue = slider.value;
    }
  }
  
  public class DataBindToggle : DataBindController<bool>
  {
    private Toggle toggle;
    
    private void Start()
    {
      toggle = GetComponent<Toggle>();
      toggle.onValueChanged.AddListener(ValueChangedHandler);
    }
    
    private void ValueChangedHandler(bool _)
    {
      ChangedFromUI = true;
    }

    protected override void BindingToUI()
    {
      toggle.isOn = Binding.BoundValue;
    }
    
    protected override void UIToBinding()
    {
      Binding.BoundValue = toggle.isOn;
    }
  }
  
  public class DataBindInputFieldString : DataBindController<string>
  {
    private InputField inputField;
    
    private void Start()
    {
      inputField = GetComponent<InputField>();
      inputField.onValueChanged.AddListener(ValueChangedHandler);
    }
    
    private void ValueChangedHandler(string _)
    {
      ChangedFromUI = true;
    }

    protected override void BindingToUI()
    {
      inputField.text = Binding.BoundValue;
    }
    
    protected override void UIToBinding()
    {
      Binding.BoundValue = inputField.text;
    }
  }
  
  public class DataBindInputFieldInt : DataBindController<int>
  {
    private InputField inputField;
    
    private void Start()
    {
      inputField = GetComponent<InputField>();
      inputField.onValueChanged.AddListener(ValueChangedHandler);
    }
    
    private void ValueChangedHandler(string _)
    {
      ChangedFromUI = true;
    }

    protected override void BindingToUI()
    {
      inputField.text = Binding.BoundValue.ToString();
    }
    
    protected override void UIToBinding()
    {
      if (int.TryParse(inputField.text, out var parsedValue))
        Binding.BoundValue = parsedValue;
    }
  }
  
  public class DataBindInputFieldFloat : DataBindController<float>
  {
    private InputField inputField;
    
    private void Start()
    {
      inputField = GetComponent<InputField>();
      inputField.onValueChanged.AddListener(ValueChangedHandler);
    }
    
    private void ValueChangedHandler(string _)
    {
      ChangedFromUI = true;
    }

    protected override void BindingToUI()
    {
      inputField.text = Binding.BoundValue.ToString();
    }
    
    protected override void UIToBinding()
    {
      if (float.TryParse(inputField.text, out var parsedValue))
        Binding.BoundValue = parsedValue;
    }
  }
}

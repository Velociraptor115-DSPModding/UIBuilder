using System;
using UnityEngine;
using UnityEngine.UI;

namespace DysonSphereProgram.Modding.UI.Builder
{
  public abstract class DataBindController<T> : MonoBehaviour
  {
    public IDataBindSource<T> Binding;
    private void LateUpdate()
    {
      if (ChangedFromUI)
      {
        UIToBinding();
        ChangedFromUI = false;
      }
      else
      {
        BindingToUI();
      }
    }

    protected bool ChangedFromUI;
    protected abstract void BindingToUI();
    protected abstract void UIToBinding();
  }

  public class DataBindSlider : DataBindController<float>
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
      slider.value = Binding.Value;
    }
    
    protected override void UIToBinding()
    {
      Binding.Value = slider.value;
    }
  }
  
  public class DataBindToggleBool : DataBindController<bool>
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
      toggle.isOn = Binding.Value;
    }
    
    protected override void UIToBinding()
    {
      Binding.Value = toggle.isOn;
    }
  }
  
  public class DataBindToggleEnum : DataBindController<Enum>
  {
    private Toggle toggle;
    public Enum linkedValue;
    
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
      toggle.isOn = Binding.Value.Equals(linkedValue);
    }
    
    protected override void UIToBinding()
    {
      if (toggle.isOn)
        Binding.Value = linkedValue;
    }
  }
  
  public class DataBindInputField : DataBindController<string>
  {
    private InputField inputField;
    private bool uiDirty;

    private void Start()
    {
      inputField = GetComponent<InputField>();
      inputField.onValueChanged.AddListener(ValueChangedHandler);
      inputField.onEndEdit.AddListener(SubmitHandler);
    }
    
    private void ValueChangedHandler(string value)
    {
      if (value != Binding.Value)
        uiDirty = true;
    }
    
    private void SubmitHandler(string _)
    {
      ChangedFromUI = true;
    }

    protected override void BindingToUI()
    {
      if (!uiDirty)
        inputField.text = Binding.Value;
    }

    protected override void UIToBinding()
    {
      Binding.Value = inputField.text;
      uiDirty = false;
    }
  }
}

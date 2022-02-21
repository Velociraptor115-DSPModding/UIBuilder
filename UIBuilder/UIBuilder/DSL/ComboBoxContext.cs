using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI.Builder;
public static partial class UIBuilderDSL
{
  public record ComboBoxContext : UIElementContext
  {
    public ComboBoxContext(GameObject uiElement) : base(uiElement)
    {
      
    }
    
    public ComboBoxContext Context => this;
  }
}
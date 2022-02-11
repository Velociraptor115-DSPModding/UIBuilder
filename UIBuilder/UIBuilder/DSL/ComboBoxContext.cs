using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DysonSphereProgram.Modding.UI;
public static partial class UIBuilderDSL
{
  public record ComboBoxContext : UIElementContextBase<ComboBoxContext>
  {
    public ComboBoxContext(GameObject uiElement) : base(uiElement)
    {
      
    }
    
    public override ComboBoxContext Context => this;
  }
}
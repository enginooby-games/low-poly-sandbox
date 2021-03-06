using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class PropertyTooltipAttribute : PropertyAttribute {
    private string _label;

    public PropertyTooltipAttribute(string label) => _label = label;
  }
}
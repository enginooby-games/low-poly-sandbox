using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Core {
  // TODO: Implement universal cooldown system using Coroutine
  // TODO: Enable/disable
  /// <summary>
  /// Check IsFinished in Update(), auto reset if true.
  /// </summary>
  [Serializable]
  [InlineProperty]
  public class Cooldown {
    // [LabelText("Cooldown")] 
    [HideLabel] [SuffixLabel("s")] [Min(0f)]
    public float Value;

    public Cooldown(float value = 1f) => Value = value;

    private float _lastResetTime;

    /// <summary>
    /// Check in Update(), auto reset if true.
    /// </summary>
    public bool IsFinished {
      get {
        if (Time.time > _lastResetTime + Value) {
          Reset();
          return true;
        }

        return false;
      }
    }

    public void Reset() {
      _lastResetTime = Time.time;
    }
  }
}
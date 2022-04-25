using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Audio {
  // DEV: How to implement variant for context (e.g., normal audio context, funny audio context)
  // Construct context interface/ScriptableObjectVariant and context concrete implementations using ScriptableObject? 

  /// <summary>
  ///   Centralization for all AudioClipMonoVariant.
  /// </summary>
  // [CreateAssetMenu(fileName = "Context_", menuName = "Audio/Context", order = 0)]
  public class AudioClipContext : ScriptableObject {
    [LabelText(" ")] [InlineEditor] [SerializeField]
    private List<AudioClipVariantSO> _audioCollections = new();

    public List<AudioClipVariantSO> Collections => _audioCollections;
  }
}
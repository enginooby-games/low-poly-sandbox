using System.Collections.Generic;
using Enginooby.Utils;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// REFACTOR: VariantSO
namespace Enginooby.Audio {
  /// <summary>
  ///   List of audio clips for a certain event (e.g. play take damage, enemy dead).
  /// </summary>
  [CreateAssetMenu(fileName = "Audio_", menuName = "Audio/Variant", order = 0)]
  public class AudioClipVariantSO : ScriptableObject {
    [SerializeField] [HideLabel] private AudioClipVariant _value;

    public AudioClipVariant Value => _value;
  }
}
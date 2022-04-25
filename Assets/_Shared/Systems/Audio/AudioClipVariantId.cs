using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Audio {
  /// <summary>
  ///   ID (alternative for audio enum/tag/contract) to construct an audio context contract (which types of sound it has)
  /// </summary>
  [CreateAssetMenu(fileName = "Sfx__Id", menuName = "Audio/Varian ID", order = 0)]
  public class AudioClipVariantId : ScriptableObject {
    // just like default method for interface
    [SerializeField] [BoxGroup("Default Variant")] [HideLabel]
    // [InlineButton(nameof(PreviewDefaultVariant), "▶")]
    private AudioClipVariant _defaultVariant;

    public AudioClipVariant DefaultVariant => _defaultVariant;

    private void PreviewDefaultVariant() => _defaultVariant.Preview();
  }
}
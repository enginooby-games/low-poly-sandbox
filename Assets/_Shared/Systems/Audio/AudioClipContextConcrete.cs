using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Audio {
  [CreateAssetMenu(fileName = "AudioContextConcrete_", menuName = "Audio/Context Concrete", order = 0)]
  public class AudioClipContextConcrete : ScriptableObject {
    [OnValueChanged(nameof(InitAudioClipVariants))] [SerializeField]
    private AudioClipContextContract _contract;

    public AudioClipContextContract Contract => _contract;

    // TODO: protect list from adding/removing 
    // Display fallback variant if not assign variant
    [SerializeField] private List<AudioClipVariantWithId> _audioVariants = new();

    // TODO: Preserver old variants with same ids
    private void InitAudioClipVariants() {
      if (_contract == null) return;

      _audioVariants.Clear();
      foreach (var audioId in _contract.AudioIds) _audioVariants.Add(new AudioClipVariantWithId(audioId));
    }

    // TODO: optimize, maybe replace list by dictionary (how to serialize -> remove AudioClipVariantWithId)
    public AudioClipVariant GetAudio(AudioClipVariantId audioId) {
      var audioVariantSo = _audioVariants.Find(e => e.Id == audioId).VariantSo;

      return audioVariantSo ? audioVariantSo.Value : audioId.DefaultVariant;
    }
  }
}
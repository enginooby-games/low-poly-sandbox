using System;
using System.Collections.Generic;
using Enginooby.Utils;
using UnityEngine;
using UnityEngine.Serialization;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Audio {
  /// <summary>
  ///   [Un-concurrent extension point]
  ///   List of audio clips for a certain event (e.g. play take damage, enemy dead).
  /// </summary>
  [Serializable]
  [InlineProperty]
  public class AudioClipVariant {
    // TODO: Turn mode: random, random iterate, iterate, random other than last
    // TODO: Drag and drop audio clips/folder to add AudioClipProfile
    // TODO: Validate no repeating audio clip in every AudioClipProfile
    // TODO: Implement copy (e.g., from ID default variant to VariantSO)

    [SerializeField]
    [MinMaxSlider(0, 1, true)]
    [OnValueChanged(nameof(UpdateGlobalVolumeRange))]
    [LabelText("Global Volume")]
    private Vector2 _globalVolumeRange = Vector2.one;

    [SerializeField]
    [MinMaxSlider(-3, 3, true)]
    [OnValueChanged(nameof(UpdateGlobalPitchRange))]
    [LabelText("Global Pitch")]
    private Vector2 _globalPitchRange = Vector2.one;

    [FormerlySerializedAs("_variants")] [SerializeField]
    private List<AudioClipProfile> _profiles = new();

    private AudioClipProfile _lastProfile;

    private void UpdateGlobalVolumeRange() {
      _profiles.ForEach(element => element.VolumeRange = _globalVolumeRange);
    }

    private void UpdateGlobalPitchRange() {
      _profiles.ForEach(element => element.PitchRange = _globalPitchRange);
    }

    [Button]
    public void Preview() {
      PlayRandom(AudioSourcePreviewer.Instance);
    }

    public void PlayRandom(AudioSource audioSource) {
      _lastProfile = _profiles.GetRandomOtherThan(_lastProfile);
      _lastProfile.Play(audioSource);
    }

    public void PlayRandom(AudioSourceOperator audioSourceOperator) {
      PlayRandom(audioSourceOperator.AudioSource);
    }
  }
}
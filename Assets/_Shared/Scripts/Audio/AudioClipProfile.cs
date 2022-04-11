using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Audio {
  /// <summary>
  /// Represent an audio clip with fine-tuned range and pitch.
  /// </summary>
  [Serializable]
  [InlineProperty]
  public class AudioClipProfile {
    [FoldoutGroup("$groupName")] [SerializeField] [InlineButton(nameof(PlayInEditMode), "▶")] [HideLabel]
    private AudioClip _audioClip;

    // TODO: Implement locking to prevent global editing in Variant
    [FoldoutGroup("$groupName")] [SerializeField] [MinMaxSlider(0, 1, true)]
    private Vector2 _volumeRange = Vector2.one;

    [FoldoutGroup("$groupName")] [SerializeField] [MinMaxSlider(0, 3, true)]
    private Vector2 _pitchRange = Vector2.one;

    private readonly string groupName = "Audio Clip";

    public Vector2 VolumeRange {
      get => _volumeRange;
      set => _volumeRange = value; // TODO: validation
    }

    public Vector2 PitchRange {
      get => _pitchRange;
      set => _pitchRange = value; // TODO: validation
    }

    public void Play(AudioSource audioSource) {
      audioSource.volume = _volumeRange.Random();
      audioSource.pitch = _pitchRange.Random();
      audioSource.clip = _audioClip;
      audioSource.Play();
    }

    public void PlayInEditMode() {
      if (_audioClip) Play(AudioSourcePreviewer.Instance);
    }
  }
}
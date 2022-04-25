using UnityEditor;
using UnityEngine;

namespace Enginooby.Audio {
  /// <summary>
  /// A global audio source for playing audio clip in edit mode.
  /// </summary>
  public class AudioSourcePreviewer : MonoBehaviourSingleton<AudioSourcePreviewer> {
    private static AudioSource _audioSource;

    public static AudioSource AudioSource {
      get {
        if (_audioSource) return _audioSource;
        Debug.Log("Create new PreviewAudioSource");
        _audioSource = EditorUtility
          .CreateGameObjectWithHideFlags("[Hide] AudioSourcePreviewer", HideFlags.HideAndDontSave,
            typeof(AudioSource)).GetComponent<AudioSource>();
        return _audioSource;
      }
    }

    public static implicit operator AudioSource(AudioSourcePreviewer d) => AudioSource;


    public void Play(AudioClip audioClip) {
      _audioSource.Play(audioClip);
    }
  }
}
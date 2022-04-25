#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using Enginooby.Audio;
using Enginooby.Utils;
using UnityEngine;

namespace Enginooby.Prototype {
  public class AudioManager : MonoBehaviourSingleton<AudioManager> {
    protected override bool IsPersistent => false;

    [SerializeField] private AudioClip _backgroundMusic;

    // ! If the context asset is swapped, need to re-assign every SFX in project
    // ! Can implement clip id to prevent that 
    // [InlineEditor] [SerializeField] private AudioClipContext _audioContext;
    // public AudioClipContext AudioContext => _audioContext;

    [InlineEditor] [SerializeField] private AudioClipContextContract _contextContract;
    public AudioClipContextContract ContextContract => _contextContract;

    [SerializeField] [ValueDropdown(nameof(GetAudioContextConcretes))]
    private AudioClipContextConcrete _contextConcrete;

    private IEnumerable<AudioClipContextConcrete> GetAudioContextConcretes() {
      var contexts = EditorUtils.GetScriptableObjectsOf<AudioClipContextConcrete>();
      return contexts.Where(context => context.Contract == ContextContract);
    }

    private AudioSource _musicSource, _sfxSource;

    public override void AwakeSingleton() {
      base.AwakeSingleton();
      _musicSource = gameObject.AddComponent<AudioSource>();
      _sfxSource = gameObject.AddComponent<AudioSource>();
      _musicSource.clip = _backgroundMusic;
      _musicSource.loop = true;
      _musicSource.Play();
    }

    public void OnGameLoaded() {
      if (_backgroundMusic) _musicSource.Play();
    }

    public void PlayOneShot(AudioClip clip) {
      _sfxSource.PlayOneShot(clip);
    }

    public AudioClipVariant GetAudioVariant(AudioClipVariantId audioId) {
      return _contextConcrete
        ? _contextConcrete.GetAudio(audioId)
        : ContextContract.AudioIds.First(e => e == audioId).DefaultVariant;
    }

    public void PlayOneShot(AudioClipVariantId audioId) {
      GetAudioVariant(audioId).PlayRandom(_sfxSource);
    }

    public static void SetMasterVolume(float value) {
      AudioListener.volume = Math.Clamp(value, 0f, 1f);
    }

    public void ToggleSfx() {
      _sfxSource.mute = !_sfxSource.mute;
    }

    public void ToggleMusic() {
      _musicSource.mute = !_musicSource.mute;
    }
  }
}
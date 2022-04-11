using System;
using UnityEngine;

namespace Enginooby.Prototype {
  public enum UnityEventFunction {
    OnAwake,
    OnStart,
    OnEnable,
    OnDisable,
    OnDestroy,
    OnUpdate,
    OnFixUpdate,
  }

  public class PlaySoundOnUnityEventFunction : MonoBehaviour {
    [SerializeField] private UnityEventFunction _trigger;
    [SerializeField] private AudioClip _clip;

    private void Start() {
      if (_trigger == UnityEventFunction.OnStart) AudioManager.Instance.PlayOneShot(_clip);
    }
  }
}
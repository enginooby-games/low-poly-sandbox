using System;
using UnityEngine;
using UnityEngine.UI;

namespace Enginooby.Prototype {
  [RequireComponent(typeof(Slider))]
  public class VolumeSlider : MonoBehaviour {
    private Slider _slider;

    private void Awake() {
      _slider = GetComponent<Slider>();
      _slider.onValueChanged.AddListener(AudioManager.SetMasterVolume);
      AudioManager.SetMasterVolume(_slider.value);
    }
  }
}
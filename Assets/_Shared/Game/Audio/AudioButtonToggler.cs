using System;
using UnityEngine;
using UnityEngine.UI;

namespace Enginooby.Prototype {
  [RequireComponent(typeof(Button))]
  public class AudioButtonToggler : MonoBehaviour {
    [SerializeField] private bool _toggleMusic;
    [SerializeField] private bool _toggleSfx;

    private Button _button;

    private void Awake() {
      _button = GetComponent<Button>();
    }

    private void Start() {
      if (_toggleMusic) _button.onClick.AddListener(() => AudioManager.Instance.ToggleMusic());
      if (_toggleSfx) _button.onClick.AddListener(() => AudioManager.Instance.ToggleSfx());
    }
  }
}
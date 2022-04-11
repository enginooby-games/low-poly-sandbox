using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Prototype {
  public class UIManager : MonoBehaviourSingleton<UIManager> {
    protected override bool IsPersistent => false;

    #region Global Config

    [SerializeField] private TMP_FontAsset _fontAsset;

    #endregion

    // ===================================================================================================================

    [SerializeField] private GameObject _startMenuPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _gamePlayPanel;

    [SerializeField] private TextMeshProUGUI _gameOverLabel;
    [SerializeField] private GameObject _restartButton;

    private EventSystem _eventSystem;

    private static GameManager GameManager => GameManager.Instance;

    private void OnEnable() {
      GameManager.OnGameLoaded += OnGameLoaded;
      GameManager.OnGameStarted += OnGameStarted;
      GameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable() {
      GameManager.OnGameLoaded -= OnGameLoaded;
    }

    public void OnGameLoaded() {
      // REFACTOR: Extension for null check
      if (_startMenuPanel) _startMenuPanel.SetActive(true);
      if (_gamePlayPanel) _gamePlayPanel.SetActive(false);
      if (_gameOverPanel) _gameOverPanel.SetActive(false);

      if (_gameOverLabel) _gameOverLabel.gameObject.SetActive(false);

      if (_restartButton) {
        _restartButton.SetActive(false);
        if (_restartButton.TryGetComponent(out Button button)) {
          button.onClick.AddListener(() => GameManager.RestartGame());
        }
      }

      // keep only one EventSystem in the scene
      _eventSystem = GetComponentInChildren<EventSystem>();
      foreach (var evenSystem in FindObjectsOfType<EventSystem>()) {
        if (evenSystem != _eventSystem) Destroy(evenSystem.gameObject);
      }
    }

    public void OnGameStarted() {
      if (_startMenuPanel) _startMenuPanel.SetActive(false);
      if (_gamePlayPanel) _gamePlayPanel.SetActive(true);
    }

    public void OnGameOver() {
      if (_startMenuPanel) _startMenuPanel.SetActive(false);
      if (_gamePlayPanel) _gamePlayPanel.SetActive(false);
      if (_gameOverPanel) _gameOverPanel.gameObject.SetActive(true);
      if (_gameOverLabel) _gameOverLabel.gameObject.SetActive(true);
      if (_restartButton) _restartButton.SetActive(true);
    }
  }
}
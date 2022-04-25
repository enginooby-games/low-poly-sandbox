// Comment out to switch to tab view
// #define VIEW_FOLDOUT

using System;
using System.Collections.Generic;
using UnityEngine;
using Event = Enginooby.Core.Event;
// REFACTOR: wrapper for directive
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Prototype {
  /// <summary>
  /// Simple game manager with common events, stats, UI. Used for small, prototype, casual games.
  /// </summary>
  public class GameManager : MonoBehaviourSingleton<GameManager> {
    protected override bool IsPersistent => false;

    private void OnDestroy() {
      OnGameLoaded.RemoveListeners();
      OnGameStarted.RemoveListeners();
      OnGameOver.RemoveListeners();
    }

    // ===================================================================================================================

    #region GAME LOAD

    private const string GameLoadGroupName = "GAME LOAD";

    // REFACTOR: Preprocessor directive?
#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameLoadGroupName))]
#else
    [TabGroup("$" + nameof(GameLoadGroupName))]
#endif
    [InfoBox("Create a GameManager prefab variant first then bind project-specific logic here " +
             "(to make sure the root GameManager prefab contains no object outside GameManager GameObject.")]
    [HideLabel]
    public Event OnGameLoaded = new(nameof(OnGameLoaded));

#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameLoadGroupName))]
#else
    [TabGroup("$" + nameof(GameLoadGroupName))]
#endif
    [SerializeField]
    [ToggleLeft]
    private bool _autoStartGame = true;


    private void Start() {
      LoadGame();
      OnGameLoaded?.Invoke();
      if (_autoStartGame) StartGame();
    }

    /// <summary>
    /// Before game start, usually for main scene.
    /// </summary>
    private void LoadGame() {
      AudioManager.Instance.OnGameLoaded();
    }

    #endregion

    // =================================================================================================================

    #region GAME START

    private const string GameStartGroupName = "GAME START";

    // TIP: Event for assigning edit-time target in GameManager or run-time targets in other scripts
    // IGameActionTarget for assigning run-time targets in GameManager or all of targets of a type
#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameStartGroupName))]
#else
    [TabGroup("$" + nameof(GameStartGroupName))]
#endif
    [HideLabel]
    public Event OnGameStarted = new(nameof(OnGameStarted));

#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameStartGroupName))]
#else
    [SerializeField]
    [TabGroup("$" + nameof(GameStartGroupName))]
#endif
    [SerializeReference]
    [LabelText("Perform Actions On Targets")]
    private List<IGameActionTarget> _actionTargetsOnGameStarted = new();

#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameStartGroupName))]
#else
    [TabGroup("$" + nameof(GameStartGroupName))]
#endif
    [SerializeField]
    private bool _reloadSceneOnRestart = true;

#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameStartGroupName))]
#else
    [TabGroup("$" + nameof(GameStartGroupName))]
#endif
    [Button]
    [GUIColor(1f, .6f, .6f)]
    public void StartGame() {
      IsGameOver = false;
      OnGameStarted.Invoke();
      _actionTargetsOnGameStarted.PerformAction();
    }

#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameStartGroupName))]
#else
    [TabGroup("$" + nameof(GameStartGroupName))]
#endif
    [Button]
    [GUIColor(1f, .6f, .6f)]
    public void RestartGame() {
      Destroy(gameObject);
      if (_reloadSceneOnRestart) SceneUtils.ReloadScene();
    }

    #endregion

    // ===================================================================================================================

    #region GAME OVER

    private const string GameOverGroupName = "GAME OVER";

#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameOverGroupName))]
#else
    [TabGroup("$" + nameof(GameOverGroupName))]
#endif
    [HideLabel]
    public Event OnGameOver = new(nameof(OnGameOver));

    [DisplayAsString] public bool IsGameOver { get; private set; }

#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameOverGroupName))]
#else
    [TabGroup("$" + nameof(GameOverGroupName))]
#endif
    [SerializeReference]
    [LabelText("Perform Actions On Targets")]
    private List<IGameActionTarget> _actionTargetsOnGameOver = new();

#if VIEW_FOLDOUT
    [FoldoutGroup("$" + nameof(GameOverGroupName))]
#else
    [TabGroup("$" + nameof(GameOverGroupName))]
#endif
    [Button]
    [GUIColor(1f, .6f, .6f)]
    public void SetGameOver() {
      if (IsGameOver) return;

      IsGameOver = true;
      OnGameOver.Invoke();
      _actionTargetsOnGameOver.PerformAction();
    }

    #endregion

    // ===================================================================================================================
  }
}
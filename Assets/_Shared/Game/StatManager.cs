using System;
using System.Collections;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Prototype {
  /// <summary>
  /// Manage all global stats which are commonly used in games.
  /// </summary>
  public class StatManager : MonoBehaviourSingleton<StatManager> {
    protected override bool IsPersistent => false;

#if STAT_LIVE
    [GUIColor("@Color.cyan")] [SerializeField] [HideLabel]
    private Stat _liveStat = new("Live");

    public Stat Live => _liveStat;
#endif

#if STAT_SCORE
    [GUIColor("@Color.yellow")] [SerializeField] [HideLabel]
    private Stat _scoreStat = new("Score");

    public Stat Score => _scoreStat;
#endif

#if STAT_TIMER
    [GUIColor("@Color.cyan")] [SerializeField] [HideLabel]
    private Stat _timerStat = new("Timer");

    public Stat Timer => _timerStat;

    // REFACTOR: Countdown coroutine
    public void StartTimer() {
      StartCoroutine(TimerCoroutine());

      IEnumerator TimerCoroutine() {
        while (_timerStat.CurrentValue > _timerStat.MinValue) {
          yield return new WaitForSeconds(1.0f);
          _timerStat.Add(-1);
        }
      }
    }
#endif

    private static GameManager GameManager => GameManager.Instance;

    public void OnGameLoaded() { }

    private void OnEnable() {
#if STAT_LIVE
      if (Live.Enable)
        Live.OnMin += () => GameManager.SetGameOver();
      else
        Live.OnDecreased += () => GameManager.SetGameOver();
#endif
#if STAT_TIMER
      if (Timer.Enable) Timer.OnMin += () => GameManager.SetGameOver();
#endif
      GameManager.OnGameLoaded += OnGameLoaded;
      GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable() {
#if STAT_LIVE
      Live.RemoveEventListeners();
#endif
#if STAT_TIMER
      Timer.RemoveEventListeners();
#endif
#if STAT_SCORE
      Score.RemoveEventListeners();
#endif
      GameManager.OnGameLoaded -= OnGameLoaded;
      GameManager.OnGameStarted -= OnGameStarted;
    }

    [Button]
    [HideInEditorMode]
    public void ResetStats() {
#if STAT_LIVE
      Live.Reset();
#endif
#if STAT_TIMER
      Timer.Reset();
#endif
#if STAT_SCORE
      Score.Reset();
#endif
    }

    public void OnGameStarted() {
#if STAT_TIMER
      if (Timer.Enable) StartTimer();
#endif
    }
  }
}
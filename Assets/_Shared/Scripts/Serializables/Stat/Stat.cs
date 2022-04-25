// * Alternative: Var Reference SO
// ? Make generic for int/float...
// ? Create subclasses (or Bridge pattern) for: StatWithEvent, StatWithUI, StatWithEventAndUI, Stat (w/o event, UI)
// ? Partial classes
// ? Rename to EnhancedInt/Float/Variable
// TODO: Cooldown time to prevent updating continuously (e.g., lose multiple lives when touch many obstacles at the same time)

using System;
using System.Collections;
using System.Collections.Generic;
using Enginooby.Utils;
using UnityEngine;
using Event = Enginooby.Core.Event;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// TODO: Rename to ExtendedInt (Stats is different: strength, defense...)
/// <summary>
/// Wrapper for primitive types, min/max, events & UIs
/// </summary>
[Serializable]
[InlineProperty]
public class Stat {
  public static implicit operator int(Stat stat) => stat.CurrentValue;

  private const float LabelWidth1 = 80f;
  [HideInInspector] public string statName;

  [FoldoutGroup("$statName")]
  [EnableIf(nameof(Enable))]
  [HorizontalGroup("$statName/Debug")]
  [LabelWidth(LabelWidth1)]
  // [ProgressBar(nameof(MinValue), nameof(MaxValue), r: 1, g: 1, b: 1, Height = 30)]
  [DisplayAsString]
  public int CurrentValue;


  // [ToggleGroup(nameof(enable), groupTitle: "$statName")]
  [FoldoutGroup("$statName")]
  [HorizontalGroup("$statName/Enable")]
  [LabelWidth(LabelWidth1)]
  [LabelText("Enable Stat")]
  [OnValueChanged(nameof(OnEnableChanged))]
  public bool Enable = true;

  private void OnEnableChanged() {
    UIs.ForEach(ui => {
      if (ui.label != null) ui.label.enabled = Enable;
    });
  }

  public Stat(string statName, int initialValue = 0) {
    this.statName = statName;
    InitialValue = initialValue;
    CurrentValue = initialValue;
    // this.ui.statName = statName;
  }

  // REFACTOR
  public Stat(StatName statName, int initialValue = 0) {
    this.statName = statName.ToString();
    InitialValue = initialValue;
    CurrentValue = initialValue;
    // this.ui.statName = statName;
  }

  // ===================================================================================================================

  // REFACTOR: Wrapper class for data with min, max

  #region VALUES

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName")]
  [EnableIf(nameof(Enable))]
  [HorizontalGroup("$statName/Value")]
  [LabelWidth(LabelWidth1)]
  [OnValueChanged(nameof(OnInitialValueChanged))]
  public int InitialValue;

  private void OnInitialValueChanged() {
    CurrentValue = InitialValue;
    InitUIs();
  }

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName")] [EnableIf(nameof(Enable))] [HorizontalGroup("$statName/Enable")] [LabelWidth(LabelWidth1)]
  public bool EnableMin = true;

  [FoldoutGroup("$statName")]
  [EnableIf(nameof(IsMinEnabled))]
  [HorizontalGroup("$statName/Value")]
  [LabelWidth(LabelWidth1)]
  public int MinValue;

  [FoldoutGroup("$statName")] [EnableIf(nameof(Enable))] [HorizontalGroup("$statName/Enable")] [LabelWidth(LabelWidth1)]
  public bool EnableMax;

  [FoldoutGroup("$statName")]
  [EnableIf(nameof(IsMaxEnabled))]
  [HorizontalGroup("$statName/Value")]
  [LabelWidth(LabelWidth1)]
  [PropertySpace(SpaceAfter = 10, SpaceBefore = 0)]
  public int MaxValue = 100;

  private bool IsMinEnabled => Enable && EnableMin;
  private bool IsMaxEnabled => Enable && EnableMax;
  private bool IsMaxDisabled => Enable && !EnableMax;

  #endregion

  // ===================================================================================================================

  #region UI

  // TODO: Implement multiple UIs (? scriptable objects)
  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName")] [ShowIf(nameof(Enable))] [OnValueChanged(nameof(InitUIs), true)] [LabelText("UIs")]
  public List<StatUI> UIs = new() {new StatUI()};


  [FoldoutGroup("$statName")]
  // [EnableIf(nameof(EnableStatAndMax))]
  // [HorizontalGroup("$statName/Debug"), LabelWidth(LABEL_WIDTH_1)]
  // [Button]
  public float CurrentPercentage => CurrentValue * 100f / MaxValue;

  private void InitUIs() {
    UpdateUIs(InitialValue);
    // this.uis.ForEach(ui => ui.prefix = statName + ": ");
  }

  private void UpdateUIs(int currentValue) {
    var maxValue = EnableMax ? (int?) MaxValue : null;
    var minValue = EnableMin ? (int?) MinValue : null;
    UIs.ForEach(ui => ui.Update(currentValue, maxValue, minValue));
  }

  public void DestroyUIs() {
    UIs.ForEach(ui => ui.Destroy());
  }

  #endregion

  // ===================================================================================================================

  #region EVENTS

  [FoldoutGroup("$statName/Events")] [ShowIf(nameof(Enable))] [HideLabel]
  public Event OnChanged = new(nameof(OnChanged));

  [FoldoutGroup("$statName/Events")] [ShowIf(nameof(Enable))] [HideLabel]
  public Event OnIncreased = new(nameof(OnIncreased));

  [FoldoutGroup("$statName/Events")] [ShowIf(nameof(Enable))] [HideLabel]
  public Event OnDecreased = new(nameof(OnDecreased));

  [FoldoutGroup("$statName/Events")] [ShowIf(nameof(EnableMin))] [HideLabel]
  public Event OnMin = new(nameof(OnMin));

  [FoldoutGroup("$statName/Events")] [ShowIf(nameof(EnableMax))] [HideLabel]
  public Event OnMax = new(nameof(OnMax));

  [FoldoutGroup("$statName/Events")] [ShowIf(nameof(Enable))] [HideLabel]
  public Event OnZero = new(nameof(OnZero));

  // [SerializeField] [HideInInspector] private bool _showOnChangedEvent = true;
  // [SerializeField] [HideInInspector] private bool _showOnIncreasedEvent = true;
  // [SerializeField] [HideInInspector]  private bool _showOnDecreasedEvent = true;
  // [SerializeField] [HideInInspector] private bool _showOnMinEvent = true;
  // [SerializeField] [HideInInspector] private bool _showOnMaxEvent = true;
  // [SerializeField] [HideInInspector]  private bool _showOnZeroEvent = true;

  private void InvokeEventsOnChanged(int oldValue) {
    OnChanged?.Invoke();
    OnIncreased?.Invoke(CurrentValue > oldValue);
    OnDecreased?.Invoke(CurrentValue < oldValue);
    OnZero?.Invoke(CurrentValue == 0);
    OnMin?.Invoke(EnableMin && CurrentValue <= MinValue);
    OnMax?.Invoke(EnableMax && CurrentValue >= MaxValue);
  }

  /// <summary>
  /// Remove all listeners of all events associated with this variable (OnChanged, OnIncreased, etc.)
  /// </summary>
  public void RemoveEventListeners() {
    OnChanged.RemoveListeners();
    OnIncreased.RemoveListeners();
    OnDecreased.RemoveListeners();
    OnMin.RemoveListeners();
    OnMax.RemoveListeners();
    OnZero.RemoveListeners();
  }

  #endregion

  // ===================================================================================================================

  #region PUBLIC METHODS

  // TODO: Lerp add value over time
  public void Add(int amount) => Set(CurrentValue + amount);

  // ? Use a singleton MonoBehaviour instead, w/ extension method: Coroutine.Start()
  /// <summary>
  ///   Add temporarily for a period of time, then return to the previous value.
  ///   Provide MonoBehaviour to start coroutine.
  /// </summary>
  public void Add(int amount, float duration, MonoBehaviour monoBehaviour) {
    monoBehaviour.StartCoroutine(AddCoroutine(amount, duration));

    IEnumerator AddCoroutine(int theAmount, float theDuration) {
      Add(theAmount);
      yield return new WaitForSeconds(theDuration);
      Add(-theAmount);
    }
  }

  public void SetMin() => Set(MinValue);
  public void SetMax() => Set(MaxValue);

  /// <summary>
  /// Set to initial value without invoking events.
  /// </summary>
  [FoldoutGroup("$statName")]
  [HideInEditorMode]
  [Button]
  public void Reset() {
    CurrentValue = InitialValue;
    UpdateUIs(CurrentValue);
  }

  // TODO: Lerp set to value over time
  // ? Set value without invoking events
  // REFACTOR
  /// <summary>
  ///   Constraint (clamp) the given amount in range of min-max (if enable) before setting value.
  /// </summary>
  public void Set(int value) {
    // clamp in min max range
    if (EnableMin && value < MinValue) value = MinValue;
    if (EnableMax && value > MaxValue) value = MaxValue;
    if (value == CurrentValue) return;

    var oldValue = CurrentValue;
    CurrentValue = value;
    UpdateUIs(CurrentValue);
    InvokeEventsOnChanged(oldValue);
  }

  #endregion
}
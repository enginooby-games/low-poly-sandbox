#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif
using System;
using UnityEngine;

namespace Enginooby.Core {
  /// <summary>
  /// Represent a (boolean) field which can be enabled, disabled, toggled. Also included triggers and events.
  /// </summary>
  [Serializable]
  [InlineProperty]
  public class ToggleableState : ISimplifyInspectorView {
    private string _stateName;

    public ToggleableState(string stateName = "State") {
      _stateName = stateName;
    }

    // TIP: Simplify inspector view
    // [FoldoutGroup("$" + nameof(_stateName))] 
    [Tooltip("Hide all disabled, non-assigned fields & events.")]
    [OnValueChanged(nameof(ToggleSimplifiedInspectorView))]
    [ToggleLeft]
    [SerializeField]
    private bool _simplifyInspectorView;

    // REFACTOR: Wrapper for field with simplified inspector view
    private void ToggleSimplifiedInspectorView() {
      if (_enableKey == KeyCode.None)
        _showEnableKey = !_simplifyInspectorView;
      if (_disableKey == KeyCode.None)
        _showDisableKey = !_simplifyInspectorView;
      if (_toggleKey == KeyCode.None)
        _showToggleKey = !_simplifyInspectorView;
      if (!OnEnabled.Enabled || OnEnabled.InspectorBindingCount == 0)
        _showOnEnabled = !_simplifyInspectorView;
      if (!OnDisabled.Enabled || OnDisabled.InspectorBindingCount == 0)
        _showOnDisabled = !_simplifyInspectorView;
    }

    public void SimplifyInspectorView(bool isSimplified) {
      _simplifyInspectorView = isSimplified;
      ToggleSimplifiedInspectorView();
    }

    // REFACTOR: Implement serializable wrapper for enable, disable, toggle key (EDTKeyCode)

    // REFACTOR: Implement generic trigger for input, area trigger, etc

    // [FoldoutGroup("$" + nameof(_stateName))] 
    [SerializeField] [ShowIf(nameof(_showEnableKey))]
    private KeyCode _enableKey;

    [SerializeField] [HideInInspector] private bool _showEnableKey = true;

    // [FoldoutGroup("$" + nameof(_stateName))] 
    [SerializeField] [ShowIf(nameof(_showDisableKey))]
    private KeyCode _disableKey;

    [SerializeField] [HideInInspector] private bool _showDisableKey = true;

    // [FoldoutGroup("$" + nameof(_stateName))] 
    [SerializeField] [ShowIf(nameof(_showToggleKey))]
    private KeyCode _toggleKey;

    [SerializeField] [HideInInspector] private bool _showToggleKey = true;

    // [FoldoutGroup("$" + nameof(_stateName))] 
    [HideLabel] [ShowIf(nameof(_showOnEnabled))]
    public Event OnEnabled = new(nameof(OnEnabled));

    [SerializeField] [HideInInspector] private bool _showOnEnabled = true;

    // [FoldoutGroup("$" + nameof(_stateName))] 
    [HideLabel] [ShowIf(nameof(_showOnDisabled))]
    public Event OnDisabled = new(nameof(OnDisabled));

    [SerializeField] [HideInInspector] private bool _showOnDisabled = true;

    public bool IsEnabled { get; private set; } = true;


    public void Enable() {
      IsEnabled = true;
      OnEnabled?.Invoke();
    }

    public void Disable() {
      IsEnabled = false;
      OnDisabled?.Invoke();
    }

    public void Toggle() {
      if (IsEnabled) Disable();
      else Enable();
    }

    public void RemoveEventListeners() {
      OnEnabled.RemoveListeners();
      OnDisabled.RemoveListeners();
    }

    public void ProcessTriggers() {
      if (_enableKey.IsDown()) Enable();
      if (_disableKey.IsDown()) Disable();
      if (_toggleKey.IsDown()) Toggle();
    }
  }
}
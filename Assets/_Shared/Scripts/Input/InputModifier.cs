// Wrapper for Keycode (decrete value 0 & 1), Input Manager Axis (continous value -1 <-> 1) + Modifier key (while holding)

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

using System;
using UnityEngine;
using static InputUtils;

[Flags]
public enum KeyCodeTriggerEvent {
  Up = 1 << 1,
  Down = 1 << 2,
  Hold = 1 << 3,
}

public enum InputAxis {
  None,
  Horizontal,
  Vertical,
  Fire1,
  Jump,
} // TODO: add more

[Serializable]
[InlineProperty]
public class InputModifier {
  public enum InputType {
    KeyCode,
    Axis,
  }

  [SerializeField] [HideInInspector] private InputType inputType = InputType.KeyCode;
  [SerializeField] [HideInInspector] private string inputTypeName = "A";

  private void ToggleInputType() {
    inputType = inputType == InputType.KeyCode ? InputType.Axis : InputType.KeyCode;
    inputTypeName = inputType == InputType.KeyCode ? "C" : "A";
  }

  #region KEYCODE ===================================================================================================================================

  [InlineButton(nameof(ToggleShowMore), "...")]
  [InlineButton(nameof(ToggleInputType), "$inputTypeName")]
  [InlineButton(nameof(SetKeyCodeNone), "Ø")]
  [InlineButton(nameof(IncreaseKeyCode), ">")]
  [InlineButton(nameof(DescreaseKeyCode), "<")]
  [ShowIf(nameof(inputType), InputType.KeyCode)]
  [HideLabel]
  public KeyCode keyCode;

  private void IncreaseKeyCode() {
    keyCode++; // TODO: constrain max value
  }

  private void DescreaseKeyCode() {
    if (keyCode > 0) keyCode--;
  }

  private void SetKeyCodeNone() {
    keyCode = KeyCode.None;
  }

  [ShowIf(nameof(ShowKeyTriggerEvent))] [HideLabel] [EnumToggleButtons] [GUIColor(.6f, 1f, .6f, .8f)]
  public KeyCodeTriggerEvent keyTriggerEvent = KeyCodeTriggerEvent.Down;

  private bool ShowKeyTriggerEvent() => inputType == InputType.KeyCode && showMore;

  public bool IsKeyCodeTriggering {
    get {
      if (!modifierKey.IsHeld()) return false;
      return
        keyTriggerEvent.HasFlag(KeyCodeTriggerEvent.Down) && keyCode.IsDown() ||
        keyTriggerEvent.HasFlag(KeyCodeTriggerEvent.Up) && keyCode.IsUp() ||
        keyTriggerEvent.HasFlag(KeyCodeTriggerEvent.Hold) && keyCode.IsHeld();
    }
  }

  private float InputValueKeyCode => IsKeyCodeTriggering ? 1 : 0;

  public InputModifier(
    InputType inputType = InputType.KeyCode,
    KeyCode keyCode = KeyCode.None,
    ModifierKey modifierKey = (ModifierKey) 1,
    KeyCodeTriggerEvent keyTriggerEvent = KeyCodeTriggerEvent.Down,
    InputAxis inputAxis = InputAxis.None) {
    this.inputType = inputType;
    this.keyCode = keyCode;
    this.modifierKey = modifierKey;
    this.keyTriggerEvent = keyTriggerEvent;
    this.inputAxis = inputAxis;
  }

  #endregion ===================================================================================================================================

  #region AXIS ===================================================================================================================================

  [InlineButton(nameof(ToggleShowMore), "...")]
  [InlineButton(nameof(ToggleInputType), "$inputTypeName")]
  [InlineButton(nameof(SetInputAxisNone), "Ø")]
  [InlineButton(nameof(IncreaseAxis), ">")]
  [InlineButton(nameof(DecreaseAsix), "<")]
  [ShowIf(nameof(inputType), InputType.Axis)]
  [HideLabel]
  public InputAxis inputAxis = InputAxis.None;

  private void SetInputAxisNone() {
    inputAxis = InputAxis.None;
  }

  private void IncreaseAxis() {
    inputAxis++;
  }

  private void DecreaseAsix() {
    if (inputAxis > 0) inputAxis--;
  }

  private bool IsAxisTriggering => InputValueAxis == 0 ? false : true;

  private float InputValueAxis {
    get {
      if (!modifierKey.IsHeld()) return 0;
      if (inputAxis == InputAxis.Horizontal) return HorizontalInput;
      if (inputAxis == InputAxis.Vertical) return VerticalInput;
      return 0;
    }
  }

  #endregion ===================================================================================================================================

  #region COMMON ===================================================================================================================================

  [ShowIf(nameof(showMore))] [HideLabel] [EnumToggleButtons]
  public ModifierKey modifierKey = (ModifierKey) 1;

  private bool showMore;

  private void ToggleShowMore() {
    showMore = !showMore;
  }

  public bool IsTriggering => inputType == InputType.KeyCode ? IsKeyCodeTriggering : IsAxisTriggering;

  public float InputValue => inputType == InputType.KeyCode ? InputValueKeyCode : InputValueAxis;

  #endregion ===================================================================================================================================
}
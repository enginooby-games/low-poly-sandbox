// REFACTOR: Separate different UI types to classes implementing IStatUI
// Rename to Number/VariableUI

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

#if ASSET_DOTWEEN
using DG.Tweening;
#endif

/// <summary>
/// 
/// </summary>
[Serializable]
[InlineProperty]
public class StatUI {
  public enum UIType {
    Text,
    Icon,
    Slider,
    Image,
  }

  public const float LABEL_WIDTH = 45f;

  [HideInInspector] public string statName; // REFACTOR: Use StringBuilder

  [BoxGroup("$statName")] [HideLabel] [EnumToggleButtons]
  public UIType uiType = UIType.Text;

  public StatUI(string statName = "UI", string prefix = null, string suffix = null) => this.statName = statName;

  public void Destroy() {
    if (_fillImage) _fillImage.transform.parent.gameObject.Destroy();
  }

  public void Update(int currentValue, int? maxValue = null, int? minValue = null) {
    switch (uiType) {
      case UIType.Text:
        if (label) {
          label.text = _stringBuilder
              .Replace("{current}", currentValue.ToString())
              .Replace("{min}", minValue.ToString())
              .Replace("{max}", maxValue.ToString())
            ;
        }

        break;
      case UIType.Slider:
        if (maxValue.HasValue && _slider) {
          var fraction = currentValue / (float) maxValue.Value;
#if ASSET_DOTWEEN
          _slider.DOValue(fraction, _sliderUpdateSpeed).SetSpeedBased(true);
#endif
          if (_enableSliderFillGradient && _sliderFillImage) {
            _sliderFillImage.color = _sliderFillGradient.Evaluate(fraction);
          }

          if (_destroySliderOnZero && Mathf.Approximately(fraction, 0f)) Object.Destroy(_slider.gameObject);
        }

        break;
      case UIType.Image:
        // REACTOR: duplicated code with slider
        if (maxValue.HasValue && _fillImage) {
          var fraction = currentValue / (float) maxValue.Value;
          _fillImage.fillAmount = fraction;

          if (_fillImageGradient != null) {
            _fillImage.color = _fillImageGradient.Evaluate(fraction);
          }
        }

        break;
    }
  }

  #region TEXT ===================================================================================================================================

  [BoxGroup("$statName")] [LabelWidth(LABEL_WIDTH)] [ShowIf(nameof(uiType), UIType.Text)] [HideLabel]
  // FIX: label text not serialized if label is an instance prefab or child of an instance prefab
  public TextMeshProUGUI label;

  // TODO: Implement string parsing in Inspector instead of pre/suffix for more flexibility
  // E.g., "Health: {current}/{max}" -> "Health: 69/100"
  // https://stackoverflow.com/a/63547973
  // https://answers.unity.com/questions/1636916/can-you-use-a-variable-in-the-text-editor-of-the-u.html
  // REFACTOR: select variables instead of writing string in Inspector
  [SerializeField]
  [BoxGroup("$statName")]
  [LabelText("Builder")]
  [LabelWidth(LABEL_WIDTH)]
  [ShowIf(nameof(uiType), UIType.Text)]
  [InfoBox("Parse variables: {current}, {min}, {max}")]
  private string _stringBuilder = "{current}";

  #endregion ===================================================================================================================================

  #region ICON ===================================================================================================================================

  // [ShowIf(nameof(statType), UIType.Icon)]
  // public Image?? statIcon;

  #endregion ===================================================================================================================================

  #region SLIDER ===================================================================================================================================

  [BoxGroup("$statName")] [ShowIf(nameof(uiType), UIType.Slider)] [SerializeField]
  private Slider _slider;

  [BoxGroup("$statName")]
  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField]
  [LabelText("Update Speed")]
  [Range(.1f, 1f)]
  private float _sliderUpdateSpeed = .5f; // TODO: ease update

  [BoxGroup("$statName")] [ShowIf(nameof(uiType), UIType.Slider)] [SerializeField] [LabelText("Destroy On Zero")]
  private bool _destroySliderOnZero;

  [BoxGroup("$statName")] [ShowIf(nameof(uiType), UIType.Slider)] [SerializeField] [LabelText("Enable Fill Gradient")]
  private bool _enableSliderFillGradient;

  [BoxGroup("$statName")]
  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField]
  [EnableIf(nameof(_enableSliderFillGradient))]
  [LabelText("Fill Gradient")]
  private Gradient _sliderFillGradient;

  [BoxGroup("$statName")]
  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField]
  [EnableIf(nameof(_enableSliderFillGradient))]
  [LabelText("Fill Image")]
  private Image _sliderFillImage;

  // TODO: Fading time, hide when max

  #endregion ===================================================================================================================================

  #region SLIDER ===================================================================================================================================

  [BoxGroup("$statName")] [ShowIf(nameof(uiType), UIType.Image)] [SerializeField]
  private Image _fillImage;

  [BoxGroup("$statName")] [ShowIf(nameof(uiType), UIType.Image)] [SerializeField] [LabelText("Fill Gradient")]
  private Gradient _fillImageGradient;

  #endregion
}
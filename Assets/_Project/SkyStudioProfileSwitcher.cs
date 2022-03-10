using System.Collections.Generic;
using Enginooby.Utils;
using Funly.SkyStudio;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

public class SkyStudioProfileSwitcher : MonoBehaviour {
  [SerializeField] private TimeOfDayController _timeOfDayController;

  [Tooltip("How long the transition animation will last.")] [Range(0.1f, 30)] [SerializeField]
  private float _transitionDuration = 2;

  [SerializeField] private List<SkyProfile> _skyProfiles;

  [SerializeField] [ValueDropdown(nameof(_skyProfiles))] [OnValueChanged(nameof(UpdateCurrentProfile))]
  private SkyProfile _currentSkyProfile;

  private void Update() {
    if (KeyCode.V.IsDown()) {
      _currentSkyProfile = _skyProfiles.GetNext(_currentSkyProfile);
      UpdateCurrentProfile();
    }
  }

  public void UpdateCurrentProfile() {
    _timeOfDayController.StartSkyProfileTransition(_currentSkyProfile, _transitionDuration);
  }
}
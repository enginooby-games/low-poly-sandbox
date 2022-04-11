#if ASSET_PUPPET_MASTER
using RootMotion.Dynamics;
#endif
#if ASSET_DOTWEEN
using DG.Tweening;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif
using Enginooby.Core;
using UnityEngine;

namespace Enginooby.Controller {
  public class PlayerController : MonoBehaviour {
    private void Update() {
      _ragdollState.ProcessTriggers();
    }

    #region Ragdoll

    [SerializeField] [HideLabel] [FoldoutGroup("Ragdoll")]
    private ToggleableState _ragdollState;

    // REFACTOR: Initialize ToggleableState with delegates on enable/disable/toggle
    private void OnEnable() {
      _ragdollState.OnDisabled += OnRagdollDisabled;
      _ragdollState.OnEnabled += OnRagdollEnabled;
    }

    private void OnDisable() => _ragdollState.RemoveEventListeners();

    public void EnableRagdoll() => _ragdollState.Enable();
    public void DisableRagdoll() => _ragdollState.Disable();
    public void ToggleRagdoll() => _ragdollState.Toggle();

#if ASSET_PUPPET_MASTER
    [FoldoutGroup("Ragdoll")] [SerializeField]
    private PuppetMaster _puppetMaster;
#endif

    private void OnRagdollEnabled() {
#if ASSET_PUPPET_MASTER
      // REFACTOR: PuppetMasterExtension
      DOTween.To(() => _puppetMaster.mappingWeight, x => _puppetMaster.mappingWeight = x, 1f, 1f);
#endif
    }

    private void OnRagdollDisabled() {
#if ASSET_PUPPET_MASTER
      DOTween.To(() => _puppetMaster.mappingWeight, x => _puppetMaster.mappingWeight = x, 0f, 1f);
#endif
    }

    #endregion
  }
}
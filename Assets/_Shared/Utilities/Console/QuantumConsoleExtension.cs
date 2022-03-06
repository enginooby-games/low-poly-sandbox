#if ASSET_QUANTUM_CONSOLE
using System.Collections.Generic;
using System.Linq;
using QFSW.QC;
using UnityEngine;

[RequireComponent(typeof(QuantumConsole))]
public class QuantumConsoleExtension : MonoBehaviour {
  [SerializeField] private List<MonoBehaviour> _disableOnConsoleActive = new();

  private QuantumConsole _quantumConsole;

  private void OnEnable() {
    _quantumConsole.OnActivate += DeactivateComponents;
    _quantumConsole.OnDeactivate += ActivateComponents;
  }

  private void OnDisable() {
    _quantumConsole.OnActivate -= DeactivateComponents;
    _quantumConsole.OnDeactivate -= ActivateComponents;
  }

  private void Awake() {
    _quantumConsole = GetComponent<QuantumConsole>();
  }

  private void ActivateComponents() => SetActive(true);

  private void DeactivateComponents() => SetActive(false);

  private void SetActive(bool isActive) {
    foreach (var monoBehaviour in _disableOnConsoleActive.Where(monoBehaviour => monoBehaviour != null)) {
      monoBehaviour.enabled = isActive;
    }
  }
}
#endif
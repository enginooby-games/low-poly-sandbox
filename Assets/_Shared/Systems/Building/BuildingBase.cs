using System;
using UnityEngine;

namespace Enginooby.Building {
  /// <summary>
  /// Trigger constructing building.
  /// </summary>
  // TODO: Setup RigidBody & Collider
  public class BuildingBase<T> : MonoBehaviour where T : MonoBehaviour {
    [Tooltip("Usually the height of the base")] [SerializeField]
    private float _baseOffset = 1f;

    protected T _building;
    protected BuildingManager<T> _buildingManager;

    private void Awake() {
      _buildingManager = FindObjectOfType<BuildingManager<T>>();
    }

    public void Construct() {
      if (_building) return;

      var prefab = _buildingManager.CurrentPrefab;
      _building = Instantiate(prefab, transform);
      _building.transform.localPosition = Vector3.zero.WithY(_baseOffset);
    }
  }
}
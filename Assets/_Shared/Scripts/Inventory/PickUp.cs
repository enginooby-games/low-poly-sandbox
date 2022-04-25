using System;
using Enginooby.Core;
using UnityEngine;
using Event = Enginooby.Core.Event;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Inventory {
  /// <summary>
  /// Picking up by trigger with a component picker.
  /// </summary>
  public abstract class PickUp<TItem> : PickUp where TItem : ScriptableObject, ICollectable {
    // TODO: SFX/VFX on picked up
    [SerializeField] protected TItem _itemType;

    [SerializeField] protected RandomInt _amount = new(1, 1);

    private void OnTriggerEnter(Collider other) {
      if (!other.TryGetComponent(out Bag<TItem> picker)) return;

      picker.AddAmount(_itemType, _amount);
      OnPickedUpBy(picker);
      Destroy(gameObject);
    }

    protected virtual void OnPickedUpBy(Bag<TItem> picker) { }
  }

  public abstract class PickUp : MonoBehaviour {
    // TODO: Rotate around global pivot (this transform) vs. local pivots (children transform)
    [SerializeField] private Vector3 _rotateSpeed = new(0, 60, 0);

    [HideLabel] public Event OnPickedUp = new("On Picked Up");

    private void Update() {
      transform.Rotate(_rotateSpeed * Time.deltaTime);
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enginooby.Inventory {
  public abstract class Bag<TItem> : MonoBehaviour where TItem : ScriptableObject, ICollectable {
    // ! Cannot serialize dictionary by default, hence create serializable class AmmoSlot
    // ! Drawback: Ammo type can be duplicated w/o dictionary
    // [SerializeField] private Dictionary<TItem, int> _test = new();

    [Serializable]
    private class ItemSlot {
      public TItem ItemType;
      public int Amount;

      // ? UI
    }

    [SerializeField] private List<ItemSlot> _slots = new();

    private ItemSlot GetSlot(ICollectable itemType) {
      // print(((ScriptableObject) itemType).GetType());
      // print(itemType.GetType());
      // ? How to get instance type of SO instead of comparing names
      return _slots.First(slot => slot.ItemType.name == ((ScriptableObject) itemType).name);
    }

    public int GetAmount(ICollectable itemType) => GetSlot(itemType).Amount;

    public void AddAmount(ICollectable itemType, int amountToAdd) => GetSlot(itemType).Amount += amountToAdd;
  }
}
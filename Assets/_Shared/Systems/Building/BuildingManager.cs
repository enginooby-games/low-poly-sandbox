using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Building {
  public class BuildingManager<T> : MonoBehaviour where T : MonoBehaviour {
    [SerializeField] [InlineEditor] protected List<T> _prefabs = new();

    [SerializeField] [ValueDropdown(nameof(_prefabs))]
    protected T _currentPrefab;

    public T CurrentPrefab => _currentPrefab;
  }
}
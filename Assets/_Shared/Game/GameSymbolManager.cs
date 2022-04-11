using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// TODO: implement singleton, otherwise causes conflict
namespace Enginooby.Prototype {
  [CreateAssetMenu(fileName = "Game Symbol Manager", menuName = "Game/Symbol Manager", order = 0)]
  public class GameSymbolManager : ScriptableObject {
    [SerializeField] [OnValueChanged(nameof(SetDirty))]
    private bool _enableStatLive = true;

    [SerializeField] [OnValueChanged(nameof(SetDirty))]
    private bool _enableStatScore = true;

    [SerializeField] [OnValueChanged(nameof(SetDirty))]
    private bool _enableStatTimer = true;

    // Whether symbol modifications have been updated?
    private bool _dirty;

    private new void SetDirty() => _dirty = true;

    [Button]
    [ShowIf(nameof(_dirty))]
    private void UpdateSymbols() {
      CompilingSymbolUtils.AddOrRemove(false, "STAT_LIVE", _enableStatLive);
      CompilingSymbolUtils.AddOrRemove(false, "STAT_SCORE", _enableStatScore);
      CompilingSymbolUtils.AddOrRemove(false, "STAT_TIMER", _enableStatTimer);
      CompilingSymbolUtils.UpdateDefines();
      _dirty = false;
    }
  }
}
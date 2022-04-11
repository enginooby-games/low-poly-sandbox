using System;
using System.Collections.Generic;
using System.Linq;
using Enginooby.Utils;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Prototype {
  [Serializable]
  public abstract class GameActionTarget<T> : IGameActionTarget where T : UnityEngine.Object {
    private enum AssignMethod {
      All, // FindObjectsOfType
      Custom, // Manually assign in Inspector
    }

    [HorizontalGroup("1")] [SerializeField]
    private bool _enable = true;

    [HorizontalGroup("1")] [SerializeField] [ValueDropdown(nameof(Actions))] [HideLabel]
    protected string _action;

    [SerializeField] [HideLabel] [EnumToggleButtons]
    private AssignMethod _assignMethod = AssignMethod.All;

    // [ShowIf(nameof(_assignMethod), AssignMethod.Custom)]
    [ShowIf(nameof(IsAssignMethodCustom))] [SceneObjectsOnly] [SerializeField]
    protected List<T> _targetReferences = new();

    private bool IsAssignMethodCustom => _assignMethod == AssignMethod.Custom;

    protected List<T> TargetReferences =>
      _assignMethod == AssignMethod.Custom ? _targetReferences : UnityEngine.Object.FindObjectsOfType<T>().ToList();

    // TODO: Common action & component-specific action lists
    protected abstract List<string> Actions { get; }

    public void PerformAction() {
      if (!_enable) return;
      
      // common action types
      if (_action == "Destroy") {
        TargetReferences.ForEach(UnityEngine.Object.Destroy);
      }
        
      if (typeof(T).Is<MonoBehaviour>() && _action == "DestroyGO") {
        TargetReferences.ForEach(e => (e as MonoBehaviour).DestroyGameObject());
      }
        
      // component-specific action types
      OnActionPerformed();
    }

    public abstract void OnActionPerformed();
  }

  public static class GameActionTargetUtils {
    public static void PerformAction(this IEnumerable<IGameActionTarget> targets) {
      targets.ForEach(target => target.PerformAction());
    }
  }

  public interface IGameActionTarget {
    void PerformAction();
  }
}
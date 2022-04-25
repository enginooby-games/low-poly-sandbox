using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.AI {
  /// <summary>
  /// <a href="https://notion.so/d5f9ac8810ba47f79cd7c8d9aad90f06">Docs</a>
  /// </summary>
  // TODO: Implement wall block (use raycast?)
  // https://www.udemy.com/course/unitycourse2/learn/lecture/15015316#questions/7516568
  public class Vision : MonoBehaviour {
    [SerializeField] [HideLabel] private AreaCircular _range = new("Vision", 10f);

    public AreaCircular Range => _range;

    private void Reset() {
      _range.SetGameObject(gameObject);
    }

    private void Awake() {
      _range.SetGameObject(gameObject);
    }

    private void OnDrawGizmos() {
      _range.DrawGizmos();
    }

    public bool See(Transform target) => _range.Contains(target.position);
  }
}
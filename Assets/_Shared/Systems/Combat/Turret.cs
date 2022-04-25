using System.Linq;
using Enginooby.Core;
using Enginooby.Utils;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// REFACTOR: Extract Projector to reuse in gun,...
namespace Enginooby.Combat {
  public class Turret<TTarget, TProjectile> : MonoBehaviour
    where TTarget : Component where TProjectile : MonoBehaviour, IProjectile {
    [SerializeField] private Transform _turretPivot;
    [SerializeField] private TProjectile _projectile;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Cooldown _cooldown = new(1);
    [SerializeField] private float _rotateSpeed = 7f;
    [SerializeField] [HideLabel] private AreaCircular _range = new("Range");
    [field: SerializeReference] public int Power { get; protected set; } = 1;

    private Transform _target;

    private void Awake() {
      _range.SetGameObject(gameObject);
      if (!_turretPivot) _turretPivot = transform.FindDeepChild("Turret");
      if (!_turretPivot) _turretPivot = transform.FindDeepChild("turret");
      GetTarget();
    }

    private void GetTarget() {
      var enemies = FindObjectsOfType<TTarget>();
      if (enemies.IsNullOrEmpty()) return;
      _target = enemies.Select(e => e.gameObject).GetNearestTo(transform.position).transform;
    }

    private void Update() {
      if (!_turretPivot) return;

      // ? track persistent target vs. always get nearest target
      if (!_target || !_range.Contains(_target.position)) {
        GetTarget();
        return;
      }

      _turretPivot.TurnTo(_target.position, _rotateSpeed, false);

      if (IsLookingAtTarget() && _cooldown.IsFinished) {
        Shoot();
      }
    }

    // UTIL
    private bool IsLookingAtTarget() {
      var dirToTarget = (_target.position - transform.position).normalized;
      var dotProd = Vector3.Dot(dirToTarget, transform.forward);
      // dotProd.Log();
      return dotProd > .3f || dotProd < -.1f && dotProd > -.8f; // ? Why these numbers?
    }

    private void Shoot() {
      var projectile = Instantiate(_projectile, _firePoint.position, Quaternion.identity);
      projectile.Power = Power;
      projectile.Target = _target;
    }

    private void OnDrawGizmos() {
      _range.DrawGizmos();
    }

    private void Reset() {
      _range.SetGameObject(gameObject);
    }
  }
}
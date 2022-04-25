using UnityEngine;

namespace Enginooby.Combat {
  public interface IProjectile {
    public Transform Target { get; set; }
    public int Power { get; set; }
  }
}
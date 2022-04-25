using UnityEngine;

namespace _Shared.Scripts.Spawner {
  public class SpawnWaveTracker : MonoBehaviour {
    public Spawner spawner;

    private void OnDestroy() {
      if (!gameObject.scene.isLoaded) return;
      spawner.OnWaveSpawnedDestroy();
    }
  }
}
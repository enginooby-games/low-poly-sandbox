using THOR;
using UnityEngine;
#if ASSET_QUANTUM_CONSOLE
using QFSW.QC;
#else
using Enginooby.Attribute;
#endif

// TODO: Spawned lighting doesn't strike to the desired point on the ground

/// <summary>
/// Control when and where to spawn the lighting
/// </summary>
[RequireComponent(typeof(THOR_Thunderstorm))]
public class THOR_ThunderstormExtension : MonoBehaviour {
  // TODO: Randomize
  [SerializeField] private Vector3 _offsetToPlayer = new(0, 100, 0);
  [SerializeField] private KeyCode _spawnNearPlayerKey = KeyCode.L;
  [SerializeField] private bool _enableSpawnAtMouseClick = true;
  [SerializeField] private Vector3 _offsetToMouse = new(0, 100, 0);
  
  private GameObject _player;

  private void Start() {
    _player = GameObject.FindGameObjectWithTag("Player");
  }

  private void Update() {
   if(_spawnNearPlayerKey.IsDown()) SpawnLightingNearPlayer();
   if(_enableSpawnAtMouseClick) SpawnLightingAtMouseClick();
  }

  private void SpawnLightingAtMouseClick() {
    if (MouseButton.Left.IsDown() && RayUtils.MousePosOnRayHit != null) {
      SpawnLighting(RayUtils.MousePosOnRayHit.Value + _offsetToMouse);
    }
  }
  
  public void SpawnLighting(Vector3 pos) {
    var lastPos = THOR_Thunderstorm.instance.spawnPos;
    THOR_Thunderstorm.instance.spawnPos = pos;
    THOR_Thunderstorm.instance.ActivateLightning();
    THOR_Thunderstorm.instance.spawnPos = lastPos;
  }

  [Command("s.lighting")]
  [Enginooby.Attribute.Button]
  private void SpawnLightingNearPlayer() {
    SpawnLighting(_player.transform.position + _offsetToPlayer);
  }
}

using CompassNavigatorPro;
using UnityEngine;

// TODO: Add asset directive and move script to library

/// <summary>
/// Fix some issues of the asset.
/// </summary>
[RequireComponent(typeof(CompassPro))]
public class CompassProExtension : MonoBehaviour {
    private CompassPro _compass;

    private void Awake() {
        _compass = GetComponent<CompassPro>();
    }

    private void Start() {
        // sometimes FOW is not enabled when replay, hence force to update it every time
        _compass.UpdateFogOfWar();

        // usually we want to track the player since camera can be far away
        // hence create a dummy camera on the player for tracking
        // TODO: add option to serialize player in case not tagged
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            var trackCamera = player.AddComponent<Camera>();
            trackCamera.enabled = false;
            _compass.cameraMain = trackCamera;
        }
    }
}

using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

// ? Rename to AreaArc3D or AreaSphericalSector
/// <summary>
/// * Define area from radius & angle. Use case: vision area
/// </summary>
[Serializable]
[InlineProperty]
public class AreaCircular : SerializableBase, IArea {
  [BoxGroup("$label")] [HideLabel] public Reference origin; // ? Replace by ReferenceVector3

  [HideInInspector] public string label;

  [BoxGroup("$label")] public float Radius = 10f;

  [BoxGroup("$label")] public float Angle = 360f;

  [BoxGroup("$label")] public bool GizmosWire;

  public AreaCircular(string label = "Circular Area", float radius = 10f, float angle = 360f) {
    this.label = label;
    Radius = radius;
    Angle = angle;
  }

  public bool Contains(Vector3 pos) {
    // https://learn.unity.com/tutorial/chasing-the-player?uv=2019.4&projectId=5e0b85cdedbc2a144cf5cde5#5e0b8be8edbc2a035d135cd8
    var directionToPos = pos - origin.GameObject.transform.position;
    var angleToPos = Vector3.Angle(directionToPos, origin.GameObject.transform.forward);
    return directionToPos.magnitude < Radius && angleToPos < Angle;
  }

  public void DrawGizmos(Color? color = null) {
    if (!origin.GameObject) return;

    color ??= Color.red;
    Gizmos.color = color.Value;

    if (GizmosWire) {
      // Gizmos.DrawWireSphere(origin.GameObject.transform.position, Radius);
    }
    else {
      // Gizmos.DrawSphere(origin.GameObject.transform.position, Radius);
    }

    // TODO: draw portion of sphere based on angle
    Handles.color = color.Value;
    var transform = origin.GameObject.transform;
    Handles.DrawWireDisc(transform.position, transform.up, Radius);
    // Handles.DrawSolidArc(transform.position, transform.up, transform.position, Angle, Radius);
  }

  public override void SetGameObject(GameObject componentOwner) {
    base.SetGameObject(componentOwner);
    origin.SetGameObject(componentOwner);
    origin.GameObject = componentOwner;
  }

  // TODO: Declare overloading Contains in IArea
  public bool Contains(GameObject target) => Contains(target.transform.position);

  public bool Contains(Reference reference) => Contains(reference.GameObject);
}
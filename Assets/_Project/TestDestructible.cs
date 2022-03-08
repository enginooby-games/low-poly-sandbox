using System;
using System.Collections;
using System.Collections.Generic;
using DestroyIt;
using UnityEngine;

public class TestDestructible : MonoBehaviour {
    private Destructible _destructible;

    private void Awake() {
        _destructible = GetComponent<Destructible>();
    }
    
    private void OnTriggerEnter(Collider other) {
        _destructible.ApplyDamage(10f);
    }

    private void OnCollisionEnter(Collision collision) {
        _destructible.ApplyDamage(10f);
    }

    private void OnMouseDown() {
        _destructible.ApplyDamage(10f);
    }
}

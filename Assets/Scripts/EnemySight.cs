using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

    [SerializeField]
    private Mob mob;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            mob.target = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            mob.target = null;
        }
    }
}

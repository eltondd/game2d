using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour {

    [SerializeField]
    private Mob mob;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            mob.targetInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            mob.targetInRange = false;
        }
    }
}

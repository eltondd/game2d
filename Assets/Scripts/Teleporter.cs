using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    [SerializeField]
    private Vector3 teleportLocation;
    [SerializeField]
    private GameObject Player;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            Teleport();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Player = null;
        }
    }

    private void Teleport() {
        if (Player != null) {
            Player.transform.localPosition = teleportLocation;
        }
    }
}

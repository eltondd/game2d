using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour {
    [SerializeField]
    private float speed;

    private Rigidbody2D rigidBody;

    private Vector2 direction;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        Invoke("DestroySelf", 2f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rigidBody.velocity = direction * speed * Time.deltaTime;
	}

    public void Initialize(int dir) {
        if (dir == 0) {
            direction = Vector2.right;
        } else {
            direction = Vector2.left;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Sword" || other.tag == "PlayerFireball" || other.tag == "Fireball") {
            DestroySelf();
        }
    }

    void DestroySelf() {
        Destroy(this.gameObject);
    }
}

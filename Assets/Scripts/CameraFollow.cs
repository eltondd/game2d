using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private float xMax;
    [SerializeField]
    private float yMax;
    [SerializeField]
    private float xMin;
    [SerializeField]
    private float yMin;

    private Transform target;
    public GameObject player;

    // Use this for initialization
    void Start () {
        target = player.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector2(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax));
	}
}

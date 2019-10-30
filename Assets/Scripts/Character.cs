using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public Animator anim { get; private set; }

    [SerializeField]
    protected List<Transform> charColliders;

    [SerializeField]
    protected float movementSpeed;

    [SerializeField]
    protected bool rightDir;

    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected int health;

    [SerializeField]
    private List<string> damageSources;

    public abstract bool dead { get; }

    public bool attack;

    public GameObject fireballPrefab;

    [SerializeField]
    private EdgeCollider2D swordCollider;


	// Use this for initialization
	public virtual void Start () {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeDirection() {
        rightDir = !rightDir;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        foreach (Transform collider in charColliders) {
            if (rightDir) {
                collider.rotation = new Quaternion(0, 180f, 0, 0);
            } else {
                collider.rotation = new Quaternion(0, 0, 0, 0);
            }
            if (collider.gameObject.name == "TakeOff") {
                collider.position = new Vector3(-collider.position.x, collider.position.y);
            }
        }
    }

    public abstract IEnumerator TakeDamage();

    public virtual void MeleeAttack() {
        swordCollider.enabled = !swordCollider.enabled;
    }

    public virtual void ShootFireball() {
        if (rightDir) {
            GameObject fireball = (GameObject)Instantiate(fireballPrefab, transform.position + new Vector3(1, 0, 0), transform.rotation);
            fireball.GetComponent<Fireball>().Initialize(0);
            if (gameObject.tag == "Boss") {
                fireball.transform.position -= new Vector3(0, 1, 0);
            }
        } else {
            GameObject fireball = (GameObject)Instantiate(fireballPrefab, transform.position - new Vector3(1, 0, 0), transform.rotation);
            fireball.GetComponent<Fireball>().Initialize(1);
            fireball.GetComponent<SpriteRenderer>().flipX = true;
            if (gameObject.tag == "Boss") {
                fireball.transform.position -= new Vector3(0, 1, 0);
            }
        }

    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        if (damageSources.Contains(other.tag)) {
            StartCoroutine(TakeDamage());
        }
    }
}

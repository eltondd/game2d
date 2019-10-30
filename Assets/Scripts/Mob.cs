using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Character {

    private EnemyInterface currentState;
    public bool targetInRange;

    public GameObject target;
    [SerializeField]
    private bool immortal = false;

    private Rigidbody2D rigidBody;

    [SerializeField]
    private float immortalTime;

    [SerializeField]
    private GameObject teleporter;

    private float attackTimer = 0;
    private float attackCooldown = 3f;
    private bool canMove = true;

    // Use this for initialization
    public override void Start () {
        base.Start();
        ChangeState(new IdleState());
        rigidBody = GetComponent<Rigidbody2D>();
        if (teleporter != null) {
            teleporter.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!dead) {
            attackTimer += Time.deltaTime;
            currentState.Execute();
            LookAtTarget();
        }
	}

    public override bool dead {
        get {
            return health <= 0;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public void Move() {
        if (!attack) {
            anim.SetFloat("Speed", 1);
            transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
        }
    }

    public Vector2 GetDirection() {
        if (rightDir) {
            return Vector2.right;
        } else {
            return Vector2.left;
        }
    }

    private void LookAtTarget() {
        if (target != null) {
            float xDir = target.transform.position.x - transform.position.x;

            if (xDir < 0 && rightDir || xDir > 0 && !rightDir) {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    ChangeDirection();
            }
        }
    }

    public void Attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && attackTimer > attackCooldown) {
            attackTimer = 0;
            attack = true;
            anim.SetTrigger("Attack");
            if (gameObject.name == "DragonKnight") {
                StartCoroutine(Charge());
            }
            Invoke("resetValues", 1f);
        }
    }

    public override void ShootFireball() {
        if (attackTimer > attackCooldown) {
            attack = true;
            anim.SetTrigger("Fire");
            base.ShootFireball();
            attackTimer = 0;
            Invoke("resetValues", 1f);
        }
        
    }

    public void ChangeState(EnemyInterface newState) {
        if (currentState != null) {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    private IEnumerator Charge() {
        Vector2 nextPos;
        if (rightDir) {
            if (gameObject.transform.position.x + 6 < 13) {
                nextPos = new Vector2(gameObject.transform.position.x + 6, 0);
            } else {
                nextPos = new Vector2(12.5f, 0);
            }            
        } else {
            if (gameObject.transform.position.x - 6 > -7) {
                nextPos = new Vector2(gameObject.transform.position.x - 6, 0);
            } else {
                nextPos = new Vector2(-7, 0);
            } 
        }
        yield return new WaitForSeconds(.4f);
        rigidBody.MovePosition(nextPos);
    }

    private IEnumerator IndicateImmortal() {
        while (immortal) {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override IEnumerator TakeDamage() {
        if (!immortal) {
            health--;
            if (!dead) {
                anim.SetTrigger("Hurt");
                immortal = true;
                ChangeState(new IdleState());
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                rigidBody.velocity = Vector2.zero; 
                immortal = false;
            } else {
                anim.SetTrigger("Die");
                Invoke("DestroySelf", 1.5f);
                teleporter.SetActive(true);
                yield return null;
            }
        }
    }

    private void DestroySelf() {
        Destroy(this.gameObject);
    }

    private void resetValues() {
        attack = false;
    }

}

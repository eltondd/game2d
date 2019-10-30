using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    private float fallMultiplier = 2.0f;
    private float lowJumpMultiplier = 1.5f;

    private Rigidbody2D rigidBody;

    // Check if on Ground
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask ground;

    // Jump Mechanics
    private float horizontalMovement;
    private bool onGround;
    private bool jump;
    [SerializeField]
    private float jumpForce;

    private bool immortal = false;
    [SerializeField]
    private float immortalTime;

    [SerializeField]
    private bool onPortal = false;

    public GameObject mainMenu;
    public GameObject gameWin;
    public GameObject gameOver;
    public Transform HealthBar;

    private float fireballTimer = 0;
    private float fireballCooldown = 0.5f;

    private bool dodge;
    private float dodgeTimer = 0;
    private float dodgeCooldown = 1f;
    private float dodgeSpeedMultiplier = 2f;

    private CapsuleCollider2D trigger;

    // Use this for initialization
    public override void Start () {
        base.Start();
        movementSpeed = 5f;
        rigidBody = GetComponent<Rigidbody2D>();
        ChangeDirection();
        health = Game.current.playerHP;
        trigger = GetComponent<CapsuleCollider2D>();
    }

    void Update() {
        if (!dead) {
            Controls();
            updateHPBar();
            dodgeTimer += Time.deltaTime;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!dead) {
            if (!dodge) {
                horizontalMovement = Input.GetAxis("Horizontal");
            }           
            onGround = onGroundFunct();
            HandleControls(horizontalMovement);
            Flip(horizontalMovement);
            resetConditions();

            if (rigidBody.velocity.y < 0) {
                rigidBody.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            } else if (rigidBody.velocity.y < 0 && !Input.GetKey(KeyCode.Space)) {
                rigidBody.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void updateHPBar() { 
        for (int i = 0; i < 5; i++) {
            if (i < health) {
                HealthBar.GetChild(i).gameObject.SetActive(true);
            } else {
                HealthBar.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public override bool dead {
        get {
            return health <= 0;
        }
    }
    private void Controls() {
        if (Input.GetMouseButtonDown(0)) {
            attack = true;
            print("Attacking");
        }

        //if (Input.GetMouseButtonDown(1)) {
        //    ShootFireball();    
        //}

        if (Input.GetKeyDown(KeyCode.Space)) {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dodgeTimer > dodgeCooldown) {
            Dodge();
        }

        if(Input.GetKeyDown(KeyCode.W)) {
            if (Game.current.stage != 4) {
                EnterNextStage();
            } else {
                gameWin.SetActive(true);
                Time.timeScale = 0;
            }
            
        }
    }

    private void HandleControls(float horizontal) {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            rigidBody.velocity = new Vector2(horizontal * movementSpeed, rigidBody.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }

        if (attack && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && onGround) {
            anim.SetTrigger("Attack");
            rigidBody.velocity = Vector2.zero;
        }

        if (onGround && jump) {
            jump = false;
            rigidBody.AddForce(new Vector2(0, jumpForce));
            anim.SetBool("Jump", true);
        }

        if (attack && anim.GetCurrentAnimatorStateInfo(0).IsName("SNinja_jump")) {
            anim.SetTrigger("JumpAtk");
        }
    }

    private void Flip(float horizontal) {
        if (horizontal < 0 && !spriteRenderer.flipX || horizontal > 0 && spriteRenderer.flipX) {
            ChangeDirection();
        }
    }

    private void resetConditions() {
        attack = false;
        jump = false;
    }

    private void resetDodge() {
        immortal = false;
        dodge = false;
    }

    public override void ShootFireball() {
        if (fireballTimer > fireballCooldown) {
            base.ShootFireball();
        }
        
    }

    private bool onGroundFunct() { 
        if (rigidBody.velocity.y < .1) {
            foreach (Transform point in groundPoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, ground);
                
                for (int i = 0; i < colliders.Length; i++) {
                    if (colliders[i].gameObject != gameObject) {
                        anim.SetBool("Jump", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public Vector2 GetDirection() {
        if (rightDir) {
            return Vector2.right;
        } else {
            return Vector2.left;
        }
    }

    private void Dodge() {
        anim.SetTrigger("Slide");
        rigidBody.velocity += GetDirection() * dodgeSpeedMultiplier;
        dodgeTimer = 0;
        immortal = true;
        Invoke("resetDodge", 0.4f);
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
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                rigidBody.velocity = Vector2.zero;
                immortal = false;
            } else {
                Time.timeScale = 0;
                anim.SetTrigger("Die");
                gameOver.SetActive(true);
                yield return null;
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        if (!dodge) {
            base.OnTriggerEnter2D(other);
        }
        
        if (other.tag == "Portal") {
            onPortal = true;
        }
        if (other.tag == "Death") {
            health = 0;
            StartCoroutine(TakeDamage());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Portal") {
            onPortal = false;
        }
    }

    public void EnterNextStage() {
        if (onPortal) {
            Game.current.playerHP = health;
            print(Game.current.playerHP);
            mainMenu.GetComponent<MainMenu>().NextStage();
        }
    }

    private void GameOver() {
        gameOver.SetActive(true);
    }
}

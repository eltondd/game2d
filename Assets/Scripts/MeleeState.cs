using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : EnemyInterface {

    private Mob mob;
    private float AttackTimer = 3f;
    private float AttackCooldown = 3f;

    public void Enter(Mob mob) {
        this.mob = mob;
    }

    public void Execute() {
        AttackTimer += Time.deltaTime;
        if (AttackTimer >= AttackCooldown) {
            mob.Attack();
            AttackTimer = 0;
        }

        if (!mob.targetInRange && !mob.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            mob.ChangeState(new MoveState());
        }
        
    }

    public void Exit() {
        
    }

    public void OnTriggerEnter(Collider2D other) {
        if (other.tag == "Edge") {
            mob.ChangeDirection();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyInterface {

    private Mob mob;

    private float idleTimer;

    private float maxIdleTime = 3f;

    public void Enter(Mob mob) {
        this.mob = mob;
    }

    public void Execute() {
        Idle();

        if (mob.target != null) {
            mob.ChangeState(new MoveState());
        }

        if (mob.targetInRange) {
            mob.ChangeState(new MeleeState());
        }
    }

    public void Exit() {
        
    }

    public void OnTriggerEnter(Collider2D other) {
        
    }

    private void Idle() {
        mob.anim.SetFloat("Speed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer > maxIdleTime) {
            mob.ChangeState(new MoveState());
        }
    }
}

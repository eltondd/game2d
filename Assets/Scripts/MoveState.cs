using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : EnemyInterface {

    private Mob mob;
    private float patrolTime;
    private float patrolDuration = 7f;

    public void Enter(Mob mob) {
        this.mob = mob;
    }

    public void Execute() {
        patrolTime += Time.deltaTime;
        if (patrolTime > patrolDuration) {
            mob.ChangeState(new IdleState());
        }

        mob.Move();

        if (mob.target != null) {
            mob.ChangeState(new RangeState());
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

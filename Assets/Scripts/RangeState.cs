using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeState : EnemyInterface {

    private Mob mob;

    public void Enter(Mob mob) {
        this.mob = mob;
    }

    public void Execute() {
        if (mob.target != null) {
            if (mob.gameObject.name == "DragonKnight") {
                mob.ShootFireball();
            }         
            mob.Move();
        } else {
            mob.ChangeState(new MoveState());
        }

        if ((mob.gameObject.name == "DragonKnight" && Game.current.stage > 4) || mob.gameObject.name != "DragonKnight") {
            if (mob.targetInRange) {
                mob.ChangeState(new IdleState());
            }
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

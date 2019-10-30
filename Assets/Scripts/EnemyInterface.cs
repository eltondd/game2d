using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyInterface {

    void Enter(Mob mob);
    void Execute();  
    void Exit();
    void OnTriggerEnter(Collider2D other);
	
}

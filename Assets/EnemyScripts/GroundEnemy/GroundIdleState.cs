using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundIdleState : GroundEnemyState
{
    private float time;

    public override void OnStateEnter(GroundEnemySC enemy)
    {
        //reset to start watching for bullet
        enemy.canDodge = false;
    }

    public override void Act(GroundEnemySC enemy)
    {
        //if bullet is heading towards enemy
        if (enemy.canDodge == true)
        {
            //create random chance of dodging based on dodge chance %
            int rand = Random.Range(1, 101);
            if (rand <= enemy.dodgeChance)
            {
                enemy.SetState(new GroundDodgeState());
            }
            else
            {
                //if chance failed, don't dodge for this bullet
                enemy.canDodge = false;
            }
        }

        time += Time.deltaTime;

        if (time >= enemy.waitTime)
        {
            time = 0;

            //check if the enemy is behind cover
            if (Physics.Linecast(enemy.transform.position, Camera.main.transform.position, enemy.castLayers))
            {
                enemy.SetState(new GroundUncoverState());
            }
            else
            {
                //random chance of shooting when done waiting
                int rand = Random.Range(1, 100);
                //also make sure the enemy is visible when it shoots
                if (rand <= enemy.chanceToAttack)
                {
                    enemy.Shoot();
                }

                enemy.SetState(new GroundCoverState());
            }
        }
    }
}

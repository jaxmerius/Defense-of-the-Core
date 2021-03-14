using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundDodgeState : GroundEnemyState
{
    public Vector3 newPos;
    private float time;

    public override void OnStateEnter(GroundEnemySC enemy)
    {
        for (int i = 0; i < 100; i++)
        {
            //choose a random point in range
            Vector3 pos = (Random.insideUnitSphere * enemy.moveRange) + enemy.startPos;
            //make the point at the same height as the enemy
            pos.y = enemy.transform.position.y;

            //check if there will be an object between the enemy and player and no object between position and new position
            if (Physics.Linecast(pos, Camera.main.transform.position, enemy.castLayers) && !Physics.Linecast(enemy.transform.position, pos, enemy.castLayers))
            {
                newPos = pos;
                return;
            }
        }

    }

    public override void Act(GroundEnemySC enemy)
    {
        time += Time.deltaTime;

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, newPos, enemy.dodgeSpeed * Time.deltaTime);
        if (Vector3.Distance(enemy.transform.position, newPos) <= enemy.agent.stoppingDistance)
        {
            newPos = Vector3.zero;
            enemy.SetState(new GroundIdleState());
        }
    }
}

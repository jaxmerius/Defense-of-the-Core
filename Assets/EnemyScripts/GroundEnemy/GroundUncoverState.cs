using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundUncoverState : GroundEnemyState
{
    public Vector3 newPos;
    private float time;

    public override void OnStateEnter(GroundEnemySC enemy)
    {
        for (int i = 0; i < 100; i++)
        {
            //choose a random point in range
            Vector3 pos = (Random.insideUnitSphere * enemy.moveRange) + enemy.startPos;
            pos.y = enemy.transform.position.y;

            //check if there will not be an object between the enemy and player and no objects at point
            if (!Physics.Linecast(pos, Camera.main.transform.position, enemy.castLayers))
            {
                //check if the navmesh agent can reach position without getting stuck
                NavMeshPath path = new NavMeshPath();
                if (enemy.agent.CalculatePath(pos, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    newPos = pos;
                    enemy.MoveToLocation(newPos);
                    return;
                }
            }
        }
    }

    public override void Act(GroundEnemySC enemy)
    {
        time += Time.deltaTime;

        if(time > enemy.navigationTimeout || enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
        {
            enemy.agent.isStopped = true;
            enemy.SetState(new GroundIdleState());
        }
    }
}

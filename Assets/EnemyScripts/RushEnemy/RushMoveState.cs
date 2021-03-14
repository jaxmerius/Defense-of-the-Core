using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RushMoveState : RushEnemyState
{
    public Vector3 newPos;
    private float time;
    public float hoverTime = 0f; 

    public override void OnStateEnter(RushEnemySC enemy)
    {
        //set navmesh agent's destination as the player
        enemy.agent.destination = Camera.main.transform.position;
        hoverTime = 0f; 
    }

    public override void Act(RushEnemySC enemy)
    { //every 240 frames, dodge 
        hoverTime++; 

        if (hoverTime >= 240f)
        {
           
           enemy.SetState(new RushEnemyDodge());
            
        }

        time += Time.deltaTime;

        //if close enough to the player, explode
        if(Vector3.Distance(enemy.agent.transform.position, Camera.main.transform.position) <1f)
        {
           enemy.agent.isStopped = true;
            enemy.SetState(new RushAttackState());
        }
    }
}

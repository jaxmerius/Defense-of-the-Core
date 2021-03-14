using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingUncoverState : FlyingEnemyState
{
    public Vector3 newPos;

    public override void OnStateEnter(FlyingEnemySC enemy)
    {
        enemy.PlaySound(enemy.hoverSound);
        for (int i = 0; i < 300; i++)
        {
            //choose a random point in range
            Vector3 pos = (Random.insideUnitSphere * enemy.moveRange) + enemy.startPos;

            //check if there will not be an object between the enemy and player, that the enemy's new position won't be inside another object, and there is no object betweent he enemy and new position
            if (!Physics.Linecast(pos, Camera.main.transform.position, enemy.castLayers) && !Physics.CheckSphere(pos, enemy.GetComponent<SphereCollider>().radius) && !Physics.Linecast(enemy.transform.position, pos, enemy.castLayers))
            {
                newPos = pos;
                return;
            }
        }
    }

    public override void Act(FlyingEnemySC enemy)
    {
        if(newPos == Vector3.zero)
        {
            //kill enemy if no new position could be found because it's stuck
            Debug.Log("Could not find new location without cover", enemy.gameObject);
            enemy.KillEnemy(enemy.gameObject);

        }
        else
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, newPos, enemy.speed * Time.deltaTime);
            if (enemy.transform.position == newPos)
            {
                newPos = Vector3.zero;
                enemy.SetState(new FlyingIdleState());
            }
        }

    }
}

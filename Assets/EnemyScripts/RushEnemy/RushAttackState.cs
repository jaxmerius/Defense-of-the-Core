using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RushAttackState : RushEnemyState
{
    public Vector3 newPos;
    private float time;
    private MonoBehaviour mb;

    public override void OnStateEnter(RushEnemySC enemy)
    {
        //don't move when exploding
        enemy.agent.isStopped = true;
        enemy.PlaySound(enemy.attackSound);
    }

    public override void Act(RushEnemySC enemy)
    {
        enemy.transform.localScale += new Vector3(enemy.explodeScale, enemy.explodeScale, enemy.explodeScale);

        time += Time.deltaTime;

        if(time >= enemy.explodeTime)
        {
            //find all colliders within explosion range (using spherecollider's radius)
            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, enemy.GetComponent<SphereCollider>().radius);

            foreach(Collider col in colliders)
            {
                if(col.tag == "Player")
                {
                    //set player's damage screen to active
                    col.transform.GetChild(0).gameObject.SetActive(true);

                    //scale damage dealt based on distance from enemy, so further enemy = less damage taken
                    float damage = (Vector3.Distance(enemy.transform.position, Camera.main.transform.position) - enemy.agent.stoppingDistance) / enemy.GetComponent<SphereCollider>().radius;
                    damage = (1 - damage) * enemy.damage;

                    //show health damage
                    HealthManager.instance.SubtractHealth(damage);
                }

                if(col.tag == "Enemy")
                {
                    //kill any enemies in range of explosion
                    enemy.KillEnemy(col.gameObject);
                }
            }
            //destroy the enemy
            enemy.KillEnemy(enemy.gameObject);
        }
    }
}

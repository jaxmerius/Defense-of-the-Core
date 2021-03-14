using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Tooltip("Speed the bullets move")]
    public float speed = 20;
    public float damage = 10;

    private Vector3 start;

    private void Start()
    {
        //get starting position at start
        start = transform.position;
    }

    void FixedUpdate()
    {
        //move the bullet forward at the given speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //if the bullet goes too far, destroy it
        if(Vector3.Distance(transform.position, start) > 50)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float value)
    {
        damage = value; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            //determine if hit enemy or player
            if(gameObject.tag == "PlayerBullet" && (other.tag == "Enemy" || other.tag =="RushEnemy"))
            {
                //destroy the enemy if not rusher
                other.GetComponent<Score>().health--;
                //destroy the bullet
                Destroy(gameObject);
            }
            else if(gameObject.tag == "EnemyBullet" && other.tag == "Player")
            {
                //set player's damage screen to active
                other.transform.GetChild(0).gameObject.SetActive(true);
                //show health damage
                HealthManager.instance.SubtractHealth(damage);
                //destroy the bullet
                Destroy(gameObject);
            }

            //destroy the bullet if any environment object is hit
            if(other.tag == "Environment")
            {
                Destroy(gameObject);
            }

        }
    }
}

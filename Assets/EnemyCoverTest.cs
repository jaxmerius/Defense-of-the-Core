using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverTest : MonoBehaviour
{
    [Tooltip("% chance the enemy will attack after waiting")]
    public int chanceToAttack = 15;
    [Tooltip("how far away the enemy can move from its start position")]
    public float moveRange = 5;
    [Tooltip("how long the enemy waits before moving")]
    public float waitTime = 0.8f;
    [Tooltip("how fast the enemy moves between positions")]
    public float speed = 10;
    [Tooltip("which layers the enemy should look at when determining if there is something in the way of movement")]
    public LayerMask castLayers;
    [Tooltip("particle prefab for when the enemy dies")]
    public GameObject enemyParticle;
    [Tooltip("prefab for the enemy's bullets")]
    public GameObject enemyShot;


    //temporary state of the enemy
    private string state = "move";
    private float time;
    private Vector3 startPos;
    private Vector3 newPos;
    private bool isQuit;

    private void Start()
    {
        startPos = transform.position;

        //find a position if starting state is move
        if(state == "move")
        {
            newPos = FindNewPosition();
        }
    }
    void Update()
    {
        //these states are temporary and will be changed to real states later
        if (state == "wait")
        {
            time += Time.deltaTime;

            if (time >= waitTime)
            {
                //25% chance of shooting when done waiting
                int rand = Random.Range(1, 100);
                if(rand <= chanceToAttack)
                {
                    GameObject go = Instantiate(enemyShot);
                    go.transform.position = transform.position;
                    go.transform.LookAt(Camera.main.transform.position);
                }

                newPos = FindNewPosition();
            }
        }

        if (state == "move")
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
            if (transform.position == newPos)
            {
                newPos = Vector3.zero;
                state = "wait";
            }
        }
    }

    private Vector3 FindNewPosition()
    {
        time = 0;


        for (int i = 0; i < 100; i++)
        {
            //choose a random point in range
            Vector3 pos = (Random.insideUnitSphere * moveRange) + startPos;
            //first, find if there is an object between the enemy and player to determine if it is under cover (cover and uncovered will probably be two different states later)
            //then check if the new position would be the opposite (e.g. if it wasn't behind cover, see if the new position would be)
            //next, make sure the enemy wouldn't be partially inside an object (might change this to use the collider instead of number)
            //Then, make sure there is nothing between the enemy's current position and the new position
            if (!Physics.Linecast(transform.position, Camera.main.transform.position, castLayers) && Physics.Linecast(pos, Camera.main.transform.position, castLayers) && !Physics.CheckSphere(pos, GetComponent<SphereCollider>().radius) && !Physics.Linecast(transform.position, pos, castLayers))
            {
                state = "move";
                return pos;
            }
            else if (Physics.Linecast(transform.position, Camera.main.transform.position, castLayers) && !Physics.Linecast(pos, Camera.main.transform.position, castLayers) && !Physics.CheckSphere(pos, GetComponent<SphereCollider>().radius) && !Physics.Linecast(transform.position, pos, castLayers))
            {
                state = "move";
                return pos;
            }
        }

        //return zero if no new position is found after 100 tries
        Debug.Log("Could not find new location to move", gameObject);
        return Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Color c = Color.red;
        c.a = 0.5f;
        Gizmos.color = c;
        if (Application.isPlaying)
        {
            Gizmos.DrawSphere(startPos, moveRange);
        }
        else
        {
            Gizmos.DrawSphere(transform.position, moveRange);
        }

    }

    private void OnDestroy()
    {
        if(!isQuit)
        {
            GameObject go = Instantiate(enemyParticle);
            go.transform.position = transform.position;
        }
    }

    //prevents error when returning to editor
    private void OnApplicationQuit()
    {
        isQuit = true;
    }
}

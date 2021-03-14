using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemySC : MonoBehaviour
{
    [Tooltip("% chance the enemy will attack after waiting")]
    public int chanceToAttack = 15;
    [Tooltip("how long the enemy waits before moving")]
    public float waitTime = 0.8f;
    [Tooltip("how fast the enemy moves, overwrites the navmesh agent speed")]
    public float speed = 1;
    [Tooltip("chance the enemy will try to dodge if a shot is moving towards it")]
    public float dodgeChance = 10;
    [Tooltip("how fast the enemy moves between positions during dodge")]
    public float dodgeSpeed = 20;
    [Tooltip("which layers the enemy should look at when determining if there is something in the way of movement")]
    public LayerMask castLayers;
    [Tooltip("particle prefab for when the enemy dies")]
    public GameObject enemyParticle;
    [Tooltip("prefab for the enemy's bullets")]
    public GameObject enemyShot;
    [Tooltip("how long the enemy will try to get to its destination before timing out")]
    public float navigationTimeout;

    public float damage = 10;
    public NavMeshAgent agent;

    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public bool canDodge;
    private bool isQuit;

    //how far away the enemy can move from its start position
    [HideInInspector]
    public float moveRange = 5;

    public GroundEnemyState currentState;

    public AudioClip talkSound;
    public AudioClip shootSound;
    private AudioSource soundPlayer;

    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        startPos = transform.parent.GetComponent<EnemySpawner>().startPos;
        //get the range from the spawner at start
        if (transform.parent)
        {
            moveRange = transform.parent.GetComponent<EnemySpawner>().range;
        }
        agent.speed = speed;
        SetState(new GroundCoverState());
    }

    void FixedUpdate()
    {
        //destroy enemy if health reaches 0
        if (GetComponent<Score>().health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            currentState.Act(this);
        }
    }

    public void SetState(GroundEnemyState state)
    {
        if (currentState != null)
        {
            currentState.OnStateExit(this);
        }
        currentState = state;
        if (currentState != null)
        {
            currentState.OnStateEnter(this);
        }
    }

    public void Shoot()
    {
        GameObject go = Instantiate(enemyShot);
        go.GetComponent<BulletScript>().SetDamage(damage);
        go.transform.position = transform.position;
        go.transform.LookAt(Camera.main.transform.position);
        PlaySound(shootSound);
    }

    public void MoveToLocation(Vector3 target)
    {
        agent.SetDestination(target);
        agent.isStopped = false;
        agent.speed = speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if a player bullet enters trigger radius and is moving towards the enemy, set that it can dodge
        if (other.tag == "PlayerBullet")
        {
            RaycastHit hit;
            if (Physics.Raycast(other.transform.position, other.transform.forward, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    canDodge = true;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (!isQuit)
        {
            //add to score 
            ScoreManager.instance.AddScore(GetComponent<Score>().score);
            GameObject go = Instantiate(enemyParticle);
            go.transform.position = transform.position;
        }
    }

    //prevents error when returning to editor
    private void OnApplicationQuit()
    {
        isQuit = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(agent.destination, GetComponent<CapsuleCollider>().radius);
    }

    public void PlaySound(AudioClip audio)
    {
        soundPlayer.pitch = Random.Range(0.9f, 1.1f);
        soundPlayer.clip = audio;
        soundPlayer.Play();
    }
}

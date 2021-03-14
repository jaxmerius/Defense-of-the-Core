using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RushEnemySC : MonoBehaviour
{
    [Tooltip("how fast the enemy moves, overwrites the navmesh agent speed")]
    public float speed = 1;
    [Tooltip("how long the enemy takes before exploding")]
    public float explodeTime;
    [Tooltip("how much the enemy grows before exploding")]
    public float explodeScale;
    [Tooltip("which layers the enemy should look at when determining if there is something in the way of movement")]
    public LayerMask castLayers;
    [Tooltip("particle prefab for when the enemy dies")]
    public GameObject enemyParticle;

    public float damage = 10; 
    public NavMeshAgent agent;

    [HideInInspector]
    public Vector3 startPos;
    private bool isQuit;

    //how far away the enemy can move from its start position
    [HideInInspector]
    public float moveRange = 5;

    public RushEnemyState currentState;

    public AudioClip attackSound;
    public AudioClip talkSound;
    private AudioSource soundPlayer;

    //public ExplosionSound explosionSound;

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
        SetState(new RushMoveState());
    }

    void Update()
    {
        if(GetComponent<Score>().health <= 0 && currentState is RushAttackState == false)
        {
            SetState(new RushAttackState());
        }

        currentState.Act(this);
    }

    public void SetState(RushEnemyState state)
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

    public void MoveToLocation(Vector3 target)
    {
        agent.SetDestination(target);
        agent.isStopped = false;
    }

    public void KillEnemy(GameObject go)
    {
        Destroy(go);
    }

    private void OnDestroy()
    {
        if (!isQuit)
        {
            print("Explode");
            //add to score 
            ScoreManager.instance.AddScore(GetComponent<Score>().score);
            GameObject go = Instantiate(enemyParticle);
            go.transform.position = transform.position;
            go.transform.localScale = new Vector3(2f, 2f, 2f);
        }
    }

    //prevents error when returning to editor
    private void OnApplicationQuit()
    {
        isQuit = true;
    }

    public void PlaySound(AudioClip audio)
    {
        soundPlayer.pitch = Random.Range(0.9f, 1.1f);
        soundPlayer.clip = audio;
        soundPlayer.Play();
    }
}

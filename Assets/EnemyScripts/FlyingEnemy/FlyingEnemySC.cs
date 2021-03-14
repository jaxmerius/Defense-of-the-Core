using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemySC : MonoBehaviour
{
    [Tooltip("% chance the enemy will attack after waiting")]
    public int chanceToAttack = 15;
    [Tooltip("how long the enemy waits before moving")]
    public float waitTime = 0.8f;
    [Tooltip("how fast the enemy moves between positions")]
    public float speed = 10;
    [Tooltip("chance the enemy will try to dodge if a shot is moving towards it")]
    public float dodgeChance = 10;
    [Tooltip("how fast the enemy moves between positions during dodge")]
    public float dodgeSpeed = 20;
    [Tooltip("how many positions the enemy will move to when dodging")]
    public float dodgeCount = 10;
    [Tooltip("which layers the enemy should look at when determining if there is something in the way of movement")]
    public LayerMask castLayers;
    [Tooltip("particle prefab for when the enemy dies")]
    public GameObject enemyParticle;
    [Tooltip("prefab for the enemy's bullets")]
    public GameObject enemyShot;
    [Tooltip("how much damage the enemy does")]
    public float damage = 10;

    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public bool canDodge;
    private bool isQuit;

    //how far away the enemy can move from its start position
    [HideInInspector]
    public float moveRange = 5;

    public FlyingEnemyState currentState;

    public AudioClip hoverSound;
    public AudioClip attackSound;

    private AudioSource soundPlayer;

    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();

        startPos = transform.parent.GetComponent<EnemySpawner>().startPos;

        //get the range from the spawner at start
        moveRange = transform.parent.GetComponent<EnemySpawner>().range;
        SetState(new FlyingCoverState());
    }

    void Update()
    {
        if (GetComponent<Score>().health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            currentState.Act(this);
            transform.LookAt(Camera.main.transform.position);
        }

        // currentState.Act(this);
        // transform.LookAt(Camera.main.transform.position);
    }

    public void SetState(FlyingEnemyState state)
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
        PlaySound(attackSound);
    }

    public void KillEnemy(GameObject go)
    {
        Destroy(go);
    }

    private void OnTriggerEnter(Collider other)
    {
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
        //PlaySound(deathSound);
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

    public void PlaySound(AudioClip audio)
    {
        soundPlayer.pitch = Random.Range(0.9f, 1.1f);
        soundPlayer.clip = audio;
        soundPlayer.Play();
    }
}

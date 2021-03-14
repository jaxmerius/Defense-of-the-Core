using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("prefab for the enemy that will be spawned")]
    public GameObject enemyPrefab;
    [Tooltip("time it takes between spawning enemies")]
    public float timeToSpawn = 1.65f;
    [Tooltip("how far enemies spawned by this spawner can move away")]
    public float range = 5;
    [Tooltip("maximum number of enemies that can be alive at once")]
    public int maxEnemies = 7;
    [Tooltip("total enemies the spawner will spawn, 0 = infinite")]
    public int totalEnemies = 0;
    [Tooltip("whether to spawn an enemy immediately when the spawner is loaded")]
    public bool spawnEnemyAtStart = true;

    [HideInInspector]
    public int enemiesSpawned;
    private float time;
    [HideInInspector]
    public Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;

        if(spawnEnemyAtStart)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
        //spawn an enemy if the max number of enemies are not alive and the total number of enemies have not been met (if not infinite)
        if(transform.childCount < maxEnemies && enemiesSpawned < totalEnemies || transform.childCount < maxEnemies && totalEnemies == 0)
        {
            time += Time.deltaTime;
            if(time >= timeToSpawn)
            {
                SpawnEnemy();
                time = 0;
            }
        }
    }

    void SpawnEnemy()
    {
        //try 100 times to spawn an enemy
        for(int i = 0; i < 100; i++)
        {
            //get a random point in the spawner's range
            Vector3 rand = (Random.insideUnitSphere * range) + transform.position;
            //add to y for raycast down
            rand.y += 50;
            RaycastHit hit;
            //raycast down to check for ground
            if(Physics.Raycast(rand, Vector3.down, out hit, 1 << LayerMask.NameToLayer("Environment")))
            {
                //raycast between spawn point and camera to see if the enemy will spawn behind cover and not inside an object
                if (Physics.Linecast(hit.point, Camera.main.transform.position, 1 << LayerMask.NameToLayer("Cover")) && !Physics.CheckSphere(hit.point, 1, 1 << LayerMask.NameToLayer("Cover")))
                {
                    //spawn the enemy and stop the loop
                    enemiesSpawned++;
                    GameObject go = Instantiate(enemyPrefab, hit.point, enemyPrefab.transform.rotation, transform);
                    //make sure flying enemies don't spawn in the ground
                    if(enemyPrefab.GetComponent<FlyingEnemySC>())
                    {
                        go.transform.Translate(new Vector3(0, 1, 0));
                    }
                    return;
                }
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Color c = Color.red;
        c.a = 0.5f;
        Gizmos.color = c;
        if (Application.isPlaying)
        {
            Gizmos.DrawSphere(startPos, range);
        }
        else
        {
            Gizmos.DrawSphere(transform.position, range);
        }

    }
}

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnGo;
    public GameObject target;

    public List<Transform> spawnPoints;

    public float SpawnDelay = 5;
    float spawnTimer = 0;
    public float difficultyIncreaseMod = 0.025f;


    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > SpawnDelay)
        {
            spawnTimer = 0;
            SpawnAtRandomPoint();
        }


        
        SpawnDelay -= Time.deltaTime * difficultyIncreaseMod;
        SpawnDelay = Mathf.Clamp(SpawnDelay, 0.5f, 5);
    }



    void SpawnAtRandomPoint()
    {
        Debug.Log("Spawns: " + spawnPoints.Count);

        Transform chosenSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject newSpawn = Instantiate(spawnGo, chosenSpawn.position, Quaternion.identity);
        newSpawn.GetComponent<EnemyBehavior>().target = target;
    }
}

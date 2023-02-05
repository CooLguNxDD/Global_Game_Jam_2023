using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] SpawnList;
    public GameObject enemyGroup;

    public int spawnRate;
    private bool isSpawning;
    private void Start()
    {
        enemyGroup = GameObject.Find("EnemySpawnGroup");

    }

    private void FixedUpdate()
    {
        if (!isSpawning)
        {
            StartCoroutine(Spawner());
        }
    }

    IEnumerator Spawner()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnRate);
        Spawn();
        isSpawning = false;
    }

    // Start is called before the first frame update
    void Spawn()
    {
        GameObject obj;
        int random = Random.Range(0, SpawnList.Length);
        var position = transform.position;
        
        Vector3 pos = new Vector3(position.x + Random.Range(-5, 5),
            position.y + Random.Range(-5, 5), position.z);
        
        obj = Instantiate(SpawnList[random], pos, Quaternion.identity);
        obj.transform.SetParent(enemyGroup.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

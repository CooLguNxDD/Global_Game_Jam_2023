using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] SpawnList;
    public GameObject enemyGroup;

    public CircleCollider2D Collider2D;

    public float spawnRate;
    private bool isSpawning;
    private void Start()
    {
        StartCoroutine(Spawner());
        enemyGroup = GameObject.Find("EnemySpawnGroup");

    }

    IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            spawnRate = Random.Range(5f, 15f);
            Spawn();
        }
    }

    // Start is called before the first frame update
    void Spawn()
    {
        GameObject obj;
        int random = Random.Range(0, SpawnList.Length);
        var position = transform.position;
        
        Vector3 pos = new Vector3(position.x + Random.Range(-2f, 2f),
            position.y + Random.Range(-2f, 2f), position.z);
        
        obj = Instantiate(SpawnList[random], pos, Quaternion.identity);
        obj.transform.SetParent(enemyGroup.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("this is: "+other.name);
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    
}

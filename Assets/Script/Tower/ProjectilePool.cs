using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // Start is called before the first frame update

    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    
    public static ProjectilePool Instance;

    [System.Serializable]
    public class Pool
    {
        public string new_tag;
        public GameObject prefeb;
        public int size;
    }

    public List<Pool> Pools;
    void Start()
    
    {
        Instance = this;
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject gameObject = Instantiate(pool.prefeb, transform, true);
                gameObject.SetActive(false);
                objectPool.Enqueue(gameObject);
            }
            // Debug.Log("tag: "+pool.new_tag);
            PoolDictionary.Add(pool.new_tag, objectPool);
            // Debug.Log(PoolDictionary.ContainsKey("bullet"));
        }
    }

    public GameObject SpawnFromPool(string target_tag, Vector3 position, Quaternion rotation)
    {
        // Debug.Log(target_tag);

        GameObject spawnObj = PoolDictionary["bullet"].Dequeue();
        spawnObj.SetActive(true);
        spawnObj.transform.position = position;
        spawnObj.transform.rotation = rotation;
        
        PoolDictionary["bullet"].Enqueue(spawnObj);
        return spawnObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

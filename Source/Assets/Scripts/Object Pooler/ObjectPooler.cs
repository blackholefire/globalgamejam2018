using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public DificultyController dificulty;

	// Use this for initialization
	void Start () {
        dificulty = GameObject.Find("DifficultyController").GetComponent<DificultyController>();

        StartCoroutine(WaitFrame());

    }

    private IEnumerator WaitFrame()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                print(dificulty.curLevel);
                dificulty.curLevel++;

                GameObject obj = Instantiate(pool.prefab, new Vector3(0,0,1000), Quaternion.identity);
                objectPool.Enqueue(obj);
                yield return 0;
                obj.SetActive(false);
            }

            poolDictionary.Add(pool.tag, objectPool);
            dificulty.curLevel = 0;
        }



    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
            pooledObject.OnObjectSpawn();

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}

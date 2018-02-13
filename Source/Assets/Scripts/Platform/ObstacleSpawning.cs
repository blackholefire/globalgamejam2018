using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawning : MonoBehaviour, IPooledObject {

    public GameObject floor;
    public List<GameObject> block;

    public DificultyController dificulty;

    public GameObject backWall;

    public static int obstacleCount = 15;

    public float maxGap = 15;

    private bool spawnedNext = false;

    private PlayerController player;

    public GameObject fireWall;

    public GameObject puzzle;

    public List<GameObject> allPuzzles;

    public GameObject checkpoint;

    private const int powerUpChance = 15;

    public List<GameObject> powerUps;

    public Renderer rend;
    void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Wall" || child.gameObject.tag == "Health" || child.gameObject.tag == "Dash")
                child.gameObject.SetActive(false);
        }

        dificulty = GameObject.Find("DifficultyController").GetComponent<DificultyController>();
        foreach(Transform t in puzzle.transform)
        {
            Destroy(t.gameObject);
        }
        Instantiate(allPuzzles[dificulty.curLevel], puzzle.transform);

    }

    void IPooledObject.OnObjectSpawn()
    {
        rend = GetComponent<Renderer>();
    }

    // Use this for initialization
    void Start() {

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();


        if(dificulty.curLevel < 5)
            block.AddRange(dificulty.lists[dificulty.curLevel]);

        if (dificulty.curLevel == 3)
        {
            foreach (GameObject obj in dificulty.lists[0])
            {
                block.Remove(obj);
            }
            //block.RemoveRange(0, dificulty.lists[0].Count);
            //block.RemoveRange(0, dificulty.lists[1].Count);
        }


        rend = GetComponent<Renderer>();

        if(dificulty.curLevel > 0)
        {
            obstacleCount += 5;
            if (obstacleCount > 25)
                obstacleCount = 25;
        }

        Vector3 offset = transform.position - transform.forward * rend.bounds.min.z;
        Vector3 pos = transform.position + offset;

        pos.z = pos.z - 10;

        float range = (rend.bounds.min.z +5)- (pos.z);
        maxGap = Mathf.Abs(range / obstacleCount);

        pos.x = 0;
        pos.y = 1f;
        for (int i = 0; i < obstacleCount; i++)
        {


            int r = UnityEngine.Random.Range(0, 100);
            if(r < powerUpChance)
            {
                SpawnPowerUp(rend, pos.z, offset);
            }

            GameObject o = block[UnityEngine.Random.Range(0, block.Count)];

            GameObject spawned = Instantiate(o, pos, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(spawned.transform.position, 4, 1 << 8);

            if (colliders.Length > 1 )
            {
                foreach(Collider c in colliders)
                {
                    if(c.gameObject != spawned)
                    {
                        Destroy(c.gameObject);
                    }
                }
                //continue;
            }
                spawned.transform.SetParent(this.transform);
                pos.z -= maxGap;
            //if (pos.z < rend.bounds.min.z + 10)
               // break;  
        }


	}
#if UNITY_EDITOR
    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Return) && !spawnedNext)
        {
            SpawnNext();
        }
    }
#endif

    public void SpawnNext()
    {
        if (dificulty.curLevel < 5)
        {
            dificulty.curLevel++;
            //print(GetComponent<Renderer>().bounds.max.z);
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 400);
            player.curLevel = Instantiate(floor, newPos, Quaternion.identity).GetComponent<ObstacleSpawning>();
            //player.curLevel = ObjectPooler.Instance.SpawnFromPool("Level", newPos, Quaternion.identity).GetComponent<ObstacleSpawning>();
            player.lastCheckPoint = player.curLevel.checkpoint;
            player.curLevel.backWall.SetActive(false);
            if(player.lives < 3)
                player.lives = 3;
            player.dashCharge = 100;
            spawnedNext = true;
            
        }
    }

    void SpawnPowerUp(Renderer ren, float z, Vector3 offset)
    {
        float xPos = UnityEngine.Random.Range(ren.bounds.min.x, ren.bounds.max.x);
        Vector3 pos = transform.position + offset;
        float range = pos.z - 15;
        float zPos = z + 5;
        if (zPos > range)
            zPos = range;

        Vector3 spawnPosition = new Vector3(xPos, 0.5f, zPos);
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1, 1 << 8);

        while (colliders.Length > 0)
        {
            spawnPosition = new Vector3(UnityEngine.Random.Range(ren.bounds.min.x, ren.bounds.max.x), spawnPosition.y, spawnPosition.z + 1);
            colliders = Physics.OverlapSphere(spawnPosition, 1, 1 << 8); 
        }
        GameObject spawned = Instantiate(powerUps[UnityEngine.Random.Range(0, powerUps.Count)], spawnPosition, Quaternion.identity);
        spawned.transform.parent = transform;

    }
}

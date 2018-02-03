﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawning : MonoBehaviour {

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


    void Awake()
    {
        dificulty = GameObject.Find("DifficultyController").GetComponent<DificultyController>();
        foreach(Transform t in puzzle.transform)
        {
            Destroy(t.gameObject);
        }
        Instantiate(allPuzzles[dificulty.curLevel], puzzle.transform);

    }

    // Use this for initialization
    void Start() {

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        block.AddRange(dificulty.lists[dificulty.curLevel]);

        if (dificulty.curLevel >= 2)
        {

            block.RemoveRange(0, dificulty.lists[dificulty.curLevel - 2].Count);

        }
        Renderer rend = GetComponent<Renderer>();

        obstacleCount += 3 ;
        if (obstacleCount > 25)
            obstacleCount = 25;

        Vector3 offset = transform.position - transform.forward * rend.bounds.min.z;
        Vector3 pos = transform.position + offset;

        pos.z = pos.z - 10;

        float range = (rend.bounds.min.z -20)- (pos.z);
        maxGap = Mathf.Abs(range / obstacleCount);

        pos.x = 0;
        pos.y = 1f;
        for (int i = 0; i < obstacleCount; i++)
        {

            bool spawnNew = false;
            GameObject o = block[Random.Range(0, block.Count)];

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
            if (pos.z < rend.bounds.min.z +10)
                break;  
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
        if (dificulty.curLevel < 4)
        {
            print(GetComponent<Renderer>().bounds.max.z);
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 400);
            player.curLevel = Instantiate(floor, newPos, Quaternion.identity).GetComponent<ObstacleSpawning>();
            player.lastCheckPoint = player.curLevel.checkpoint;
            player.curLevel.backWall.SetActive(false);
            player.lives = 5;
            player.dashCharge = 100;
            spawnedNext = true;
            dificulty.curLevel++;
        }
    }
}

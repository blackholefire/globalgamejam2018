using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawning : MonoBehaviour {

    public GameObject floor;
    public List<GameObject> block;

    public DificultyController dificulty;

    public GameObject backWall;

    public int obstacleCount = 10;

    public float maxGap = 15;

    private bool spawnedNext = false;


    // Use this for initialization
    void Start () {

        block.AddRange(dificulty.lists[dificulty.curLevel]);
        Renderer rend = GetComponent<Renderer>();

        obstacleCount = 9 * (dificulty.curLevel + 1);

        Vector3 offset = transform.position - transform.forward * rend.bounds.min.z;
        Vector3 pos = transform.position + offset;

        pos.z = pos.z - 10.0f;

        float range = (rend.bounds.min.z)- (rend.bounds.max.z);
        maxGap = Mathf.Abs(range / obstacleCount);
        pos.x = 0;
        pos.y = 1f;
        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject o = block[Random.Range(0, block.Count)];

            GameObject spawned = Instantiate(o, pos, Quaternion.identity);
            spawned.transform.SetParent(this.transform);
            pos.z -= maxGap;
        }

	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Return) && !spawnedNext)
        {
            SpawnNext();
        }
#endif
    }

    void SpawnNext()
    {
        dificulty.curLevel++;
        print(GetComponent<Renderer>().bounds.max.z);
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 400);
        Instantiate(floor, newPos, Quaternion.identity);
        spawnedNext = true;
    }
}

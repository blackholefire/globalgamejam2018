using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawning : MonoBehaviour {

    public GameObject floor;
    public GameObject block;

    public int obstacleCount = 10;

    // Use this for initialization
    void Start () {
        Renderer rend = GetComponent<Renderer>();

        Vector3 offset = transform.position - transform.forward * rend.bounds.max.z;
        Vector3 pos = transform.position + offset;

        pos.y = 0.5f;
        for (int i = 0; i < obstacleCount; i++)
        {
            pos.x = Random.Range(rend.bounds.min.x, rend.bounds.max.x);

            Instantiate(block, pos, Quaternion.identity);
            pos.z += 15;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
           print( GetComponent<Renderer>().bounds.max.z);
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z +150);
            Instantiate(floor, newPos, Quaternion.identity);
        }
	}
}

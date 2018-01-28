using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour {

    PlayerController player;

    GameObject wallToOpen;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        wallToOpen = player.curLevel.GetComponent<ObstacleSpawning>().fireWall;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Open()
    {
        wallToOpen.GetComponent<Animator>().SetTrigger("Down");
        Destroy(gameObject);
    }
}

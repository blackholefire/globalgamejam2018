using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour {

    PlayerController player;

    GameObject wallToOpen;

    Animator wallAnimator;

    public Animator cameraObj;

    public AudioClip puzzleComplete;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        wallToOpen = player.curLevel.GetComponent<ObstacleSpawning>().fireWall;
        wallAnimator = wallToOpen.GetComponent<Animator>();
        cameraObj = Camera.main.transform.parent.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Open()
    {
        wallAnimator.SetTrigger("Down");
        audioSource.PlayOneShot(puzzleComplete, 0.5f);
        cameraObj.SetTrigger("Follow");
        Destroy(gameObject);
    }
}

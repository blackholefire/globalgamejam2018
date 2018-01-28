using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCircuit : MonoBehaviour {
    public bool on = false;

    public GameObject onBar;
    public GameObject offBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (on)
        {
            onBar.SetActive(true);
            offBar.SetActive(false);
        }
        if (!on)
        {
            onBar.SetActive(false);
            offBar.SetActive(true);
        }
    }
}

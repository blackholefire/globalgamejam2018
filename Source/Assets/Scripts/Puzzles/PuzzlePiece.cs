using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour {
    
    public PuzzleCircuit nextCirc;
    public PuzzleCircuit prevCirc;

    public PuzzleController controller;

    public bool lastPiece;


    bool canNext = false;

    bool hasOpened = false;

	// Use this for initialization
	void Awake () {
        controller = gameObject.transform.parent.parent.GetComponent<PuzzleController>();
	}

    void Update()
    {
        if (lastPiece && prevCirc.on)
        {
            if(canNext && !hasOpened)
            {
                hasOpened = true;
                controller.Open();
            }
                
        }

        if (prevCirc)
        {
            if (!prevCirc.on)
            {
                if(nextCirc)
                    nextCirc.on = false;
            }
        }
    }
	
	// Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PuzzleCircuit>())
        {
            if (other.gameObject.GetComponent<PuzzleCircuit>() == nextCirc)
            {
                if (prevCirc)
                {
                    if (prevCirc.on && canNext)
                        nextCirc.on = true;
                }
                else
                    nextCirc.on = true;
            }
            if (other.gameObject.GetComponent<PuzzleCircuit>() == prevCirc)
            {
                canNext = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PuzzleCircuit>())
        {
            if (other.gameObject.GetComponent<PuzzleCircuit>() == nextCirc)
            {
                    nextCirc.on = false;
            }
            if (other.gameObject.GetComponent<PuzzleCircuit>() == prevCirc)
            {
                //prevCirc.on = false;
                canNext = false;
            }
        }
    }
}

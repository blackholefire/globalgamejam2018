using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour {
    
    public PuzzleCircuit nextCirc;
    public PuzzleCircuit prevCirc;

    private GameObject nextObj;
    private GameObject prevObj;

    public PuzzleController controller;

    public bool lastPiece;


    bool canNext = false;

    bool hasOpened = false;

	// Use this for initialization
	void Start () {
        controller = gameObject.transform.parent.GetComponent<PuzzleController>();
        if(nextCirc)
            nextObj = nextCirc.gameObject;
        if(prevCirc)
            prevObj = prevCirc.gameObject;
	}

    void Update()
    {
        if (controller == null)
        {
           gameObject.transform.parent.GetComponent<PuzzleController>();
        }

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
        if (other.CompareTag("PuzzleCircuit"))
        {
            //PuzzleCircuit otherCirc = other.gameObject.GetComponent<PuzzleCircuit>();

            if (other.gameObject == nextObj)
            {
                if (prevCirc)
                {
                    if (prevCirc.on && canNext)
                        nextCirc.on = true;
                }
                else
                    nextCirc.on = true;
            }
            if (other.gameObject == prevObj)
            {
                canNext = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PuzzleCircuit"))
        {
            //PuzzleCircuit otherCirc = other.gameObject.GetComponent<PuzzleCircuit>();
            if (other.gameObject == nextObj)
            {
                    nextCirc.on = false;
            }
            if (other.gameObject == prevObj)
            {
                //prevCirc.on = false;
                canNext = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float h = speed * Input.GetAxis("Horizontal");
        float v = speed * Input.GetAxis("Vertical");

        Vector3 movment = new Vector3(h, 0.0f, v);

        transform.Translate(movment);
    }
}

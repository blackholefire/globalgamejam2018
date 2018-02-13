using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    public int speed = 2;
    public static bool moving = true;
	// Use this for initialization


	// Update is called once per frame
	void Update () {
        if(moving)
            transform.Translate(Vector3.back * speed *  Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        if (moving)
            Destroy(gameObject);
    }
}

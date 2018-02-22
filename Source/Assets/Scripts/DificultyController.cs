using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DificultyController : MonoBehaviour {
    public List<GameObject> level1;
    public List<GameObject> level2;
    public List<GameObject> level3;
    public List<GameObject> level4;
    public List<GameObject> level5;

    public List<GameObject>[] lists;


    public int curLevel = 0;
	// Use this for initialization
	void Awake () {

        lists = new List<GameObject>[] { level1, level2, level3, level4, level5};
    }
}

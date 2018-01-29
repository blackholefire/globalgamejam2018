using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class screenLoader : MonoBehaviour {
	public void onClickPlay () {
		SceneManager.LoadScene("main");
	}
	public void onClickTutorial () {
	SceneManager.LoadScene("tutorial");

	}
	public void onClickCredits () {
		SceneManager.LoadScene ("credits");
	}
	public void onClickBack () {
		SceneManager.LoadScene ("menu");
	}

    public void Quit()
    {
        Application.Quit();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    public static bool paused = false;
    public BlurOptimized cameraBlur;

    public Button resumebtn;
    public EventSystem eventSystem;

    public Animator HUD_anim;

    Animator pauseAnim;

    void Start()
    {
        pauseAnim = GetComponent<Animator>();
    }
 
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            switch (paused)
            {
                case true:
                    UnPauseGame();
                    break;
                case false:
                    PauseGame();
                    break;
            }
        }
	}

    void PauseGame()
    {
        cameraBlur.enabled = true;
        Time.timeScale = 0;
        paused = true;
        HUD_anim.SetBool("Paused", true);
        pauseAnim.SetBool("Paused", true);
        resumebtn.Select();
    }

    public void UnPauseGame()
    {
        cameraBlur.enabled = false;
        Time.timeScale = 1;
        paused = false;
        HUD_anim.SetBool("Paused", false);
        pauseAnim.SetBool("Paused", false);
        eventSystem.SetSelectedGameObject(null);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

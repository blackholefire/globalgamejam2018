using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{

    public static bool paused = false;
    public BlurOptimized cameraBlur;

    public Button resumebtn;
    public EventSystem eventSystem;

    public Image controllerImage;
    public Image keyboardImage;
    public GameObject prompt;
    static GameObject staticPrompt;

    public static bool promptActive = false;

    static Image contrImg;
    static Image keyImg;

    public Animator HUD_anim;

    Animator pauseAnim;

    void Start()
    {
        paused = false;
        pauseAnim = GetComponent<Animator>();
        Cursor.visible = false;
        contrImg = controllerImage;
        keyImg = keyboardImage;
        staticPrompt = prompt;
    }

    // Update is called once per frame
    void Update()
    {
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
        Cursor.visible = true;
        Time.timeScale = 0;
        paused = true;
        HUD_anim.SetBool("Paused", true);
        pauseAnim.SetBool("Paused", true);
        resumebtn.Select();
    }

    public void UnPauseGame()
    {
        cameraBlur.enabled = false;
        Cursor.visible = false;
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

    public static void ShowPrompt()
    {
        promptActive = true;
        bool isController = false;
        if (Input.GetJoystickNames().Length > 0)
        {
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                //print(Input.GetJoystickNames()[i]);
                if (Input.GetJoystickNames()[i] == "")
                {
                    isController = false;
                }
                else isController = true;
            }
        }

        if (isController)
        {
            contrImg.enabled = true;
            keyImg.enabled = false;
        }
        else
        {
            contrImg.enabled = false;
            keyImg.enabled = true;
        }

        staticPrompt.SetActive(true);

    }

    public static void HidePrompt()
    {
        promptActive = false;
        staticPrompt.SetActive(false);
    }
}

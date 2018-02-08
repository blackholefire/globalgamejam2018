using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuAnimationController : MonoBehaviour {
    public Animator anim;

    public List<Image> GamepadIcons;
    public List<Image> KeyboardIcons;

    public Button backButton;
    public Button playButton;


    bool controllerActive = false;

    public EventSystem eventSys;

    string transitionTo;

    float checkTimer = 2.5f;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        Time.timeScale = 1;
	}

    public void Update()
    {
        checkTimer += Time.deltaTime;
        if (Input.GetJoystickNames().Length > 0 && checkTimer >= 2.5f)
        {
            checkTimer = 0;
            bool isController = false;
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                print(Input.GetJoystickNames()[i]);
                if (Input.GetJoystickNames()[i] == "")
                {
                    isController = false;
                }
                else isController = true;
            }

            if (isController)
            {
                controllerActive = true;
                foreach (Image i in GamepadIcons)
                {
                    i.enabled = true;
                }
                foreach (Image i in KeyboardIcons)
                {
                    i.enabled = false;
                }

                if (eventSys.currentSelectedGameObject == null)
                {
                    if (anim.GetBool("Main"))
                    {
                        playButton.Select();
                        playButton.OnSelect(null);
                    }
                    else
                    {
                        backButton.Select();
                        backButton.OnSelect(null);
                    }
                }

            }
            else
            {
                controllerActive = false;
                foreach (Image i in GamepadIcons)
                {
                    i.enabled = false;
                }
                foreach (Image i in KeyboardIcons)
                {
                    i.enabled = true;
                }
            }
        }
    }

    public void StartCredits()
    {
        anim.SetBool("Main",false) ;
        backButton.Select();
        transitionTo = "Credits";
    }

    public void StartControls()
    {
        anim.SetBool("Main", false);
        backButton.Select();
        transitionTo = "Controls";
    }

    public void Back()
    {
        anim.SetBool(transitionTo, false);
        playButton.Select();
        transitionTo = "Main";
    }

    public void AnimationEvent()
    {
        anim.SetBool(transitionTo, true);
    }

}

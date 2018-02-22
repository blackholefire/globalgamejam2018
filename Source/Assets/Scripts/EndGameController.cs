using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class EndGameController : MonoBehaviour {
    private static EndGameController _instance;

    public static EndGameController Instance { get { return _instance; } }

    [TextArea]
    public string lossText;
    [TextArea]
    public string winText;

    public Text resultText;

    private Animator childAnim;
    public Button menuButton;

    private BlurOptimized cameraBlur;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }

        childAnim = transform.GetChild(0).GetComponent<Animator>();
        cameraBlur = Camera.main.GetComponent<BlurOptimized>();
    }

    // Use this for initialization
    public void SetLoss()
    {
        Cursor.visible = true;
        resultText.text = lossText;
        childAnim.SetTrigger("End");
        PlatformController.moving = false;
        menuButton.Select();
        cameraBlur.enabled = true;
    }

    public void SetWin()
    {
        Cursor.visible = true;
        resultText.text = winText;
        childAnim.SetTrigger("End");
        PlatformController.moving = false;
        menuButton.Select();
        cameraBlur.enabled = true;
    }
}

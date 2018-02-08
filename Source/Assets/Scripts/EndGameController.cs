using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour {
    private static EndGameController _instance;

    public static EndGameController Instance { get { return _instance; } }

    [TextArea]
    public string lossText;
    [TextArea]
    public string winText;

    public Text resultText;

    private GameObject child;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }

        child = transform.GetChild(0).gameObject;
    }

    // Use this for initialization
    public void SetLoss()
    {
        Cursor.visible = true;
        resultText.text = lossText;
        child.SetActive(true);
        PlatformController.moving = false;
    }

    public void SetWin()
    {
        Cursor.visible = true;
        resultText.text = winText;
        child.SetActive(true);
        PlatformController.moving = false;
    }
}

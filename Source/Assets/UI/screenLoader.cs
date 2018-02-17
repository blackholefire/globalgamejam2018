using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class screenLoader : MonoBehaviour {
    public Image loadBar;
    public GameObject load;
    public Text progressText;

	public void onClickPlay () {
        StartCoroutine(LoadAsynchronously());
        load.SetActive(true);
	}

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("main");
        operation.allowSceneActivation = false;

        float progress = 0;
        while (operation.progress < 0.5f)
        {
            progress = Mathf.Clamp01(operation.progress / .9f);
            loadBar.fillAmount = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }

        while(progress < 1)
        {
            progress += 0.025f;
            loadBar.fillAmount = progress;
            if ((progress * 100) > 100)
                progress = 1;
            progressText.text = Mathf.FloorToInt(progress * 100f) + "%";
            yield return 1;
        }
        operation.allowSceneActivation = true;

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
}

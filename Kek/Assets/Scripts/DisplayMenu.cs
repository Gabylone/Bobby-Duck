using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayMenu : MonoBehaviour {

    public GameObject group;

	// Use this for initialization
	void Start () {
        Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenMenu()
    {
        RegionRayBlocker.onTouchRayblocker += Hide;
        RegionRayBlocker.Instance.Show();

        Sound.Instance.PlaySound(Sound.Type.Menu2);

        Show();
    }

    public void Show()
    {
        Tween.Bounce(transform);

        group.SetActive(true);
    }

    public void Hide()
    {
        group.SetActive(false);
    }

    public void Quit()
    {
        Transition.Instance.Fade(1f);

        Sound.Instance.PlaySound(Sound.Type.Menu3);
        Invoke("QuitGame", 1f);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    public void RemoveSaves()
    {
        Sound.Instance.PlaySound(Sound.Type.Menu4);
        SaveTool.Instance.EraseSave();

        Transition.Instance.Fade(1f);

        Invoke("RestartGame", 1f);
    }

    void RestartGame()
    {
        SceneManager.UnloadSceneAsync("Main (map)");

        Destroy(SaveTool.Instance.dontDestoyOnLoadGroup);
        SceneManager.LoadSceneAsync("Splash");
    }
}

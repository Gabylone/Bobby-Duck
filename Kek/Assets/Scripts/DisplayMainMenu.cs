using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class DisplayMainMenu : MonoBehaviour {

    public Button newGameButton;
    public Button loadGameButton;
    public Button switchTutoButton;
    public Button quitGameButton;

    private void Start()
    {
        UpdateTutorialButton();

        if (!SaveTool.Instance.FileExists("regions"))
        {
            loadGameButton.interactable = false;
        }
    }

    private void UpdateTutorialButton()
    {
        if (Tutorial.show)
        {
            switchTutoButton.GetComponentInChildren<Text>().text = "Tutoriel : activé";
        }
        else
        {
            switchTutoButton.GetComponentInChildren<Text>().text = "Tutoriel : désactivé";
        }
    }

    public void NewGame()
    {

        Sound.Instance.PlaySound(Sound.Type.Menu7);

        if (SaveTool.Instance.FileExists("inventory"))
        {
            SaveTool.Instance.EraseSave();
        }

        Tween.Bounce(newGameButton.transform);
        Transition.Instance.Fade(1f);
        Invoke("NewGameDelay",1f);
    }

    void NewGameDelay()
    {
        SceneManager.LoadScene( "Main (map)" );
    }

    public void LoadGame()
    {
        Sound.Instance.PlaySound(Sound.Type.Menu7);
        Tween.Bounce(loadGameButton.transform);
        Transition.Instance.Fade(1f);
        Invoke("LoadGameDelay", 1f);
    }

    void LoadGameDelay()
    {
        SceneManager.LoadScene("Main (map)");
    }

    public void SwitchTuto()
    {
        Sound.Instance.PlaySound(Sound.Type.Menu7);
        Tween.Bounce(switchTutoButton.transform);
        Tutorial.show = !Tutorial.show;

        UpdateTutorialButton();
    }

    public void QuitGame()
    {
        Sound.Instance.PlaySound(Sound.Type.Menu7);
        Tween.Bounce(quitGameButton.transform);

        Transition.Instance.Fade(1f);

        Invoke("QuitGameDelay", 1f);
    }

    void QuitGameDelay()
    {
        Application.Quit();
    }
}

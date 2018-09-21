using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Holoville.HOTween;

public class MainMenu : MonoBehaviour {

    public Button newGameButton;
    public Button loadGameButotn;
    public Button quitGameButton;

    public Transform sound_OnTransform;
    public Transform sound_OffTransform;

    // Use this for initialization
    void Start () {
		
        if ( PlayerPrefs.GetInt("progress", -1) < 0)
        {
            loadGameButotn.interactable = false;
        }
        else
        {
            loadGameButotn.interactable = true;
        }

        UpdateSoundUI();
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        Invoke("NewGameDelay", 1f);

        Transition.Instance.Fade(1f);

        SoundManager.Instance.Play(SoundManager.SoundType.Door_Open);
    }

    void NewGameDelay()
    {
        SceneManager.LoadScene("map");
    }


    public void LoadGame()
    {
        Invoke("LoadGameDelay", 1f);

        SoundManager.Instance.Play(SoundManager.SoundType.Door_Open);
        Transition.Instance.Fade(1f);
    }

    void LoadGameDelay()
    {

        SceneManager.LoadScene("map");
    }

    public void QuitGame()
    {
        Invoke("QuitGameDelay", 1f);

        Tween.Bounce(quitGameButton.transform);

        SoundManager.Instance.Play(SoundManager.SoundType.Door_Close);
        Transition.Instance.Fade(1f);
    }

    void QuitGameDelay()
    {
        Application.Quit();
    }

    public void RetourCarte()
    {
        Tween.Bounce(quitGameButton.transform);

        Invoke("LoadGameDelay", 1f);

        SoundManager.Instance.Play(SoundManager.SoundType.Door_Close);
        Transition.Instance.Fade(1f);
    }

    public void SwitchSounds()
    {
        SoundManager.Instance.playSounds = !SoundManager.Instance.playSounds;

        Inventory.Instance.Save();

        UpdateSoundUI();
    }

    void UpdateSoundUI()
    {
        if (SoundManager.Instance.playSounds)
        {
            sound_OffTransform.localScale = Vector3.one;
            sound_OnTransform.localScale = Vector3.one * 1.2f;
            /*HOTween.To(sound_OnTransform.transform, 0.5f, "localScale", Vector3.one * 1.2f);
            HOTween.To(sound_OffTransform.transform, 0.5f, "localScale", Vector3.one);*/
        }
        else
        {
            sound_OnTransform.localScale = Vector3.one;
            sound_OffTransform.localScale = Vector3.one * 1.2f;

            /*HOTween.To(sound_OffTransform.transform, 0.5f, "localScale", Vector3.one * 1.2f);
            HOTween.To(sound_OnTransform.transform, 0.5f, "localScale", Vector3.one);*/
        }
        
    }



}

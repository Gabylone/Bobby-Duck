using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class DisplayLevel : DisplayGroup {

    public static DisplayLevel Instance;

    public Text levelNameText;

    public float fadeDuration = 1f;

    public float bounceAmount = 0.1f;

    public GameObject endlessGroup;
    public Text endless_UiText;

    public Star[] stars;

    public GameObject lockedGroup;

    public GameObject displayHeartGroup;

    public GameObject multiplyGoldGroup;
    public GameObject continueButton;

    bool locked = false;

    public Button multiplyGoldButton;

    Level levelDisplayed;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        Inventory.multiplyGold = false;

        Invoke("StartDelay", 1.5f);

    }

    void StartDelay()
    {
        Tutorial.Instance.Show(TutorialStep.Map, DisplayUpgrades.Instance.tavernButton.transform);
    }

    public void HandleOnPointerClick(int id)
    {
        if ( id == 10 && !Inventory.Instance.displayedInvitation)
        {
            DisplayInvitation.Instance.Open();
            return;
        }

        Open();

        SoundManager.Instance.Play(SoundManager.SoundType.UI_Open);

        levelDisplayed = Level.levels[id];

        UpdateUI();
    }

    public override void OpenDelay()
    {
        base.OpenDelay();

        if (levelDisplayed.newClientType != Client.Type.None)
        {
            closeGroup.SetActive(false);
            Invoke("DisplayNewClientType", 0.2f);
        }

        if ( levelDisplayed.id < 2)
        {
            
        }
        else
        {
            if (!Inventory.Instance.showedMultGold)
            {
                Tutorial.Instance.Show(TutorialStep.GoldMult, multiplyGoldGroup.transform, true);
                Inventory.Instance.showedMultGold = true;
                Inventory.Instance.Save();
            }
            else
            {
                if ( locked)
                {
                    Debug.Log("is locked");
                    Tutorial.Instance.Show(TutorialStep.Health, displayHeartGroup.transform , true);
                }
            }
        }
    }

    void DisplayNewClientType()
    {
        DisplayNewClient.Instance.Display(levelDisplayed.newClientType);

        Invoke("DisplayNewClientTypeDelay", 0.5f);
    }

    void DisplayNewClientTypeDelay()
    {
        closeGroup.SetActive(true);
    }

    public override void Close(bool b)
    {
        base.Close(b);

        SoundManager.Instance.Play(SoundManager.SoundType.UI_Close);
        DisplayNewClient.Instance.Hide();

        CancelInvoke("DisplayNewClientType");
    }

    public void UpdateUI ()
    {
        endlessGroup.SetActive(false);

        Level.SetCurrent(levelDisplayed);

        if (levelDisplayed.id == 0)
        {
            DisplayTutorial();
            return;
        }

        if ( LevelManager.endless )
        {
            DisplayEndless();

            return;
        }

        DisplayNormalLevel();

    }

    void DisplayEndless()
    {

        endlessGroup.SetActive(true);
        levelNameText.text = "ENDLESS";

        endless_UiText.text = "" + Inventory.Instance.highscore;

        Unlock();

        multiplyGoldGroup.SetActive(false);
    }

    private void DisplayNormalLevel()
    {
        levelNameText.text = "" + (levelDisplayed.id);

        if (Inventory.multiplyGold)
        {
            multiplyGoldButton.interactable = false;
        }
        else
        {
            multiplyGoldButton.interactable = true;
        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].UpdateUI(levelDisplayed.clientObjectives[i]);

            if (Inventory.Instance.starAmounts[levelDisplayed.id] > i)
            {
                stars[i].Bounce();
            }
            else
            {
                stars[i].Fade();
            }

        }

        if (Inventory.Instance.lifes <= 0)
        {
            Lock();
        }
        else
        {
            Unlock();
        }
    }

    void DisplayTutorial()
    {
        if (Inventory.currentLanguageType == Inventory.LanguageType.French)
        {
            levelNameText.text = "Tutoriel";
        }
        else
        {
            levelNameText.text = "Tutorial";
        }

        Unlock();

        multiplyGoldGroup.SetActive(false);

        foreach (var item in stars)
        {
            item.Bounce();
        }
    }

    public void Lock()
    {
        locked = true;

        lockedGroup.SetActive(true);

        continueButton.SetActive(false);
        multiplyGoldGroup.SetActive(false);
        displayHeartGroup.SetActive(true);
    }

    public void Unlock()
    {
        locked = false;

        continueButton.SetActive(true);
        displayHeartGroup.SetActive(false);
        lockedGroup.SetActive(false);
        multiplyGoldGroup.SetActive(true);
    }

    public void LaunchLevel()
    {
        Close();

        Tween.Bounce(continueButton.transform);

        Transition.Instance.Fade(fadeDuration);

        Invoke("LaunchLevelDelay", fadeDuration);

        SoundManager.Instance.Play(SoundManager.SoundType.Star);

        if (levelDisplayed.id == 0)
        {
            Tutorial.show = true;
        }
    }

    void LaunchLevelDelay()
    {
        SceneManager.LoadScene("tavern");
    }

    
}

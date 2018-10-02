using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DisplayEndOfDay : DisplayGroup {

    public static DisplayEndOfDay Instance;

    public bool lost = false;

    public Text levelName_Text;
    
    public Star[] stars;

    public Text clientText;

    public float timeBerweenClients = 0.05f;

    public float fadeDuration = 1f;

    public GameObject lostGroup;

    public GameObject tutoGroup;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Open()
    {
        if (opened)
        {
            return;
        }

        base.Open();

        Music.Instance.PlayMenu();

        Swipe.Instance.enabled = false;

        if (LevelManager.endless)
        {
            levelName_Text.text = "Endless";
            tutoGroup.SetActive(false);

            clientText.text = "0";

        }
        else if (Level.Current.id == 0)
        {
            levelName_Text.text = "Tuto";
            tutoGroup.SetActive(true);

            clientText.text = "0 / " + ClientManager.Instance.currentClientAmount;

        }
        else
        {
            levelName_Text.text = "Level " + Level.Current.id;
            tutoGroup.SetActive(false);

            clientText.text = "0 / " + ClientManager.Instance.currentClientAmount;

        }

        if (lost)
        {
            Lost();

        }
        else
        {
            Win();
        }


    }

    public override void OpenDelay()
    {
        base.OpenDelay();

        if (Level.Current.id != 0)
        {
            closeGroup.SetActive(false);
        }
        else
        {
            closeGroup.SetActive(true);
        }
    }

    void Lost()
    {

        if ( LevelManager.endless)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);

            }

            lostGroup.SetActive(false);

        }
        else
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].Fade();

                stars[i].uiText.text = "x";
                stars[i].image.color = Color.red;
            }

            lostGroup.SetActive(true);

            Inventory.Instance.SaveLifes();


        }

        Invoke("LostDelay", tweenDuration * 2f);

    }

    void LostDelay()
    {
        if (!LevelManager.endless)
        {
            StartCoroutine(LostCoroutine());
        }
        else
        {
            StartCoroutine(WinCououtine());
        }
    }

    public int goldLossPerFrame = 4;
    public float timeBetweenGoldLoss = 0.5f;

    IEnumerator LostCoroutine()
    {

        int g = Inventory.Instance.goldAquiredInLevel;

        while (g > 0)
        {


            Inventory.Instance.RemoveGold(goldLossPerFrame);
              
            g -= goldLossPerFrame;

            SoundManager.Instance.Play(SoundManager.SoundType.UI_Bip);

            yield return new WaitForSeconds(timeBetweenGoldLoss);
        }

        closeGroup.SetActive(true);
        Tween.Bounce(closeGroup.transform);
    }

    void Win()
    {
        lostGroup.SetActive(false);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].Fade();

            stars[i].UpdateUI(Level.Current.clientObjectives[i]);
        }

        if (Level.Current.id != 0)
        {
            Invoke("WinDelay", tweenDuration * 2f);
        }

    }

    void WinDelay()
    {

        StartCoroutine(WinCououtine());
    }

    IEnumerator WinCououtine()
    {
        int tmpClients = 0;

        int clientObjectiveIndex = 0;

        int clientsServed = ClientManager.Instance.currentClientAmount - ClientManager.Instance.lostClientAmount;

        while (tmpClients < clientsServed)
        {
            
            ++tmpClients;

            if (!LevelManager.endless)
            {
                clientText.text = "" + tmpClients + " / " + ClientManager.Instance.currentClientAmount;
            }
            else
            {
                clientText.text = "" + tmpClients;
            }

            if (tmpClients >= Level.Current.clientObjectives[clientObjectiveIndex])
            {
                stars[clientObjectiveIndex].Bounce();

                ++clientObjectiveIndex;

            }

            yield return new WaitForSeconds(timeBerweenClients);

            if (clientObjectiveIndex == stars.Length)
            {
                break;
            }

        }

        if (!LevelManager.endless)
        {
            clientText.text = "" + tmpClients + " / " + ClientManager.Instance.currentClientAmount;

            if (clientObjectiveIndex > 0)
            {
                Inventory.Instance.SetStarAmount(Level.Current.id, clientObjectiveIndex);

                Debug.Log("setting " + clientObjectiveIndex + " for level");

                if (Inventory.Instance.progress == Level.Current.id)
                {
                    ++Inventory.Instance.progress;
                }

            }
        }
        else
        {
            clientText.text = "" + tmpClients;
        }

        closeGroup.SetActive(true);
        Tween.Bounce(closeGroup.transform);

        Inventory.Instance.Save();

    }

    public void GotoMap()
    {
        Close(false);

        Tween.Bounce(closeGroup.transform);

        Transition.Instance.Fade(fadeDuration);

        Invoke("GotoMapDelay", fadeDuration);
    }

    void GotoMapDelay()
    {
        SceneManager.LoadScene("map");
    }
}

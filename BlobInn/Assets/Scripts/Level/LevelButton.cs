using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public int id = 0;

    public static int globalId = 0;

    public Text uiText;

    public GameObject lockedGroup;

    public bool endless = false;

    bool locked = false;

    public Star[] stars;

    public GameObject tutoGroup;

    public Image image;

    public GameObject endlessGroup;
    public Text endlessText;

    // Use this for initialization
    void Start()
    {
        lockedGroup.SetActive(false);

        if (endless)
        {
            ShowEndless();
            return;
        }

        if ( id == 0 )
        {
            ShowTuto();
            return;
        }

        ShowNormalLevel();

    }

    void ShowNormalLevel()
    {
        uiText.text = "" + (Level.levels[id].id);

        if (id > Inventory.Instance.progress)
        {
            Lock();
        }
        else
        {
            Unlock();

        }
    }

    private void Unlock()
    {
        int count = 3;

        for (int starIndex = 0; starIndex < stars.Length; starIndex++)
        {
            int starAmount = Inventory.Instance.starAmounts[Level.levels[id].id];

            if (starIndex < starAmount)
            {
                stars[starIndex].image.color = Color.yellow;
            }
            else
            {
                stars[starIndex].Fade();
                //stars[starIndex].transform.localScale = Vector3.one * 0.8f;

                --count;
            }
        }

        if (count == 3)
        {
            image.color = LevelLoader.Instance.color_Completed;
        }
        else if (count > 0)
        {
            image.color = LevelLoader.Instance.color_Done;
        }
        else
        {
            image.color = LevelLoader.Instance.color_YetToBeDone;
        }

    }

    private void ShowTuto()
    {
        uiText.text = "?";

        HideStars();
    }

    private void ShowEndless()
    {
        uiText.gameObject.SetActive(false);

        endlessGroup.SetActive(true);

        endlessText.text = "" + Inventory.Instance.highscore;

        HideStars();
    }

    void HideStars()
    {
        foreach (var item in stars)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void PointerClick()
    {
        if ( locked)
        {
            Tween.Bounce(transform, Tween.defaultDuration, 0.9f);

            return;
        }

        Tween.Bounce(transform);

        if ( endless)
        {
            LevelManager.endless = true;
        }
        else
        {
            LevelManager.endless = false;
        }

        DisplayLevel.Instance.HandleOnPointerClick(id);
    }

    public void Lock()
    {
        image.color = LevelLoader.Instance.color_Locked;

        lockedGroup.SetActive(true);

        locked = true;

        foreach (var star in stars)
        {
            star.gameObject.SetActive(false);
        }
    }
}

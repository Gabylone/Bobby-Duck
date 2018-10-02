using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class RegionActionButton : MonoBehaviour {

    public delegate void OnClickRegionActionButton();
    public static OnClickRegionActionButton onClickRegionActionButton;

    public GameObject group;

    public Text uiText_Difficulty;

	// Use this for initialization
	void Start () {

        RegionButton.onClickRegionButton += HandleOnClickRegionButton;

        Hide();
	}
    private void OnDestroy()
    {
        onClickRegionActionButton = null;
    }

    private void HandleOnClickRegionButton(RegionButton regionButton)
    {
        if ( regionButton.selected)
        {
            Invoke("Show", 0.2f);
        }
        else
        {
            CancelInvoke("Show");
            Hide();
        }
    }

    public void OnClick()
    {
        Tween.Bounce( transform );

        Invoke("LoadGameScene", 1f);

        Transition.Instance.Fade(1f);
        Sound.Instance.PlaySound(Sound.Type.Menu3);

        if (onClickRegionActionButton != null)
            onClickRegionActionButton();
    }

    void LoadGameScene()
    {
        RegionOutcome.Instance.region = Region.selected;

        Tween.Bounce( transform );
        SceneManager.LoadScene("Main (game)");
    }

    void Show()
    {
        group.SetActive(true);
        Tween.Bounce(group.transform);

        uiText_Difficulty.text = "" + Region.selected.level;
    }

    void Hide()
    {
        group.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class RegionActionButton : MonoBehaviour {

    public void OnClick()
    {
        Tween.Bounce( transform );

        Invoke("LoadGameScene", 1f);

        Transition.Instance.Fade(1f);

		//DisplayRegion.Instance.Close ();

		SoundManager.Instance.Play (SoundManager.SoundType.UI_Bip);
    }

    void LoadGameScene()
    {
        RegionOutcome.Instance.region = Region.selected;

        SceneManager.LoadScene(1);
    }
}

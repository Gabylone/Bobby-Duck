using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class MessageDisplay : MonoBehaviour {

    public static MessageDisplay Instance;

    public GameObject overall;

    public Image backgroundImage;
    public float displayDuration = 1f;
    public float quitGameDelay = 3f;

    public Text uiText;

    public GameObject displayGroup;

	void Awake () {
        Instance = this;
	}

    public void Display( string str )
    {
        overall.SetActive(true);
        displayGroup.SetActive(false);

        backgroundImage.color = Color.clear;
        HOTween.To(backgroundImage, displayDuration, "color", Color.black);

        uiText.text = str;

        Invoke("DisplayDelay", displayDuration);
    }

    void DisplayDelay()
    {
        displayGroup.SetActive(true);
        Tween.Bounce( displayGroup.transform );

        Invoke("EndGame", quitGameDelay);
    }

    void EndGame()
    {
        
    }
}

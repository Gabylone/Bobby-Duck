using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Holoville.HOTween;

public class DisplayInfo : MonoBehaviour {

    public static DisplayInfo Instance;

    public GameObject group;

    public float timeOnScreen = 1f;

    public float fadeTime = 0.5f;

    public float decal = 5f;

    public Text uiText;
    public Image image;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        Hide();
	}

    public void Show(string str)
    {
        group.SetActive(true);

        uiText.text = str;

        transform.localPosition = Vector3.up * decal;
        image.color = Color.clear;
        uiText.color = Color.clear;

        HOTween.To( image , fadeTime , "color" , Color.black );
        HOTween.To( uiText, fadeTime , "color" , Color.white);

        HOTween.To(transform, fadeTime, "localPosition", Vector3.zero, false, EaseType.EaseInOutCubic, 0f);

        Invoke("Fade", timeOnScreen);

    }

    void Hide()
    {
        group.SetActive(false);
    }

    void Fade()
    {
        HOTween.To(transform, fadeTime, "localPosition", Vector3.up * -decal, false, EaseType.EaseInCubic, 0f);

        HOTween.To(image, fadeTime, "color", Color.clear);
        HOTween.To(uiText, fadeTime, "color", Color.clear);

        Invoke("FadeDelay", fadeTime);
    }

    void FadeDelay ()
    {
        Hide();
    }
}

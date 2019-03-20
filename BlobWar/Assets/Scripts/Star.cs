using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour {

    public Image image;

    public Text uiText;

    public RectTransform layoutGroup;

    public float bounceAmount = 1.1f;

    public float bounceDuration = 0.25f;

    public void UpdateUI(int i)
    {
        uiText.text = "" + i;

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);

    }

    public void Bounce()
    {
        image.color = Color.white;

        Tween.Bounce(transform);
    }

    public void Fade()
    {
        image.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    }
}

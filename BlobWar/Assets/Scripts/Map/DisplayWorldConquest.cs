using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System.Linq;

public class DisplayWorldConquest : MonoBehaviour {

    public Text uiText_Percent;

    public RectTransform backGroundRectTransform;
    public RectTransform fillRectTransform;
    public RectTransform fillBackgroundRectTransform;

    public float duration = 0.2f;

	// Use this for initialization
	void Start () {
        UpdateDisplay();
	}

    private void UpdateDisplay()
    {
        int currentConquest = Region.regions.Values.ToList().FindAll(x => x.state == Region.State.Conquered).Count;

        float w = backGroundRectTransform.rect.width;

        float l = (float)currentConquest / Region.regions.Count;

        Vector2 scale = new Vector2(-w + l * w, fillBackgroundRectTransform.sizeDelta.y);

        fillBackgroundRectTransform.sizeDelta = scale;

        HOTween.To( fillRectTransform , duration , "sizeDelta" , scale );

        int percent = (int)(l * 100f);

        if (percent < 1)
            percent = 1;

        uiText_Percent.text = percent + "%";
    }
}

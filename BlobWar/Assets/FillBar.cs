using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Holoville.HOTween;

public class FillBar : MonoBehaviour {

    public RectTransform bg;

    public Button button;

    public RectTransform fill;

    public float tweenDur = 0.2f;

    public void UpdateUI(int curr , int max)
    {
        float w = bg.rect.width;

        float l = (float)curr / (float)max;

        Vector2 scale = new Vector2(-w + l * w, 0f);
        //Vector2 scale = new Vector2(Mathf.Lerp(w,0,l), bg.sizeDelta.y);

        //fill.sizeDelta = scale;

        HOTween.To( fill, tweenDur , "sizeDelta" , scale , false , EaseType.EaseOutCubic , 0f);

    }
}

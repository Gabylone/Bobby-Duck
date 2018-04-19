using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class MemberCreationScrollView : MonoBehaviour {

    public RectTransform rectTransform;

    public RectTransform contentFitter;

    Vector2 initScale;

    public float dur = 0.2f;

    private void Start()
    {
        initScale = rectTransform.sizeDelta;
    }

    public void OnPointerDown()
    {
        HOTween.To(rectTransform, dur, "sizeDelta", contentFitter.sizeDelta);

        foreach (var item in GetComponentsInChildren<MemberCreationButton_Apparence>())
        {
            item.GetComponent<Image>().raycastTarget = true;
        }

        transform.SetAsLastSibling();
    }

    public void OnPointerUp ()
    {
        HOTween.To( rectTransform , dur , "sizeDelta" , initScale );

        MemberCreationButton_Apparence.lastSelected.transform.SetAsFirstSibling();

        foreach (var item in GetComponentsInChildren<MemberCreationButton_Apparence>())
        {
            item.GetComponent<Image>().raycastTarget = false;
        }
    }
}

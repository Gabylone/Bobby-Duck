using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System;

public class DisplayZone : MonoBehaviour {

    public GameObject group;

    RectTransform rectTransform;

    public float hideDecal = 20f;
    Vector3 initPos;

    public RectTransform playerRectTransform;

    public Image[] zoneImages;

    public Transform zoneImageParent;

    public float tweenDuration = 1f;

	// Use this for initialization
	void Start () {

        rectTransform = GetComponent<RectTransform>();

        initPos = rectTransform.position;
        
        zoneImages = zoneImageParent.GetComponentsInChildren<Image>();

        Init();

        Zone.onRetreat += HandleOnRetreat;
        Zone.onGoForth += HandleOnGoForth;
        Zone.onFinishZone += HandleOnFinishZone;

        Hide();

    }

    private void HandleOnFinishZone()
    {
        Show();
        Appear();
    }

    void Show()
    {
        group.SetActive(true);
    }

    void Hide()
    {
        group.SetActive(false);
    }

    void Appear()
    {
        rectTransform.position = initPos + Vector3.right * hideDecal;

        Vector3 p = initPos;

        HOTween.To( rectTransform , tweenDuration , "position", p);
    }

    void Disappear()
    {
        rectTransform.position = initPos;

        Vector3 p = initPos + Vector3.right * hideDecal;

        HOTween.To(rectTransform, tweenDuration, "position", p);

        Invoke("Hide", tweenDuration);
    }

    private void Init()
    {

        Canvas.ForceUpdateCanvases();

        foreach (var item in zoneImages)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < ZoneManager.Instance.zones.Count; i++)
        {
            zoneImages[i].gameObject.SetActive(true);
        }

        UpdateDisplay();
    }

    private void HandleOnGoForth()
    {
        UpdateDisplay();
        Invoke("Disappear", tweenDuration);
    }

    private void HandleOnRetreat()
    {
        Show();
        Invoke("UpdateDisplay", tweenDuration);
        Invoke("Disappear", tweenDuration * 3f);
    }

    void UpdateDisplay()
    {
        int i = 0;
        foreach (var item in zoneImages)
        {
            if ( i < ZoneManager.Instance.zoneIndex )
            {
                item.color = Color.blue;
                Tween.Bounce(item.transform);
            }

            ++i;
        }

        Vector3 p = zoneImages[ZoneManager.Instance.zoneIndex].rectTransform.position;

        HOTween.To( playerRectTransform , tweenDuration , "position" , p , false , EaseType.EaseInOutBounce, 0f );
    }
}

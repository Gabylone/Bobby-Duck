using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System;

public class DisplayGroup : MonoBehaviour {

    public float minWidth = 12f;

    public float tweenDuration = 0.25f;
    float decalY = 150f;

    public GameObject group;

    public RectTransform maskRectTransform;

    public Vector2 initScale = Vector2.zero;
    public Vector2 initPos = Vector2.zero;

    public GameObject closeGroup;

    public bool opened = false;

    public Vector3 closeButtonInitPos = Vector3.zero;

    public bool stopTime = false;

    public virtual void Start()
    {
        //_transform = base.transform;
        initPos = maskRectTransform.anchoredPosition;

        initScale = maskRectTransform.sizeDelta;

        if (closeGroup != null)
            closeGroup.SetActive(false);

        Hide();
    }

    public virtual void Show()
    {
        group.SetActive(true);
    }

    public virtual void Hide()
    {
        group.SetActive(false);
    }

    public virtual void Open()
    {
        Open(true);
    }

    public virtual void Open ( bool hideBottomBar )
    {
        Show();

        if (hideBottomBar && BottomBar.Instance != null) {
            BottomBar.Instance.Down();
        }

        maskRectTransform.anchoredPosition = initPos + Vector2.up * decalY;
        HOTween.To(maskRectTransform, tweenDuration, "anchoredPosition", initPos);

        opened = true;

        UpdateSize();

        Invoke("OpenDelay", tweenDuration);
    }

    public virtual void OpenDelay()
    {
        HOTween.To(maskRectTransform, tweenDuration, "sizeDelta", initScale);

        if (closeGroup != null)
        {
            closeGroup.SetActive(true);
        }

        if ( stopTime)
        {
            Invoke("StopTime", tweenDuration * 2f);
        }
    }

    void StopTime()
    {
        Time.timeScale = 0f;
    }


    public virtual void UpdateSize()
    {
        // NE RIEN METTRE DAUTRE DANS CsETTE FONCTION TU MENTENDS ?!
        maskRectTransform.sizeDelta = new Vector2(minWidth, maskRectTransform.rect.height);
    }

    

    bool showBottomBar = true;

    public virtual void Close(bool _showBottomBar)
    {
        if ( closeGroup != null)
        {
            closeGroup.SetActive(false);
        }

        if (stopTime)
        {
            Time.timeScale = 1f;
        }

        showBottomBar = _showBottomBar;

        HOTween.To(maskRectTransform, tweenDuration, "sizeDelta", new Vector2(minWidth, maskRectTransform.rect.height));

        Invoke("CloseDelay", tweenDuration);
    }

    public virtual void Close()
    {
        Close(true);
    }

    void CloseDelay()
    {
        Invoke("Hide", tweenDuration);

        opened = false;

        HOTween.To(maskRectTransform, tweenDuration, "anchoredPosition", initPos + Vector2.up * decalY);

        if( showBottomBar && BottomBar.Instance != null)
            BottomBar.Instance.Up();
    }
}

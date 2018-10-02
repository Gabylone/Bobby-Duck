using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Holoville.HOTween;
using System;

public class DisplayInfo_Tuto : DisplayGroup {

    public static DisplayInfo_Tuto Instance;

    public RectTransform layoutGroup;
    public RectTransform rectTransform;

    public Image backgroundImage;
    Color initColor;

    public Text titleText;
    public Text descriptionText;

    public GameObject confirmGroup;

    TutoInfo currentTutoInfo;

    public Transform focus_Transform;
    public Transform focus_PreviousParent;
    public Vector2 focus_PreviousPos;
    public Transform focus_Anchor;
    public int focus_ChildIndex = 0;

    bool canClose = false;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        initColor = backgroundImage.color;
    }

    public void Display(TutoInfo tutoInfo)
    {

        CancelInvoke();

        HOTween.Kill(group.transform);
        HOTween.Kill(rectTransform);

        currentTutoInfo = tutoInfo;

        canClose = false;

        confirmGroup.SetActive(true);
        backgroundImage.color = Color.clear;
        HOTween.To(backgroundImage, tweenDuration, "color", initColor);
        backgroundImage.raycastTarget = false;

        // 

        if (focus_Transform != null)
        {
            Focus();
        }

        Show();
        UpdateLayoutGroup();
        Open(false);

        initScale.y = layoutGroup.sizeDelta.y;

        Invoke("StopTime", tweenDuration * 2f);


    }

    private void UpdateLayoutGroup()
    {
        titleText.text = currentTutoInfo.titles[(int)Inventory.currentLanguageType];

        string description = currentTutoInfo.descriptions[(int)Inventory.currentLanguageType];
        string str = description.Remove(0, 1).Remove(description.Length - 2);
        descriptionText.text = str;

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
    }

    public override void UpdateSize()
    {

        maskRectTransform.sizeDelta = new Vector2(minWidth, layoutGroup.sizeDelta.y);
    }

    void StopTime()
    {
        backgroundImage.raycastTarget = true;
        confirmGroup.SetActive(true);

        // change text
        canClose = true;

        Time.timeScale = 0f;

        


    }

    public override void Close(bool b)
    {
        Time.timeScale = 1f;

        base.Close(false);

        CancelInvoke("StopTime");

        confirmGroup.SetActive(false);

        HOTween.To(backgroundImage, tweenDuration, "color", Color.clear);

        Invoke("CheckDoubleTutos", tweenDuration + 0.1f);

        if (focus_Transform != null)
        {
            Unfocus();
        }
    }

    public void Confirm()
    {
        if (!canClose)
            return;

        Close(false);
    }

    private void CheckDoubleTutos()
    {
        switch (currentTutoInfo.step)
        {
            /*case TutorialStep.Patience1:
                Tutorial.Instance.Show(TutorialStep.Patience2);
                break;*/
            case TutorialStep.Tables:
                Tutorial.Instance.Show(TutorialStep.Plates, DisplayUpgrades.Instance.upgradeScrollViews[1].transform);
                break;
            case TutorialStep.Plates:
                Tutorial.Instance.Show(TutorialStep.NewIngredients, DisplayUpgrades.Instance.upgradeScrollViews[2].transform);
                break;
            case TutorialStep.Map:
                Tutorial.Instance.Show(TutorialStep.BlobCust, DisplayUpgrades.Instance.blobButton.transform);
                break;
            default:
                break;
        }


    }

    #region focus
    public void SetFocus(Transform _transform)
    {
        focus_Transform = _transform;

        focus_PreviousParent = focus_Transform.parent;

        focus_PreviousPos = focus_Transform.GetComponent<RectTransform>().anchoredPosition;

        focus_ChildIndex = focus_Transform.GetSiblingIndex();
    }
    void Focus()
    {
        focus_Transform.transform.SetParent(focus_Anchor);

        HOTween.To(focus_Transform, 0.5f, "localPosition", Vector3.zero);
        //focus_Transform.localPosition = Vector3.zero;

        Tween.Bounce(focus_Transform);
    }

    void Unfocus()
    {
        focus_Transform.SetParent(focus_PreviousParent);


        focus_Transform.SetSiblingIndex(focus_ChildIndex);
        //focus_Transform.GetComponent<RectTransform>().anchoredPosition = focus_PreviousPos;

        HOTween.To(focus_Transform.GetComponent<RectTransform>(), tweenDuration, "anchoredPosition", focus_PreviousPos);

        LayoutGroup focusLayoutGroup = GetComponentInParent<LayoutGroup>();

        if ( focusLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(focusLayoutGroup.GetComponent<RectTransform>());
        }

        focus_Transform = null;

    }
    #endregion
}

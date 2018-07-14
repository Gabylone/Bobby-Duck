using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class MemberCreationScrollView : MonoBehaviour
{

    public RectTransform rectTransform;

    public RectTransform contentFitter;

    public GameObject memberCreationPrefab;

    public ApparenceType apparenceType;

    Vector2 initScale;

    public float dur = 0.2f;

    private void Start()
    {
        for (int i = 0; i < CrewCreator.Instance.apparenceGroups[(int)apparenceType].items.Count; ++i)
        {
            GameObject inst = Instantiate(memberCreationPrefab, contentFitter.transform) as GameObject;
            inst.GetComponent<MemberCreationButton_Apparence>().apparenceItem.id = i;
            inst.GetComponent<MemberCreationButton_Apparence>().apparenceItem.apparenceType = apparenceType;
        }

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

    public void OnPointerUp()
    {
        HOTween.To(rectTransform, dur, "sizeDelta", initScale);

        if (MemberCreationButton_Apparence.lastSelected != null)
            MemberCreationButton_Apparence.lastSelected.OnPointerUp();


        foreach (var item in GetComponentsInChildren<MemberCreationButton_Apparence>())
        {
            item.GetComponent<Image>().raycastTarget = false;
        }



    }
}

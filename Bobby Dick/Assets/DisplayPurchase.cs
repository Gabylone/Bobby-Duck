using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPurchase : MonoBehaviour {

    public static DisplayPurchase Instance;

    public GameObject group;

    public Transform itemAnchor;

    ApparenceItem apparenceItem;

    MemberCreatorButton apparenceButton;

    public Button purchaseButton;

    private void Awake()
    {
        Instance = this;
    }

    public void Display ( ApparenceItem _item , MemberCreatorButton _apparenceButton)
    {
        Show();

        this.apparenceItem = _item;
        this.apparenceButton = _apparenceButton;

        apparenceButton.transform.parent = itemAnchor;

        apparenceButton.transform.localPosition = Vector3.zero;

        Tween.Bounce(group.transform);

        if ( apparenceItem.price > PlayerInfo.Instance.pearlAmount)
        {
            purchaseButton.interactable = false;
        }
        else
        {
            purchaseButton.interactable = true;
        }


    }

    void Show()
    {
        group.SetActive(true);
    }

    void Hide()
    {
        group.SetActive(false);
    }

    public void Buy()
    {
        apparenceItem.locked = false;

        apparenceButton.UpdateImage();

        PlayerInfo.Instance.RemovePearl(apparenceItem.price);

        PlayerInfo.Instance.AddApparenceItem(apparenceItem);

        Close();

        PlayerInfo.Instance.Save();
    }

    public void Close()
    {
        apparenceButton.transform.SetParent(apparenceButton.initParent);

        Hide();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPurchase : MonoBehaviour {

    public static DisplayPurchase Instance;

    public GameObject group;

    public Transform itemAnchor;

    ApparenceItem apparenceItem;

    Transform targetTransform;
    Transform initParent;

    public Button purchaseButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hide();
    }

    public void Display ( ApparenceItem _item , Transform _targetTransform)
    {
        Show();

        this.apparenceItem = _item;

        GameObject g = Instantiate(_targetTransform.gameObject, itemAnchor) as GameObject;
        targetTransform = g.transform;
        targetTransform.localPosition = Vector3.zero;

        Tween.Bounce(group.transform);

        if ( apparenceItem.price > PlayerInfo.Instance.pearlAmount)
        {
            purchaseButton.interactable = false;
            purchaseButton.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            purchaseButton.GetComponentInChildren<Text>().color = Color.white;
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

        if ( targetTransform.GetComponent<MemberCreatorButton>() != null)
        {
            targetTransform.GetComponent<MemberCreatorButton>().UpdateImage();
        }
        else
        {
            targetTransform.GetComponent<Map>().UpdateUI();
        }

        PlayerInfo.Instance.RemovePearl(apparenceItem.price);

        PlayerInfo.Instance.AddApparenceItem(apparenceItem);

        Close();

        PlayerInfo.Instance.Save();
    }

    public void Close()
    {
        //targetTransform.SetParent(initParent);
        Destroy(targetTransform.gameObject);

        Hide();
    }

}

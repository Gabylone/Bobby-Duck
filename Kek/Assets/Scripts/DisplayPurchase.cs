using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPurchase : MonoBehaviour {

    public Image backgroundImage;
    public Image image;

    public GameObject group;

    public Text uiTextPrice;
    public Text uiTextName;

    public Button button;

    public virtual void Start()
    {
        Hide();
    }
  

    public virtual void Display(string str, int price)
    {
        Show();

        uiTextName.text = str;
        uiTextPrice.text = price.ToString();

        if (price > Inventory.Instance.gold)
        {
            button.interactable = false;

        }

        Tween.Bounce( transform );

        RegionRayBlocker.onTouchRayblocker += HandleOnTouchRayblocker;

    }

    private void HandleOnTouchRayblocker()
    {
        Hide();
    }

    public virtual void Buy()
    {
        RegionRayBlocker.Instance.Hide();

        Hide();

        Sound.Instance.PlaySound(Sound.Type.Menu7);
    }

    public void Show()
    {
        group.SetActive(true);
    }

    public void Hide()
    {
        group.SetActive(false);
    }
}

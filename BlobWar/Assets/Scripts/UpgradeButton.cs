using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    public enum Type
    {
        Barricade,
    }

    public Type type;

    public int id = 0;

    public GameObject goldGroup;
    public Text goldText;

    public int price = 100;

    public GameObject lockedGroup;

    public Image image;

    public Image gold_Image;

    public Button button;

    public void UpdateUI()
    {
        switch (type)
        {
            case Type.Barricade:
                if (id < Inventory.Instance.barricadeAmount)
                {
                    AlreadyPurshased();
                }
                else if (id == Inventory.Instance.barricadeAmount)
                {
                    if (Inventory.Instance.gold >= price)
                    {
                        CanBePurshased();
                    }
                    else
                    {
                        TooExpensive();
                    }
                }
                else
                {
                    CantBePurshased();
                }
                break;
            default:
                break;
        }
       
    }

    private void TooExpensive()
    {
        button.interactable = false;
        goldGroup.SetActive(true);

        goldText.text = "" + price;

        gold_Image.color = Color.red;
        lockedGroup.SetActive(false);
        //button.image.color = DisplayUpgrades.Instance.cantAffordColor;
    }

    public void CantBePurshased()
    {
        button.interactable = false;
        goldGroup.SetActive(false);
        lockedGroup.SetActive(true);
    }

    public void CanBePurshased()
    {
        button.interactable = true;
        goldGroup.SetActive(true);
        goldText.text = "" + price;
        button.image.color = Color.white;
        gold_Image.color = Color.green;
        lockedGroup.SetActive(false);
    }

    public void AlreadyPurshased()
    {
        button.image.color = Color.grey;
        button.interactable = false;
        goldGroup.SetActive(false);
        lockedGroup.SetActive(false);
    }

    public void OnPointerDown()
    {
        //DisplaySoldierPurchase.Instance.Display();

        Tween.Bounce(transform);
    }
}

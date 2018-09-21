using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    public enum Type
    {
        Table,
        Plate,
        Ingredient,
        Waiter
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
            case Type.Table:
                if (id < Inventory.Instance.tableAmount)
                {
                    AlreadyPurshased();
                }
                else if (id == Inventory.Instance.tableAmount)
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
            case Type.Plate:
                if (id < Inventory.Instance.plateAmount)
                {
                    AlreadyPurshased();
                }
                else if (id == Inventory.Instance.plateAmount)
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
            case Type.Ingredient:

                image.sprite = IngredientManager.Instance.ingredientSprites[id];

                if (Inventory.Instance.ingredientTypes.Contains((Ingredient.Type)id))
                {
                    AlreadyPurshased();
                }
                else
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
                break;
            case Type.Waiter:

                if (id < Inventory.Instance.waiterAmount)
                {
                    AlreadyPurshased();
                }
                else if (id == Inventory.Instance.waiterAmount)
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
        DisplayPurchase.Instance.Display(this);

        Tween.Bounce(transform);
    }
}

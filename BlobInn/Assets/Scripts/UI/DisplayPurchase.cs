using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPurchase : DisplayGroup {

    public static DisplayPurchase Instance;

    public Text goldText;

    public Text titleText;

    public Image image;

    public Image goldImage;

    public Button purchaseButton;

    UpgradeButton currentUpgradeButton;

    private void Awake()
    {
        Instance = this;
    }

    public void Display(UpgradeButton upgradeButton)
    {
        Open();

        //DisplayUpgrades.Instance.Close();

        group.SetActive(true);

        currentUpgradeButton = upgradeButton;

        image.sprite = currentUpgradeButton.image.sprite;
        image.SetNativeSize();

        goldText.text = "" + currentUpgradeButton.price;

        if ( Inventory.Instance.gold < currentUpgradeButton.price)
        {
            goldImage.color = Color.red;
            purchaseButton.interactable = false;
        }
        else
        {
            goldImage.color = Color.white;
            purchaseButton.interactable = true;
        }

        SoundManager.Instance.Play(SoundManager.SoundType.UI_Open);

    }

    public void Confirm()
    {
        Inventory.Instance.RemoveGold(currentUpgradeButton.price);

        switch (currentUpgradeButton.type)
        {
            case UpgradeButton.Type.Table:
                Inventory.Instance.tableAmount++;
                break;
            case UpgradeButton.Type.Plate:
                Inventory.Instance.plateAmount++;
                break;
            case UpgradeButton.Type.Ingredient:
                Inventory.Instance.ingredientTypes.Add((Ingredient.Type)currentUpgradeButton.id);
                break;
            case UpgradeButton.Type.Waiter:
                Inventory.Instance.waiterAmount++;
                break;
            default:
                break;
        }

        Inventory.Instance.Save();

        DisplayUpgrades.Instance.UpdateUI();

        Close();

        SoundManager.Instance.Play(SoundManager.SoundType.Star);


    }

    public override void Close(bool b)
    {
        base.Close(false);

        SoundManager.Instance.Play(SoundManager.SoundType.UI_Close);
    }
}

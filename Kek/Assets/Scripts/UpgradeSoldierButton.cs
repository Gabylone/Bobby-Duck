using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class UpgradeSoldierButton : MonoBehaviour
{
    public delegate void OnClickUpgradeSoldierButton();
    public static OnClickUpgradeSoldierButton onClickUpgradeSoldierButton;

    public SoldierInfo soldierInfo = null;

    public int price = 0;

    public int minPrice = 10;
    public int maxPrice = 100;


    public Image progression_FillImage;
    public Image progression_BackgroundImage;

    public Button button;
    public Text uiText_Price;

    public virtual void Start()
    {
        SoldierInventoryButton.onClickSoldierInventoryButton += HandleOnClickSoldierInventoryButton;
    }
    private void OnDestroy()
    {
        onClickUpgradeSoldierButton = null;
    }

    public virtual void HandleOnClickSoldierInventoryButton(SoldierInfo soldierInfo)
    {
        this.soldierInfo = soldierInfo;

        UpdateButton();

    }

    public virtual void OnClick()
    {
        InventoryManager.Instance.RemoveGold(price);

        UpdateButton();

        onClickUpgradeSoldierButton();

        Sound.Instance.PlaySound(Sound.Type.Upgrade);

    }

    public virtual void UpdateButton()
    {
        uiText_Price.text = "" + price;

        button.interactable = true;

        if (Inventory.Instance.gold < price)
        {
            button.interactable = false;
        }
    }

    public void UpdateBar(int progression, int max)
    {
        float w = progression_BackgroundImage.rectTransform.rect.width;
        float l1 = (float)(progression - 1) / (float)max;
        float l2 = progression / (float)max;

        Vector2 targetScale = new Vector2(-(w) + (l2 * w), 0);
        //progression_FillImage.rectTransform.sizeDelta = new Vector2(-(w) + (l1 * w), 0);
        HOTween.To(progression_FillImage.rectTransform, 0.25f, "sizeDelta", targetScale);

    }

}

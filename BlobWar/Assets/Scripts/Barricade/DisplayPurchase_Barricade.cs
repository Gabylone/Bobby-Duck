using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPurchase_Barricade : DisplayGroup {

    public static DisplayPurchase_Barricade Instance;

    public float maxPrice = 500;
    public float minPrice = 1;

    public int maxBarricade = 15;

    public Text gold_Text;

    public Button purchaseButton;

    public Text barricade_Text;

    public Transform barricadeImage;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    int GetBarricadePrice()
    {
        float l = (float)Inventory.Instance.barricadeAmount / maxBarricade;
        return (int)Mathf.Lerp(minPrice, maxPrice, l);
    }

    public override void Open()
    {
        base.Open();

        UpdateUI();


    }

    private void UpdateUI()
    {
        barricade_Text.text = "" + Inventory.Instance.barricadeAmount;

        if (Inventory.Instance.barricadeAmount == maxBarricade)
        {
            gold_Text.text = "max";
            purchaseButton.interactable = false;
            barricade_Text.color = Color.red;

            return;
        }

        barricade_Text.color = Color.white;
        gold_Text.text = "" + GetBarricadePrice();

        if (Inventory.Instance.gold < GetBarricadePrice())
        {
            purchaseButton.interactable = false;
        }
        else
        {
            purchaseButton.interactable = true;
        }
    }

    public void Buy()
    {
        Inventory.Instance.RemoveGold(GetBarricadePrice());
        Inventory.Instance.AddBarricade();

        Tween.Bounce(barricadeImage);

        UpdateUI();

        Inventory.Instance.Save();

        BarricadeInventoryButton.Instance.UpdateDisplay();
    }

}

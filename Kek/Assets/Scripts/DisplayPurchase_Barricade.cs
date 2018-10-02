using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPurchase_Barricade : DisplayPurchase {

    public float maxPrice = 500;
    public float minPrice = 1;

    public int maxBarricade = 10;

    int price;

    public override void Start()
    {
        base.Start();
        BuyBarricadeButton.onClickBuyBarricadeButton += HandleOnClickBuyBarricadeButton;
    }
    private void OnDestroy()
    {
        BuyBarricadeButton.onClickBuyBarricadeButton -= HandleOnClickBuyBarricadeButton;
    }

    private void HandleOnClickBuyBarricadeButton()
    {
        if ( Inventory.Instance.barricadeCount == maxBarricade)
        {
            uiTextPrice.text = "x";
            uiTextName.text = "Trop de barricade";
            button.interactable = false;
            return;
        }

        float l = (float)Inventory.Instance.barricadeCount / maxBarricade;
        price = (int)Mathf.Lerp( minPrice , maxPrice , l );
        Display("Nouvelle Barricade ?", price);

    }

    public override void Buy()
    {
        base.Buy();

        InventoryManager.Instance.RemoveGold(price);
        Inventory.Instance.AddBarricade();
    }

}

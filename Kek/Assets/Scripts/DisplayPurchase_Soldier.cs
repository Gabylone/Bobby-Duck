using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPurchase_Soldier : DisplayPurchase {

    public int soldierPrice;

    public int minPrice = 20;

    public int maxPrice = 200;

    public override void Start()
    {
        base.Start();
        BuySoldierButton.onClickBuySoldierButton += HandleOnClickBuySoldierButton;
    }

    private void HandleOnClickBuySoldierButton()
    {
        RegionRayBlocker.Instance.Show();

        float l = (float)Inventory.Instance.soldierInfos.Count / InventoryManager.Instance.maxSoldierAmount;
        soldierPrice = (int)Mathf.Lerp(minPrice, maxPrice, l);

        if (Inventory.Instance.soldierInfos.Count == InventoryManager.Instance.maxSoldierAmount)
        {
            Display("Nombre max soldat atteint", soldierPrice);
            button.interactable = false;
        }
        else
        {
            Display("Nouveau Soldat ?", soldierPrice);

            if ( Inventory.Instance.gold < soldierPrice)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
            
        }
    }

    public override void Buy()
    {
        base.Buy();

        InventoryManager.Instance.RemoveGold(soldierPrice);
        SoldierInfo newSoldierInfo = new SoldierInfo();
        newSoldierInfo.name = NameGeneration.Instance.randomWord;
        newSoldierInfo.colorID = Random.Range(0, InventoryManager.Instance.soldierColors.Length);
        Inventory.Instance.AddSoldier(newSoldierInfo);
    }
}

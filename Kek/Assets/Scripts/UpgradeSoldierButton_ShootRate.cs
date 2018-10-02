using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSoldierButton_ShootRate: UpgradeSoldierButton {

    public override void HandleOnClickSoldierInventoryButton(SoldierInfo soldierInfo)
    {
        base.HandleOnClickSoldierInventoryButton(soldierInfo);

        UpdateBar(soldierInfo.shootRateLevel, soldierInfo.maxShootRateLevel);

    }

    public override void OnClick()
    {
        soldierInfo.shootRateLevel++;

        UpdateBar(soldierInfo.shootRateLevel, soldierInfo.maxShootRateLevel);

        base.OnClick();

    }

    public override void UpdateButton()
    {
        price = minPrice + (int)(soldierInfo.shootRateLevel * maxPrice / soldierInfo.maxShootRateLevel);

        base.UpdateButton();

        if (soldierInfo.shootRateLevel == soldierInfo.maxShootRateLevel)
        {
            button.interactable = false;
        }
    }


}


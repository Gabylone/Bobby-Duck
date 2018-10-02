using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSoldierButton_SpeedBetweenLigns : UpgradeSoldierButton {


    public override void Start()
    {
        base.Start();
    }

    public override void HandleOnClickSoldierInventoryButton(SoldierInfo soldierInfo)
    {
        base.HandleOnClickSoldierInventoryButton(soldierInfo);

        UpdateBar(soldierInfo.speedBetweenLignsLevel, soldierInfo.maxSpeedBetweenLignsLevel);

    }

    public override void OnClick()
    {
        soldierInfo.speedBetweenLignsLevel++;

        UpdateBar(soldierInfo.speedBetweenLignsLevel, soldierInfo.maxSpeedBetweenLignsLevel);

        base.OnClick();

    }

    public override void UpdateButton()
    {
        price = minPrice + (int)(soldierInfo.speedBetweenLignsLevel * maxPrice / soldierInfo.maxSpeedBetweenLignsLevel);
        Debug.Log(price);

        base.UpdateButton();

        if (soldierInfo.speedBetweenLignsLevel == soldierInfo.maxSpeedBetweenLignsLevel)
        {
            button.interactable = false;
        }
    }


}


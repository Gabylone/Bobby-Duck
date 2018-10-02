using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInventoryButton : InventoryButton {

    public SoldierInfo soldierInfo;

    public GameObject soldierPrefab;

    public Image soldierImage;

    public bool upgrade = false;

    public delegate void OnClickSoldierInventoryButton(SoldierInfo soldierInfo);
    public static OnClickSoldierInventoryButton onClickSoldierInventoryButton;
    private void OnDestroy()
    {
        onClickSoldierInventoryButton = null;
    }

    public override void Activate()
    {
        base.Activate();

        Hide();

        if ( upgrade)
        {
            UpgradeSoldier();
        }
        else
        {
            PlaceSoldier();
        }

        if (onClickSoldierInventoryButton != null)
        {
            onClickSoldierInventoryButton(soldierInfo);
        }
    }

    private void PlaceSoldier()
    {
        GameObject soldier = Instantiate(soldierPrefab, ZoneManager.Instance.soldierParent) as GameObject;

        soldier.GetComponent<AISoldier>().SetInfo(soldierInfo);
        soldier.GetComponent<AISoldier>().PlaceStart();
    }

    public void SetSoldierInfo(SoldierInfo item)
    {

        Show();

        //soldierImage.color = item.GetColor;

        soldierInfo = item;
    }

    private void UpgradeSoldier()
    {
        RegionRayBlocker.Instance.Show();
        RegionRayBlocker.onTouchRayblocker += HandleOnTouchRayblocker;
    }

    private void HandleOnTouchRayblocker()
    {
        Show();
    }

   
}

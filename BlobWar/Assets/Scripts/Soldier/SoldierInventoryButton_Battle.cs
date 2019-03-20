using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInventoryButton_Battle : SoldierInventoryButton {

    public GameObject soldierPrefab;

    public override void Activate ()
	{
		base.Activate ();

		PlaceSoldier ();

        Hide();
	}

	private void PlaceSoldier()
	{
		GameObject soldierObj = Instantiate(soldierPrefab, SoldierManager.Instance.soldierParent) as GameObject;

        Soldier soldier = soldierObj.GetComponent<SoldierAI>();

        soldier.PlaceStart();

        soldier.apparence.SetSpriteIDs(soldierInfo.apparenceIDs);

        DisplayCharge.Instance.ResetCharge ();
    }


}

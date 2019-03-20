using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierInventoryButton_Map : SoldierInventoryButton {


	public override void Activate ()
	{
		base.Activate ();

		DisplayUpgradeSoldier.Instance.DiplaySoldier (soldierInfo);

        Tween.Bounce(transform);
	}

    public override void SetSoldierInfo(SoldierInfo item)
    {
        base.SetSoldierInfo(item);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInventoryButton : InventoryButton {

    public SoldierInfo soldierInfo;


    public BlobApparence_UI blobApparence;

    public override void Activate()
    {
        base.Activate();

    }
    

    public virtual void SetSoldierInfo(SoldierInfo item)
    {
        Show();

        soldierInfo = item;

        blobApparence.SetSpriteIDs(item.apparenceIDs);


    }

    private void UpgradeSoldier()
    {
        
    }

   
}

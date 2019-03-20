using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgradeSoldier : DisplayGroup {

	public static DisplayUpgradeSoldier Instance;

    public SoldierInfo currentSoldierInfo;

    public Text uiText_SoldierLevel;
    public Text uiText_SoldierName;

    public FillBar[] fillBars;
    public Text[] uiTexts;

    public BlobApparence blob_Apparence;

    public enum UpgradeType {

		Speed,
		ShootRate

	}

	void Awake () {
		Instance = this;
	}

	public override void Start ()
	{
		base.Start ();

	}

	public override void Update ()
	{
		base.Update ();
	}

	public void DiplaySoldier(SoldierInfo soldierInfo)
    {
		Open ();

        currentSoldierInfo = soldierInfo;

        blob_Apparence.SetSpriteIDs(currentSoldierInfo.apparenceIDs);

        UpdateSoldierInfo();

		SoundManager.Instance.Play (SoundManager.SoundType.UI_Bip);
    }

	public void UpdateSoldierInfo()
    {
        int level = 1 + (currentSoldierInfo.GetStat(SoldierInfo.Stat.ShootRate) + currentSoldierInfo.GetStat(SoldierInfo.Stat.SpeedBetweenLigns));

        uiText_SoldierLevel.text = "" + level;
        uiText_SoldierName.text = currentSoldierInfo.name;

        for (int i = 0; i < fillBars.Length; i++)
        {
            fillBars[i].UpdateUI( currentSoldierInfo.GetStat( (SoldierInfo.Stat)i ) , currentSoldierInfo.GetStatMax((SoldierInfo.Stat)i));

            if ( currentSoldierInfo.GetStat( (SoldierInfo.Stat)i) == currentSoldierInfo.GetStatMax( (SoldierInfo.Stat)i) )
            {
                fillBars[i].button.interactable = false;
                uiTexts[i].text = "MAX";
            }
            else
            {
                uiTexts[i].text = "" + currentSoldierInfo.GetPrice((SoldierInfo.Stat)i);
            }

            if (Inventory.Instance.gold < currentSoldierInfo.GetPrice((SoldierInfo.Stat)i))
            {
                fillBars[i].button.interactable = false;
            }
            else
            {
                fillBars[i].button.interactable = true;
            }
        }
    }

    public void UpgradeStat ( int i)
    {
        int stat = currentSoldierInfo.GetStat((SoldierInfo.Stat)i);

        currentSoldierInfo.SetStat((SoldierInfo.Stat)i, stat + 1 );
        Inventory.Instance.RemoveGold(currentSoldierInfo.GetPrice((SoldierInfo.Stat)i));

        Tween.Bounce( fillBars[i].button.transform );

        UpdateSoldierInfo();

        Inventory.Instance.Save();

    }

    public override void Close()
    {
        base.Close();



    }

}

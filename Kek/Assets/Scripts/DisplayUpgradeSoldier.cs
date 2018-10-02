using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgradeSoldier : MonoBehaviour {

    public SoldierInfo currentSoldierInfo;

    public GameObject group;

    public Text uiText_SoldierLevel;
    public Text uiText_SoldierName;

    UpgradeSoldierButton[] upgradeSoldierButtons;

    public Image soldierimage;

    public void Show()
    {
        group.SetActive(true);

    }

    public void Hide()
    {
        group.SetActive(false);
    }

	// Use this for initialization
	void Start () {

        SoldierInventoryButton.onClickSoldierInventoryButton += HandleOnClickSoldierInventoryButton;

        UpgradeSoldierButton.onClickUpgradeSoldierButton += HandleOnClickUpgradeSoldierButton;

        upgradeSoldierButtons = GetComponentsInChildren<UpgradeSoldierButton>(true);

        Hide();

    }
    private void OnDestroy()
    {
        UpgradeSoldierButton.onClickUpgradeSoldierButton -= HandleOnClickUpgradeSoldierButton;
        SoldierInventoryButton.onClickSoldierInventoryButton -= HandleOnClickSoldierInventoryButton;

    }
    private void HandleOnClickUpgradeSoldierButton()
    {
        UpdateSoldierInfo();
    }

    private void HandleOnClickSoldierInventoryButton(SoldierInfo soldierInfo)
    {
        RegionRayBlocker.onTouchRayblocker += Hide;

        currentSoldierInfo = soldierInfo;

        foreach (var item in upgradeSoldierButtons)
        {
            item.HandleOnClickSoldierInventoryButton(soldierInfo);
        }

        Show();

        UpdateSoldierInfo();

        Sound.Instance.PlaySound(Sound.Type.Fall4);
    }

    private void UpdateSoldierInfo()
    {
        int level = 1 + (currentSoldierInfo.shootRateLevel + currentSoldierInfo.speedBetweenLignsLevel);

        uiText_SoldierLevel.text = "niveau " + level;

        uiText_SoldierName.text = currentSoldierInfo.name;

        //soldierimage.color = currentSoldierInfo.GetColor;
    }
}

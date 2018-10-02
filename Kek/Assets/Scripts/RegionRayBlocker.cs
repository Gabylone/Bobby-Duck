using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionRayBlocker : MonoBehaviour {

    public static RegionRayBlocker Instance;

    public GameObject group;
    public Image image;

    private void Awake()
    {
        Instance = this;
    }

    public delegate void OnTouchRayblocker();
    public static OnTouchRayblocker onTouchRayblocker;
    private void OnDestroy()
    {
        onTouchRayblocker = null;

        RegionButton.onClickRegionButton -= HandleOnClickRegionButton;

        BuyBarricadeButton.onClickBuyBarricadeButton -= Show;
        BuySoldierButton.onClickBuySoldierButton -= Show;
    }

    void Start ()
    {
        RegionButton.onClickRegionButton += HandleOnClickRegionButton;

        BuyBarricadeButton.onClickBuyBarricadeButton += Show;
        BuySoldierButton.onClickBuySoldierButton += Show;
    }

    private void HandleonClickSoldierInventoryButton(SoldierInfo soldierInfo)
    {
        Show();
    }

    private void HandleOnClickRegionButton(RegionButton regionButton)
    {
        if ( regionButton.selected)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        group.SetActive(true);
    }

    public void Hide()
    {

        Sound.Instance.PlaySound(Sound.Type.Menu5);
        group.SetActive(false);
        onTouchRayblocker = null;

    }

    
    public void Press()
    {
        if (onTouchRayblocker != null)
        {
            onTouchRayblocker();
            Hide();
        }
    }
}

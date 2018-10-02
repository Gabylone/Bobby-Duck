using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class DisplayInventory : MonoBehaviour {

    public static DisplayInventory Instance;

    public GameObject soldierInventoryButtonPrefab;

    public Transform targetParent;

    public RectTransform rectTransform;

    public bool showPlayerSoldierInfo = false;

    public float tweenDuration = 1f;

    public SoldierInventoryButton[] soldierInventoryButtons;

    Vector2 initPos;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitDisplay();

        UpdateDisplay();

        initPos = rectTransform.anchoredPosition;

        Inventory.onAddSoldier += UpdateDisplay;
    }
    private void OnDestroy()
    {
        Inventory.onAddSoldier -= UpdateDisplay;
    }

    private void InitDisplay()
    {
        soldierInventoryButtons = new SoldierInventoryButton[InventoryManager.Instance.maxSoldierAmount + 1];

        for (int i = 0; i < InventoryManager.Instance.maxSoldierAmount + 1; i++)
        {
            GameObject soldierInventoryButton = Instantiate(soldierInventoryButtonPrefab, targetParent) as GameObject;
            soldierInventoryButtons[i] = soldierInventoryButton.GetComponent<SoldierInventoryButton>();
        }
    }

    public void Hide()
    {
        HOTween.To( rectTransform , tweenDuration , "anchoredPosition" , initPos - Vector2.up * rectTransform.sizeDelta.y , false , EaseType.Linear , 0f );
    }

    public void Show ()
    {
        HOTween.To(rectTransform, tweenDuration, "anchoredPosition", initPos , false , EaseType.Linear , 0f );
    }

    private void UpdateDisplay()
    {
        foreach (var item in soldierInventoryButtons)
        {
            item.Hide();
        }

        int a = 0;

        if ( showPlayerSoldierInfo)
        {
            soldierInventoryButtons[0].SetSoldierInfo(Inventory.Instance.playerSoldierInfo);
            ++a;
        }

        foreach (var item in Inventory.Instance.soldierInfos)
        {
            soldierInventoryButtons[a].SetSoldierInfo(item);
            ++a;
        }
    }
}

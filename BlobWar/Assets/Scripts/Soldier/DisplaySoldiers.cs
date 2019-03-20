using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class DisplaySoldiers : MonoBehaviour {

    public static DisplaySoldiers Instance;

    public GameObject soldierInventoryButtonPrefab;

    public Transform targetParent;

    public bool showPlayerSoldierInfo = false;

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
    }

    private void InitDisplay()
    {
		soldierInventoryButtons = new SoldierInventoryButton[Inventory.Instance.maxSoldierAmount + 1];

		for (int i = 0; i < soldierInventoryButtons.Length; i++)
        {
            GameObject soldierInventoryButton = Instantiate(soldierInventoryButtonPrefab, targetParent) as GameObject;
            soldierInventoryButtons[i] = soldierInventoryButton.GetComponent<SoldierInventoryButton>();
        }
    }

	public void UpdateDisplay()
	{
		foreach (var item in soldierInventoryButtons)
		{
            item.gameObject.SetActive(false);
        }

		int a = 0;

		if ( showPlayerSoldierInfo)
		{
            soldierInventoryButtons[0].gameObject.SetActive(true);
            soldierInventoryButtons[0].SetSoldierInfo(Inventory.Instance.soldierInfo_Player);
			++a;
		}



		foreach (var item in Inventory.Instance.soldierInfos)
		{
            soldierInventoryButtons[0].gameObject.SetActive(true);
            soldierInventoryButtons[a].SetSoldierInfo(item);
			++a;
		}
	}
    
}

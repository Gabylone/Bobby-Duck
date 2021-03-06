﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ActionGroup : MonoBehaviour {

	[SerializeField]
	private InventoryActionButton[] inventoryActionButtons;

	public enum ButtonType {
		Eat,
		Equip,

        PurchaseAndEquip,
        Unequip,
        Throw,
		Purchase,
		Sell,
		PickUp,

		None
	}

	bool visible = false;

    public void UpdateButtons(ButtonType[] buttonTypes) {

        int a = (int)buttonTypes[0];

        HideAll();

        bool canThrow = true;

        switch (LootUI.Instance.SelectedItem.category)
        {

            case ItemCategory.Weapon:
            case ItemCategory.Clothes:

                if (LootUI.Instance.SelectedItem == CrewMember.GetSelectedMember.GetEquipment(LootUI.Instance.SelectedItem.EquipmentPart)
                    &&
                    DisplayItem_Loot.selectedDisplayItem.index == 0
                    &&
                    LootUI.Instance.currentSide == Crews.Side.Player
                    )
                {

                    canThrow = false;
                    a = (int)ButtonType.Unequip;
                }
                else
                {
                    if (LootUI.Instance.categoryContentType == CategoryContentType.Inventory)
                    {
                        a = (int)ButtonType.Equip;
                    }

                }
                break;
            default:
                break;
        }

		inventoryActionButtons [a].gameObject.SetActive (true);
		Tween.Bounce (inventoryActionButtons [(int)buttonTypes [0]].transform);

		if (buttonTypes.Length > 1 && canThrow) {
			inventoryActionButtons [(int)buttonTypes [1]].gameObject.SetActive (true);
			Tween.Bounce (inventoryActionButtons [(int)buttonTypes [1]].transform);
		}

	}

    public void HideAll()
    {
        foreach (var item in inventoryActionButtons)
        {
            item.gameObject.SetActive(false);
        }
    }
}

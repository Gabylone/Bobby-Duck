using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAttack : MonoBehaviour {

	Image image;
	Text uiText;

	// Use this for initialization
	void Start () {

		image = GetComponent<Image> ();
		uiText = GetComponentInChildren<Text> ();

		CrewInventory.Instance.onOpenInventory += HandleOpenInventory;
		LootUI.useInventory += HandleUseInventory;
		StatButton.onClickStatButton += HandleOnClickStatButton;
		CrewInventory.onRemoveItemFromMember += HandleOnRemoveItemFromMember;

		UpdateUI (CrewMember.GetSelectedMember);

	}


	void HandleOnRemoveItemFromMember (Item item)
	{
		UpdateUI (CrewMember.GetSelectedMember);	
	}

	void HandleOnClickStatButton ()
	{
		UpdateUI (CrewMember.GetSelectedMember);
	}

	void HandleOpenInventory (CrewMember crewMember)
	{
		UpdateUI (crewMember);
	}

	void UpdateUI (CrewMember crewMember) {
		uiText.text = crewMember.Attack.ToString ();
		Tween.Bounce (transform);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{

		switch (actionType) {
		case InventoryActionType.Equip:
		case InventoryActionType.PurchaseAndEquip:
			UpdateUI (CrewMember.GetSelectedMember);
			break;
		default:
			break;
		}
	}
}

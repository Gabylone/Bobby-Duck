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

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		LootUI.useInventory += HandleUseInventory;

		HandleOpenInventory (CrewMember.selectedMember);

	}

	void HandleOpenInventory (CrewMember crewMember)
	{
		uiText.text = crewMember.Attack.ToString ();
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Equip ) {
			if (LootUI.Instance.SelectedItem.category == ItemCategory.Weapon) {
				Bounce ();
			}
		}

	}

	void Bounce () 
	{
		Tween.Bounce (transform);
	}
}

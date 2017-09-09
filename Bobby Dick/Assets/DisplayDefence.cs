using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDefence : MonoBehaviour {

	Image image;
	Text uiText;

	// Use this for initialization
	void Awake () {

		image = GetComponent<Image> ();
		uiText = GetComponentInChildren<Text> ();

		PlayerLoot.Instance.openInventory += HandleOnCardUpdate;
		PlayerLoot.Instance.LootUI.useInventory += HandleUseInventory;

	}

	void HandleOnCardUpdate (CrewMember crewMember)
	{
		uiText.text = crewMember.Defense.ToString ();
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Equip ) {
			if (PlayerLoot.Instance.LootUI.SelectedItem.category == ItemCategory.Clothes) {
				Bounce ();
			}
		}
	}

	void Bounce () 
	{
		Tween.Bounce (transform);
	}
}

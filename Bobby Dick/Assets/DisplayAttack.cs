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
		StatButton.onClickStatButton += HandleOnClickStatButton;

		UpdateUI (CrewMember.selectedMember);

	}

	void HandleOnClickStatButton ()
	{
		UpdateUI (CrewMember.selectedMember);
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
		if ( actionType == InventoryActionType.Equip ) {
			if (LootUI.Instance.SelectedItem.category == ItemCategory.Weapon) {
				UpdateUI (CrewMember.selectedMember);
			}
		}

	}
}

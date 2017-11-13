using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHunger : MonoBehaviour {

	public Image fillImage;

	// Use this for initialization
	void Start () {

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		LootUI.useInventory += HandleUseInventory;

		HandleOpenInventory (CrewMember.selectedMember);
	}

	void HandleOpenInventory (CrewMember crewMember)
	{
		UpdateImage ();
	}

	void UpdateImage ()
	{
		fillImage.fillAmount = 1 - ( (float)CrewMember.selectedMember.CurrentHunger / (float)CrewMember.selectedMember.maxHunger );
		Tween.Bounce (transform);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Eat ) {
			UpdateImage ();
		}
	}
}

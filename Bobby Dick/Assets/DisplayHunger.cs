using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHunger : MonoBehaviour {

	public Image fillImage;

	// Use this for initialization
	void Start () {

		CrewInventory.Instance.openInventory += HandleOnCardUpdate;
		LootUI.useInventory += HandleUseInventory;

	}

	void HandleOnCardUpdate (CrewMember crewMember)
	{
		fillImage.fillAmount = 1 - ( (float)crewMember.CurrentHunger / (float)crewMember.maxHunger );
	}

	void Bounce() {
		Tween.Bounce (fillImage.transform);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Eat ) {
			Bounce ();
		}
	}
}

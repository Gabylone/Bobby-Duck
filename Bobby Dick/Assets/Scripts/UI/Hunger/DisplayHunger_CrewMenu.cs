using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHunger_CrewMenu : DisplayHunger {

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();

		CrewInventory.Instance.onOpenInventory += HandleOpenInventory;
		LootUI.useInventory += HandleUseInventory;

		HandleOpenInventory (CrewMember.GetSelectedMember);
	}

	void HandleOpenInventory (CrewMember crewMember)
	{
		UpdateHungerIcon (CrewMember.GetSelectedMember);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Eat ) {
			UpdateHungerIcon (CrewMember.GetSelectedMember);
		}
	}

	void OnDestroy () {
		CrewInventory.Instance.onOpenInventory -= HandleOpenInventory;
		LootUI.useInventory -= HandleUseInventory;
	}
}
